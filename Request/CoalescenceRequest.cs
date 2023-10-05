using System;
using System.Net.Http;

namespace EllipticBit.Coalescence.Request
{
	internal sealed class CoalescenceRequest : ICoalescenceRequest
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly CoalescenceRequestOptions options;

		public CoalescenceRequest(IHttpClientFactory httpClientFactory, CoalescenceRequestOptions options) {
			this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory), "Must specify an IHttpClientFactory instance to use for this request.");
			this.options = options ?? throw new ArgumentNullException(nameof(options), "Must specify an options instance to use for this request.");
		}

		public ICoalescenceRequestBuilder Get() {
			return new CoalescenceRequestBuilder(HttpMethod.Get, httpClientFactory, options);
		}

		public ICoalescenceRequestBuilder Put() {
			return new CoalescenceRequestBuilder(HttpMethod.Put, httpClientFactory, options);
		}

		public ICoalescenceRequestBuilder Post() {
			return new CoalescenceRequestBuilder(HttpMethod.Post, httpClientFactory, options);
		}

		public ICoalescenceRequestBuilder Patch() {
			return new CoalescenceRequestBuilder(new HttpMethod("PATCH"), httpClientFactory, options);
		}

		public ICoalescenceRequestBuilder Delete() {
			return new CoalescenceRequestBuilder(HttpMethod.Delete, httpClientFactory, options);
		}

		public ICoalescenceRequestBuilder Head() {
			return new CoalescenceRequestBuilder(HttpMethod.Head, httpClientFactory, options);
		}
	}
}
