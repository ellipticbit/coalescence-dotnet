using EllipticBit.Hotwire.Shared;

using System.Collections.Generic;
using System.Net.Http;

namespace EllipticBit.Hotwire.Client
{
	internal sealed class HotwireRequest : IHotwireRequest
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly HotwireRequestOptions options;
		private readonly IEnumerable<IHotwireSerializer> serializers;

		public HotwireRequest(IHttpClientFactory httpClientFactory, HotwireRequestOptions options, IEnumerable<IHotwireSerializer> serializers) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
			this.serializers = serializers;
		}

		public IHotwireRequestBuilder Get() {
			return new HotwireRequestBuilder(HttpMethod.Get, httpClientFactory, options, serializers);
		}

		public IHotwireRequestBuilder Put() {
			return new HotwireRequestBuilder(HttpMethod.Put, httpClientFactory, options, serializers);
		}

		public IHotwireRequestBuilder Post() {
			return new HotwireRequestBuilder(HttpMethod.Post, httpClientFactory, options, serializers);
		}

		public IHotwireRequestBuilder Patch() {
			return new HotwireRequestBuilder(new HttpMethod("PATCH"), httpClientFactory, options, serializers);
		}

		public IHotwireRequestBuilder Delete() {
			return new HotwireRequestBuilder(HttpMethod.Delete, httpClientFactory, options, serializers);
		}

		public IHotwireRequestBuilder Head() {
			return new HotwireRequestBuilder(HttpMethod.Head, httpClientFactory, options, serializers);
		}
	}
}
