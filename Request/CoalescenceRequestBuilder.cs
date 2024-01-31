using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EllipticBit.Coalescence.Shared;
using EllipticBit.Coalescence.Shared.Request;
using Microsoft.AspNetCore.WebUtilities;

namespace EllipticBit.Coalescence.Request
{
	internal sealed class CoalescenceRequestBuilder : ICoalescenceRequestBuilder
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly CoalescenceRequestOptions options;
		private readonly HttpMethod method;

		private readonly List<string> path = new();
		private readonly Dictionary<string, IEnumerable<string>> query = new();
		private readonly Dictionary<string, IEnumerable<string>> headers = new();
		private ICoalescenceAuthentication authentication = null;
		private IEnumerable<ICoalescenceAuthentication> authenticators;
		private TimeSpan timeout = TimeSpan.FromSeconds(100);
		private bool noRetry = false;

		private CoalescenceContentItem content;
		private CoalescenceMultipartContentBuilder multipartContentBuilder = null;
		private HttpContent cachedContent = null;

		public CoalescenceRequestBuilder(HttpMethod method, IHttpClientFactory httpClientFactory, IEnumerable<ICoalescenceAuthentication> authenticators, CoalescenceRequestOptions options) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
			this.method = method;
			this.authenticators = authenticators;
		}

		public ICoalescenceRequestBuilder Path(params string[] parameter) {
			path.AddRange(parameter.Select(a => WebUtility.UrlEncode(a.Trim().Trim('/', '&'))));
			return this;
		}

		public ICoalescenceRequestBuilder Path(string parameter) {
			path.Add(WebUtility.UrlEncode(parameter.Trim().Trim('/', '&')));
			return this;
		}

		public ICoalescenceRequestBuilder Path(byte[] parameter) {
			path.Add(WebEncoders.Base64UrlEncode(parameter));
			return this;
		}

		public ICoalescenceRequestBuilder Path<T>(T parameter) where T : unmanaged, IComparable, IFormattable
		{
			if (typeof(T) == typeof(DateTime)) {
				path.Add(WebUtility.UrlEncode(((DateTime)(object)parameter).ToString(options.DateTimeFormatString)));
			}
			else if (typeof(T) == typeof(DateTimeOffset)) {
				path.Add(WebUtility.UrlEncode(((DateTimeOffset)(object)parameter).ToString(options.DateTimeFormatString)));
			}
			else {
				path.Add(WebUtility.UrlEncode(Convert.ToString(parameter)));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Path<T>(T? parameter) where T : unmanaged, IComparable, IFormattable
		{
			if (typeof(T) == typeof(DateTime)) {
				path.Add(parameter == null ? "null" : WebUtility.UrlEncode(((DateTime?)(object)parameter)?.ToString(options.DateTimeFormatString)));
			}
			else if (typeof(T) == typeof(DateTimeOffset)) {
				path.Add(parameter == null ? "null" : WebUtility.UrlEncode(((DateTimeOffset?)(object)parameter)?.ToString(options.DateTimeFormatString)));
			}
			else {
				path.Add(parameter == null ? "null" : WebUtility.UrlEncode(Convert.ToString(parameter)));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Query(string key, IEnumerable<string> values) {
			query.Add(key, values.Select(a => WebUtility.UrlEncode(a.Trim())));
			return this;
		}

		public ICoalescenceRequestBuilder Query(string key, IEnumerable<byte[]> values) {
			query.Add(key, values.Select(WebEncoders.Base64UrlEncode));
			return this;
		}

		public ICoalescenceRequestBuilder Query<T>(string key, IEnumerable<T> values) where T : unmanaged, IComparable, IFormattable
		{
			if (typeof(T) == typeof(DateTime)) {
				query.Add(key, values.Select(a => WebUtility.UrlEncode(((DateTime)(object)a).ToString(options.DateTimeFormatString))));
			}
			else if (typeof(T) == typeof(DateTimeOffset)) {
				query.Add(key, values.Select(a => WebUtility.UrlEncode(((DateTimeOffset)(object)a).ToString(options.DateTimeFormatString))));
			}
			else {
				query.Add(key, values.Select(a => WebUtility.UrlEncode(Convert.ToString(a))));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Query<T>(string key, IEnumerable<T?> values) where T : unmanaged, IComparable, IFormattable
		{
			if (typeof(T) == typeof(DateTime)) {
				query.Add(key, values.Select(a => a == null ? "null" : WebUtility.UrlEncode(((DateTime?)(object)a)?.ToString(options.DateTimeFormatString))));
			}
			else if (typeof(T) == typeof(DateTimeOffset)) {
				query.Add(key, values.Select(a => a == null ? "null" : WebUtility.UrlEncode(((DateTimeOffset?)(object)a)?.ToString(options.DateTimeFormatString))));
			}
			else {
				query.Add(key, values.Select(a => a == null ? "null" : WebUtility.UrlEncode(Convert.ToString(a))));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Query<T>(T parameters) where T : class, ICoalescenceParameters {
			var pl = parameters.GetParameters();
			foreach (var p in pl) {
				query.Add(p.Key, p.Value.Select(WebUtility.UrlEncode));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Header(string key, IEnumerable<string> values) {
			headers.Add(key, values.Select(a => a.Trim()));
			return this;
		}

		public ICoalescenceRequestBuilder Header(string key, IEnumerable<byte[]> values) {
			headers.Add(key, values.Select(Convert.ToBase64String));
			return this;
		}

		public ICoalescenceRequestBuilder Header<T>(string key, IEnumerable<T> values) where T : unmanaged, IComparable, IFormattable
		{
			if (typeof(T) == typeof(DateTime)) {
				headers.Add(key, values.Select(a => ((DateTime)(object)a).ToString(options.DateTimeFormatString)));
			}
			else if (typeof(T) == typeof(DateTimeOffset)) {
				headers.Add(key, values.Select(a => ((DateTimeOffset)(object)a).ToString(options.DateTimeFormatString)));
			}
			else {
				headers.Add(key, values.Select(a => Convert.ToString(a)));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Header<T>(string key, IEnumerable<T?> values) where T : unmanaged, IComparable, IFormattable
		{
			if (typeof(T) == typeof(DateTime)) {
				headers.Add(key, values.Select(a => a == null ? "null" : ((DateTime?)(object)a)?.ToString(options.DateTimeFormatString)));
			}
			else if (typeof(T) == typeof(DateTimeOffset)) {
				headers.Add(key, values.Select(a => a == null ? "null" : ((DateTimeOffset?)(object)a)?.ToString(options.DateTimeFormatString)));
			}
			else {
				headers.Add(key, values.Select(a => a == null ? "null" : Convert.ToString(a)));
			}

			return this;
		}

		public ICoalescenceRequestBuilder Header<T>(T parameters) where T : class, ICoalescenceParameters {
			var pl = parameters.GetParameters();
			foreach (var p in pl)
			{
				headers.Add(p.Key, p.Value);
			}

			return this;
		}

		public ICoalescenceRequestBuilder Serialized<T>(T content, string contentType = null)
		{
			this.content = new CoalescenceContentItem(HttpContentScheme.Serialized, content, contentType);
			return this;
		}

		public ICoalescenceRequestBuilder ByteArray(byte[] content, string contentType = null)
		{
			this.content = new CoalescenceContentItem(HttpContentScheme.Binary, content, contentType);
			return this;
		}

		public ICoalescenceRequestBuilder Stream(Stream content, string contentType = null)
		{
			this.content = new CoalescenceContentItem(HttpContentScheme.Stream, content, contentType);
			return this;
		}

		public ICoalescenceRequestBuilder Text(string content, string contentType = null)
		{
			this.content = new CoalescenceContentItem(HttpContentScheme.Text, content, contentType);
			return this;
		}

		public ICoalescenceRequestBuilder FormUrlEncoded(Dictionary<string, string> content)
		{
			this.content = new CoalescenceContentItem(HttpContentScheme.FormUrlEncoded, content, "application/x-www-form-urlencoded");
			return this;
		}

		public ICoalescenceRequestBuilder Content(HttpContent content) {
			this.content = new CoalescenceContentItem(content);
			return this;
		}

		public ICoalescenceMultipartContentBuilder Multipart()
		{
			this.multipartContentBuilder = new CoalescenceMultipartContentBuilder(false, this, options);
			return this.multipartContentBuilder;
		}

		public ICoalescenceMultipartContentBuilder MultipartForm()
		{
			this.multipartContentBuilder = new CoalescenceMultipartContentBuilder(true, this, options);
			return this.multipartContentBuilder;
		}

		public ICoalescenceRequestBuilder Authentication(string scheme = null) {
			this.authentication = authenticators.GetCoalescenceAuthentication(scheme ?? options.DefaultAuthencationScheme);
			return this;
		}

		public ICoalescenceRequestBuilder NoRetry() {
			this.noRetry = true;
			return this;
		}

		public ICoalescenceRequestBuilder Timeout(TimeSpan timeout) {
			this.timeout = timeout;
			return this;
		}

		public async Task<ICoalescenceResponse> Send() {
			int retries = 0;
			HttpResponseMessage response = null;
			using var http = string.IsNullOrWhiteSpace(options.HttpClientId) ? httpClientFactory.CreateClient() : httpClientFactory.CreateClient(options.HttpClientId);
			http.Timeout = timeout;

			while ((noRetry && retries < 1) || retries < options.MaxRetryCount) {
				using var rm = await BuildRequest();
				response = await http.SendAsync(rm, HttpCompletionOption.ResponseHeadersRead);
				if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 400) break;
				if ((int)response.StatusCode == 429) break; // Add 429 handling here
				if ((int)response.StatusCode >= 500) break; // 5xx Errors on not recoverable on the client, so exit early.
				if (response.StatusCode == HttpStatusCode.Unauthorized && await authentication.ContinueOnFailure() == false) break; // Cancel or continue the request as indicated by the failure handler.
				retries++;
			}

			return new CoalescenceResponse(response, options);
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

			var rm = new HttpRequestMessage(method, uri.ToString());

			//Add any additional headers
			if (headers.Any(a => a.Value != null && a.Value.Any(b => !string.IsNullOrWhiteSpace(b))))
			{
				foreach (var h in headers)
				{
					rm.Headers.Add(h.Key, h.Value);
				}
			}

			if (authentication != null && !string.IsNullOrEmpty(authentication?.Scheme)) rm.Headers.Authorization = new AuthenticationHeaderValue(authentication.Scheme, await authentication.Get());

			//Get multipart content from builder if any.
			if (this.cachedContent == null) {
				if (multipartContentBuilder != null) {
					this.cachedContent = rm.Content = await multipartContentBuilder.Build();
				}
				else if (content != null) {
					this.cachedContent = rm.Content = await content.Build(options.Serializers);
				}
			}
			else {
				rm.Content = this.cachedContent;
			}

			return rm;
		}
	}
}
