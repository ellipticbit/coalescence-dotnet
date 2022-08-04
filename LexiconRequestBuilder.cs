using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace EllipticBit.Lexicon.Client
{
	internal sealed class LexiconRequestBuilder : ILexiconRequestBuilder
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly LexiconRequestOptions options;
		private readonly HttpMethod method;
		private readonly List<string> path = new();
		private readonly Dictionary<string, IEnumerable<string>> query = new();
		private readonly Dictionary<string, IEnumerable<string>> headers = new();
		private string authenticationScheme = null;
		private string authenticationTenant = null;
		private TimeSpan? timeout = null;
		private bool suppressHttpErrorExceptions = false;

		private LexiconContentItem content;
		private LexiconMultipartContentBuilder multipartContentBuilder = null;

		public LexiconRequestBuilder(HttpMethod method, IHttpClientFactory httpClientFactory, LexiconRequestOptions options) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
			this.method = method;
		}

		public ILexiconRequestBuilder Path(params string[] parameter) {
			path.AddRange(parameter.Select(a => a.Trim().Trim('/', '&')));
			return this;
		}

		public ILexiconRequestBuilder Path(string parameter) {
			path.Add(parameter.Trim().Trim('/', '&'));
			return this;
		}

		public ILexiconRequestBuilder Path(byte[] parameter) {
			path.Add(WebEncoders.Base64UrlEncode(parameter));
			return this;
		}

		public ILexiconRequestBuilder Path<T>(T parameter) where T : struct, IConvertible {
			path.Add(Convert.ToString(parameter));
			return this;
		}

		public ILexiconRequestBuilder Path<T>(T? parameter) where T : struct, IConvertible {
			path.Add(parameter == null ? "null" : Convert.ToString(parameter));
			return this;
		}

		public ILexiconRequestBuilder Query(string key, IEnumerable<string> values) {
			query.Add(key, values.Select(a => a.Trim()));
			return this;
		}

		public ILexiconRequestBuilder Query(string key, IEnumerable<byte[]> values) {
			query.Add(key, values.Select(WebEncoders.Base64UrlEncode));
			return this;
		}

		public ILexiconRequestBuilder Query<T>(string key, IEnumerable<T> values) where T : struct, IConvertible {
			query.Add(key, values.Select(a => Convert.ToString(a)));
			return this;
		}

		public ILexiconRequestBuilder Query<T>(string key, IEnumerable<T?> values) where T : struct, IConvertible {
			query.Add(key, values.Select(a => a == null ? "null" : Convert.ToString(a)));
			return this;
		}

		public ILexiconRequestBuilder Header(string key, IEnumerable<string> values) {
			headers.Add(key, values.Select(a => a.Trim()));
			return this;
		}

		public ILexiconRequestBuilder Header(string key, IEnumerable<byte[]> values) {
			headers.Add(key, values.Select(Convert.ToBase64String));
			return this;
		}

		public ILexiconRequestBuilder Header<T>(string key, IEnumerable<T> values) where T : struct, IConvertible {
			headers.Add(key, values.Select(a => Convert.ToString(a)));
			return this;
		}

		public ILexiconRequestBuilder Header<T>(string key, IEnumerable<T?> values) where T : struct, IConvertible {
			headers.Add(key, values.Select(a => a == null ? "null" : Convert.ToString(a)));
			return this;
		}

		public ILexiconRequestBuilder Serialized<T>(T content, HttpContentScheme scheme)
		{
			this.content = new LexiconContentItem(scheme ?? HttpContentScheme.Json, content, (scheme ?? HttpContentScheme.Json).ToString());
			return this;
		}

		public ILexiconRequestBuilder ByteArray(byte[] content, string contentType = null)
		{
			this.content = new LexiconContentItem(HttpContentScheme.Binary, content, contentType ?? HttpContentScheme.Binary.ToString());
			return this;
		}

		public ILexiconRequestBuilder Stream(Stream content, string contentType = null)
		{
			this.content = new LexiconContentItem(HttpContentScheme.Stream, content, contentType ?? HttpContentScheme.Stream.ToString());
			return this;
		}

		public ILexiconRequestBuilder Text(string content, string contentType = null)
		{
			this.content = new LexiconContentItem(HttpContentScheme.Text, content, contentType ?? HttpContentScheme.Text.ToString());
			return this;
		}

		public ILexiconRequestBuilder FormUrlEncoded(Dictionary<string, string> content)
		{
			this.content = new LexiconContentItem(HttpContentScheme.FormUrlEncoded, content, HttpContentScheme.FormUrlEncoded.ToString());
			return this;
		}

		public ILexiconRequestBuilder Content(HttpContent content) {
			this.content = new LexiconContentItem(content);
			return this;
		}

		public ILexiconMultipartContentBuilder Multipart()
		{
			this.multipartContentBuilder = new LexiconMultipartContentBuilder(false, this, options);
			return this.multipartContentBuilder;
		}

		public ILexiconMultipartContentBuilder MultipartForm()
		{
			this.multipartContentBuilder = new LexiconMultipartContentBuilder(true, this, options);
			return this.multipartContentBuilder;
		}

		public ILexiconRequestBuilder BasicAuthentication(string tenantId = null) {
			this.authenticationScheme = "Basic".ToLowerInvariant();
			this.authenticationTenant = tenantId;
			return this;
		}

		public ILexiconRequestBuilder BearerAuthentication(string tenantId = null) {
			this.authenticationScheme = "Bearer".ToLowerInvariant();
			this.authenticationTenant = tenantId;
			return this;
		}

		public ILexiconRequestBuilder CustomAuthentication(string scheme, string tenantId = null) {
			if (scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase) ||
				scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException($"Authentication scheme '{scheme}' is not a valid custom scheme.");

			this.authenticationScheme = scheme.ToLowerInvariant();
			this.authenticationTenant = tenantId;
			return this;
		}

		public ILexiconRequestBuilder Timeout(TimeSpan timeout) {
			this.timeout = timeout;
			return this;
		}

		public ILexiconRequestBuilder SuppressHttpResultExceptions() {
			suppressHttpErrorExceptions = true;
			return this;
		}

		public async Task<HttpResponseMessage> Send() {
			using var rm = await BuildRequest();
			using var http = string.IsNullOrWhiteSpace(options.HttpClientId) ? this.httpClientFactory.CreateClient() : this.httpClientFactory.CreateClient(options.HttpClientId);
			http.Timeout = timeout ?? TimeSpan.FromSeconds(100);

			var response = await http.SendAsync(rm, HttpCompletionOption.ResponseContentRead);

			if (suppressHttpErrorExceptions || response.IsSuccessStatusCode) return response;

			var nex = new HttpRequestException();
			nex.Data.Add("StatusCode", response.StatusCode);
			nex.Data.Add("ReasonPhrase", response.ReasonPhrase);
			nex.Data.Add("Content", await response.Content.ReadAsStringAsync());
			throw nex;
		}

		public async Task<T> Send<T>() {
			using var rm = await BuildRequest();
			using var http = string.IsNullOrWhiteSpace(options.HttpClientId) ? this.httpClientFactory.CreateClient() : this.httpClientFactory.CreateClient(options.HttpClientId);
			http.Timeout = timeout ?? TimeSpan.FromSeconds(100);

			var response = await http.SendAsync(rm, HttpCompletionOption.ResponseContentRead);

			if (suppressHttpErrorExceptions || response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<T>();

			var nex = new HttpRequestException();
			nex.Data.Add("StatusCode", response.StatusCode);
			nex.Data.Add("ReasonPhrase", response.ReasonPhrase);
			nex.Data.Add("Content", await response.Content.ReadAsStringAsync());
			throw nex;
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

			rm.Headers.Authorization = await GetAuthenticationHeader();

			//Get multipart content from builder if any.
			if (multipartContentBuilder != null)
			{
				rm.Content = await multipartContentBuilder.Build();
			}
			else if (content != null)
			{
				rm.Content = await content.Build(options);
			}

			return rm;
		}

		private async Task<AuthenticationHeaderValue> GetAuthenticationHeader() {
			if (authenticationScheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
			{
				var basic = await options.AuthenticationHandler.Basic(authenticationTenant);
				return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{basic.username}:{basic.password}")));
			}
			else if (authenticationScheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase)) {
				return new AuthenticationHeaderValue(authenticationScheme, await options.AuthenticationHandler.Bearer(authenticationTenant));
			}
			else {
				return new AuthenticationHeaderValue(authenticationScheme, await options.AuthenticationHandler.Custom(authenticationScheme, authenticationTenant));
			}
		}
	}
}
