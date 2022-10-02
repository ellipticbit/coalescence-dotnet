﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using EllipticBit.Hotwire.Shared;

using Microsoft.AspNetCore.WebUtilities;

namespace EllipticBit.Hotwire.Client
{
	internal sealed class HotwireRequestBuilder : IHotwireRequestBuilder
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly HotwireRequestOptions options;
		private readonly HttpMethod method;
		private readonly IEnumerable<IHotwireSerializer> serializers;
		private readonly IEnumerable<IHotwireAuthentication> authenticators;

		private readonly List<string> path = new();
		private readonly Dictionary<string, IEnumerable<string>> query = new();
		private readonly Dictionary<string, IEnumerable<string>> headers = new();
		private IHotwireAuthentication authentication = null;
		private string tenantId = null;
		private string userId = null;
		private TimeSpan timeout = TimeSpan.FromSeconds(100);
		private bool noRetry = false;

		private HotwireContentItem content;
		private HotwireMultipartContentBuilder multipartContentBuilder = null;
		private HttpContent cachedContent = null;

		public HotwireRequestBuilder(HttpMethod method, IHttpClientFactory httpClientFactory, HotwireRequestOptions options, IEnumerable<IHotwireSerializer> serializers, IEnumerable<IHotwireAuthentication> authenticators) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
			this.method = method;
			this.serializers = serializers;
			this.authenticators = authenticators;
		}

		public IHotwireRequestBuilder Path(params string[] parameter) {
			path.AddRange(parameter.Select(a => a.Trim().Trim('/', '&')));
			return this;
		}

		public IHotwireRequestBuilder Path(string parameter) {
			path.Add(parameter.Trim().Trim('/', '&'));
			return this;
		}

		public IHotwireRequestBuilder Path(byte[] parameter) {
			path.Add(WebEncoders.Base64UrlEncode(parameter));
			return this;
		}

		public IHotwireRequestBuilder Path<T>(T parameter) where T : struct, IConvertible {
			path.Add(Convert.ToString(parameter));
			return this;
		}

		public IHotwireRequestBuilder Path<T>(T? parameter) where T : struct, IConvertible {
			path.Add(parameter == null ? "null" : Convert.ToString(parameter));
			return this;
		}

		public IHotwireRequestBuilder Query(string key, IEnumerable<string> values) {
			query.Add(key, values.Select(a => a.Trim()));
			return this;
		}

		public IHotwireRequestBuilder Query(string key, IEnumerable<byte[]> values) {
			query.Add(key, values.Select(WebEncoders.Base64UrlEncode));
			return this;
		}

		public IHotwireRequestBuilder Query<T>(string key, IEnumerable<T> values) where T : struct, IConvertible {
			query.Add(key, values.Select(a => Convert.ToString(a)));
			return this;
		}

		public IHotwireRequestBuilder Query<T>(string key, IEnumerable<T?> values) where T : struct, IConvertible {
			query.Add(key, values.Select(a => a == null ? "null" : Convert.ToString(a)));
			return this;
		}

		public IHotwireRequestBuilder Header(string key, IEnumerable<string> values) {
			headers.Add(key, values.Select(a => a.Trim()));
			return this;
		}

		public IHotwireRequestBuilder Header(string key, IEnumerable<byte[]> values) {
			headers.Add(key, values.Select(Convert.ToBase64String));
			return this;
		}

		public IHotwireRequestBuilder Header<T>(string key, IEnumerable<T> values) where T : struct, IConvertible {
			headers.Add(key, values.Select(a => Convert.ToString(a)));
			return this;
		}

		public IHotwireRequestBuilder Header<T>(string key, IEnumerable<T?> values) where T : struct, IConvertible {
			headers.Add(key, values.Select(a => a == null ? "null" : Convert.ToString(a)));
			return this;
		}

		public IHotwireRequestBuilder Serialized<T>(T content, string contentType = null)
		{
			this.content = new HotwireContentItem(HttpContentScheme.Serialized, content, contentType);
			return this;
		}

		public IHotwireRequestBuilder ByteArray(byte[] content, string contentType = null)
		{
			this.content = new HotwireContentItem(HttpContentScheme.Binary, content, contentType);
			return this;
		}

		public IHotwireRequestBuilder Stream(Stream content, string contentType = null)
		{
			this.content = new HotwireContentItem(HttpContentScheme.Stream, content, contentType);
			return this;
		}

		public IHotwireRequestBuilder Text(string content, string contentType = null)
		{
			this.content = new HotwireContentItem(HttpContentScheme.Text, content, contentType);
			return this;
		}

		public IHotwireRequestBuilder FormUrlEncoded(Dictionary<string, string> content)
		{
			this.content = new HotwireContentItem(HttpContentScheme.FormUrlEncoded, content, "application/x-www-form-urlencoded");
			return this;
		}

		public IHotwireRequestBuilder Content(HttpContent content) {
			this.content = new HotwireContentItem(content);
			return this;
		}

		public IHotwireMultipartContentBuilder Multipart()
		{
			this.multipartContentBuilder = new HotwireMultipartContentBuilder(false, this, options, serializers);
			return this.multipartContentBuilder;
		}

		public IHotwireMultipartContentBuilder MultipartForm()
		{
			this.multipartContentBuilder = new HotwireMultipartContentBuilder(true, this, options, serializers);
			return this.multipartContentBuilder;
		}

		public IHotwireRequestBuilder Authentication(string scheme) {
			this.authentication = string.IsNullOrWhiteSpace(scheme) ? null : authenticators.GetHotwireAuthentication(scheme);
			return this;
		}

		public IHotwireRequestBuilder User(string userId) {
			this.userId = userId;
			return this;
		}

		public IHotwireRequestBuilder Tenant(string tenantId) {
			this.tenantId = tenantId;
			return this;
		}

		public IHotwireRequestBuilder NoRetry() {
			this.noRetry = true;
			return this;
		}

		public IHotwireRequestBuilder Timeout(TimeSpan timeout) {
			this.timeout = timeout;
			return this;
		}

		public async Task<IHotwireResponse> Send() {
			int retries = 0;
			HttpResponseMessage response = null;
			using var http = string.IsNullOrWhiteSpace(options.HttpClientId) ? httpClientFactory.CreateClient() : httpClientFactory.CreateClient(options.HttpClientId);
			http.Timeout = timeout;

			while ((!noRetry && retries > 1) || retries > options.MaxRetryCount) {
				using var rm = await BuildRequest();
				response = await http.SendAsync(rm, HttpCompletionOption.ResponseHeadersRead);
				if ((int)response.StatusCode == 429) break; // Add 429 handling here
				if ((int)response.StatusCode >= 500) break; // 5xx Errors on not recoverable on the client, so exit early.
				if (response.StatusCode == HttpStatusCode.Unauthorized && await authentication.ContinueOnFailure(userId, tenantId) == false) break; // Cancel or continue the request as indicated by the failure handler.
				retries++;
			}

			return new HotwireResponse(response, options, serializers, authenticators);
		}

		private async Task<HttpRequestMessage> BuildRequest() {
			var uri = new StringBuilder(string.Join("/", path), 2048);

			//Build query string if any
			if (query.Any(a => a.Value != null && a.Value.Any(b => !string.IsNullOrWhiteSpace(b))))
			{
				uri.Append("?");
				uri.Append(string.Join("&",
					query.Where(a => a.Value != null)
						.SelectMany(a => a.Value.Select(b => new KeyValuePair<string, string>(a.Key, b)))
						.Select(a => $"{a.Key}={a.Value}")));
			}

			using var rm = new HttpRequestMessage(method, uri.ToString());

			//Add any additional headers
			if (headers.Any(a => a.Value != null && a.Value.Any(b => !string.IsNullOrWhiteSpace(b))))
			{
				foreach (var h in headers)
				{
					rm.Headers.Add(h.Key, h.Value);
				}
			}

			if (authentication != null) rm.Headers.Authorization = new AuthenticationHeaderValue(authentication.Scheme, await authentication.Get(userId, tenantId));

			//Get multipart content from builder if any.
			if (this.cachedContent == null) {
				if (multipartContentBuilder != null) {
					this.cachedContent = rm.Content = await multipartContentBuilder.Build();
				}
				else if (content != null) {
					this.cachedContent = rm.Content = await content.Build(serializers);
				}
			}
			else {
				rm.Content = this.cachedContent;
			}

			return rm;
		}
	}
}
