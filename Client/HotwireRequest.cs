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
		private readonly IEnumerable<IHotwireAuthentication> authenticators;

		public HotwireRequest(IHttpClientFactory httpClientFactory, HotwireRequestOptions options, IEnumerable<IHotwireSerializer> serializers, IEnumerable<IHotwireAuthentication> authenticators) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
			this.serializers = serializers;
			this.authenticators = authenticators;
		}

		public IHotwireRequestBuilder Get() {
			return new HotwireRequestBuilder(HttpMethod.Get, httpClientFactory, options, serializers, authenticators);
		}

		public IHotwireRequestBuilder Put() {
			return new HotwireRequestBuilder(HttpMethod.Put, httpClientFactory, options, serializers, authenticators);
		}

		public IHotwireRequestBuilder Post() {
			return new HotwireRequestBuilder(HttpMethod.Post, httpClientFactory, options, serializers, authenticators);
		}

		public IHotwireRequestBuilder Patch() {
			return new HotwireRequestBuilder(new HttpMethod("PATCH"), httpClientFactory, options, serializers, authenticators);
		}

		public IHotwireRequestBuilder Delete() {
			return new HotwireRequestBuilder(HttpMethod.Delete, httpClientFactory, options, serializers, authenticators);
		}

		public IHotwireRequestBuilder Head() {
			return new HotwireRequestBuilder(HttpMethod.Head, httpClientFactory, options, serializers, authenticators);
		}
	}
}
