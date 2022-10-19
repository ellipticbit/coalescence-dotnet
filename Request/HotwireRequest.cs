using System;
using System.Net.Http;

namespace EllipticBit.Hotwire.Request
{
	internal sealed class HotwireRequest : IHotwireRequest
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly HotwireRequestOptions options;

		public HotwireRequest(IHttpClientFactory httpClientFactory, HotwireRequestOptions options) {
			this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory), "Must specify an IHttpClientFactory instance to use for this request.");
			this.options = options ?? throw new ArgumentNullException(nameof(options), "Must specify an options instance to use for this request.");
		}

		public IHotwireRequestBuilder Get() {
			return new HotwireRequestBuilder(HttpMethod.Get, httpClientFactory, options);
		}

		public IHotwireRequestBuilder Put() {
			return new HotwireRequestBuilder(HttpMethod.Put, httpClientFactory, options);
		}

		public IHotwireRequestBuilder Post() {
			return new HotwireRequestBuilder(HttpMethod.Post, httpClientFactory, options);
		}

		public IHotwireRequestBuilder Patch() {
			return new HotwireRequestBuilder(new HttpMethod("PATCH"), httpClientFactory, options);
		}

		public IHotwireRequestBuilder Delete() {
			return new HotwireRequestBuilder(HttpMethod.Delete, httpClientFactory, options);
		}

		public IHotwireRequestBuilder Head() {
			return new HotwireRequestBuilder(HttpMethod.Head, httpClientFactory, options);
		}
	}
}
