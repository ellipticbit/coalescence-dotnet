using System.Net.Http;

namespace EllipticBit.Hotwire.Client
{
	internal sealed class HotwireRequest : IHotwireRequest
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly HotwireRequestOptions options;

		public HotwireRequest(IHttpClientFactory httpClientFactory, HotwireRequestOptions options) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
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
