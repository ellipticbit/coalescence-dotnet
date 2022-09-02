using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	internal sealed class LexiconResponse : ILexiconResponse
	{
		private readonly HttpResponseMessage response;
		private readonly LexiconRequestOptions options;

		public LexiconResponse(HttpResponseMessage response, LexiconRequestOptions options) {
			this.response = response;
			this.options = options;
		}

		public ILexiconResponse ThrowOnFailureResponse() {
			if (response.IsSuccessStatusCode) return this;

			throw new LexiconResponseError(response.StatusCode, response.ReasonPhrase, response.Content.ReadAsStringAsync().Result);
		}

		public ILexiconResponse GetResponseError(out LexiconResponseError error) {
			if (response.IsSuccessStatusCode) error = null;

			error = new LexiconResponseError(response.StatusCode, response.ReasonPhrase, response.Content.ReadAsStringAsync().Result);

			return this;
		}

		public Dictionary<string, string[]> AsHeaders() {
			return response.Headers.ToDictionary(k => k.Key, v => v.Value.ToArray());
		}

		public Task<HttpContent> AsContent() {
			return Task.FromResult(response.Content);
		}

		public async Task<T> AsObject<T>() {
			if (response.Content.Headers.ContentType?.MediaType == "application/json") {
				using var stream = await response.Content.ReadAsStreamAsync();
				return await JsonSerializer.DeserializeAsync<T>(stream, options.JsonSerializerOptions);
			}
			else if (response.Content.Headers.ContentType?.MediaType is "application/xml" or "text/xml") {
				using var stream = await response.Content.ReadAsStreamAsync();
				var xml = new XmlMediaTypeFormatter();
				options.XmlSerializerOptions.ApplyOptions(xml);
				return (T)await xml.ReadFromStreamAsync(typeof(T), stream, response.Content, null);
			}
			throw new NotImplementedException($"No deserializer has been implemented for Content-Type: {response.Content.Headers.ContentType?.MediaType}");
		}

		public Task<byte[]> AsByteArray() {
			return response.Content.ReadAsByteArrayAsync();
		}

		public Task<Stream> AsStream() {
			return response.Content.ReadAsStreamAsync();
		}

		public Task<string> AsText() {
			return response.Content.ReadAsStringAsync();
		}

		public async Task<Dictionary<string, string>> AsFormUrlEncoded() {
			if (response.Content is not FormUrlEncodedContent fuec) {
				throw new InvalidOperationException("Content type is not FormUrlEncoded.");
			}

			var results = await fuec.ReadAsFormDataAsync();
			return results.AllKeys.ToDictionary(k => k, k => results[k]);
		}

		public async Task<string> AsMultipartString(string name) {
			if (response.Content.IsMimeMultipartContent()) throw new InvalidOperationException("Response content is not valid multi-part content");
			if (response.Content is MultipartFormDataContent formContent)
			{
				formContent.
			} else if (response.Content is MultipartContent content) {
				var cl = content.ToList();
			}
			else {
				throw new InvalidOperationException("Response content is not valid multi-part content");
			}
		}
	}
}
