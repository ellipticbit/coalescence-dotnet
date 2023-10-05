using System;
using System.Net.Http;
using EllipticBit.Coalescence.Shared;

namespace EllipticBit.Coalescence.Request
{
	internal class CoalescenceRequestFactory : ICoalescenceRequestFactory
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICoalescenceOptionsRepository _repository;

		public CoalescenceRequestFactory(IHttpClientFactory httpClientFactory, ICoalescenceOptionsRepository repository) {
			this._httpClientFactory = httpClientFactory;
			this._repository = repository;
		}

		public ICoalescenceRequest CreateRequest() {
			return new CoalescenceRequest(_httpClientFactory, _repository.DefaultRequestOptions as CoalescenceRequestOptions);
		}

		public ICoalescenceRequest CreateRequest(string name) {
			if (_repository.RequestOptions.TryGetValue(name, out CoalescenceOptionsBase result)) {
				return new CoalescenceRequest(_httpClientFactory, result as CoalescenceRequestOptions);
			}

			throw new IndexOutOfRangeException($"No Coalescence Factory available for name: {name}");
		}
	}
}
