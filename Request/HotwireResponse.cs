using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EllipticBit.Hotwire.Shared;

namespace EllipticBit.Hotwire.Request
{
	internal sealed class HotwireResponse : IHotwireResponse
	{
		private readonly HttpResponseMessage response;
		private readonly HotwireRequestOptions options;

		public HotwireResponse(HttpResponseMessage response, HotwireRequestOptions options) {
			this.response = response;
			this.options = options;
		}

		public IHotwireResponse ThrowOnFailureResponse() {
			if (response.IsSuccessStatusCode) return this;

			throw new HotwireResponseError(response.StatusCode, response.ReasonPhrase, response.Content.ReadAsStringAsync().Result);
		}

		public IHotwireResponse GetResponseError(out HotwireResponseError error) {
			if (response.IsSuccessStatusCode) error = null;

			error = new HotwireResponseError(response.StatusCode, response.ReasonPhrase, response.Content.ReadAsStringAsync().Result);

			return this;
		}

		public Dictionary<string, string[]> AsHeaders() {
			return response.Headers.ToDictionary(k => k.Key, v => v.Value.ToArray());
		}

		public Task<HttpContent> AsContent() {
			return Task.FromResult(response.Content);
		}

		public async Task<T> AsObject<T>() {
			if (!response.IsSuccessStatusCode) return default(T);
			var serializer = options.Serializers.GetHotwireSerializer(response.Content.Headers.ContentType?.MediaType);
			return await serializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
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
			if (!response.IsSuccessStatusCode) return null;
			if (response.Content is not FormUrlEncodedContent fuec) {
				throw new InvalidOperationException("Content type is not FormUrlEncoded.");
			}

			var results = await fuec.ReadAsFormDataAsync();
			return results.AllKeys.ToDictionary(k => k, k => results[k]);
		}

		//public async Task<string> AsMultipartString(string name) {
		//	if (response.Content.IsMimeMultipartContent()) throw new InvalidOperationException("Response content is not valid multi-part content");
		//	if (response.Content is MultipartFormDataContent formContent)
		//	{
		//	} else if (response.Content is MultipartContent content) {
		//		var cl = content.ToList();
		//	}
		//	else {
		//		throw new InvalidOperationException("Response content is not valid multi-part content");
		//	}
		//}

		public async ValueTask DisposeAsync() {
			if (response is IAsyncDisposable responseAsyncDisposable)
				await responseAsyncDisposable.DisposeAsync();
			else if (response != null)
				response.Dispose();
		}
	}
}
