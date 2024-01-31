using System;
using System.Collections.Generic;
using System.Net.Http;
using EllipticBit.Coalescence.Shared;
using EllipticBit.Coalescence.Shared.Request;

namespace EllipticBit.Coalescence.Request
{
	internal class CoalescenceRequestFactory : ICoalescenceRequestFactory
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IEnumerable<ICoalescenceAuthentication> authenticators;
		private readonly ICoalescenceOptionsRepository _repository;

		public CoalescenceRequestFactory(IHttpClientFactory httpClientFactory, IEnumerable<ICoalescenceAuthentication> authenticators, ICoalescenceOptionsRepository repository) {
			this._httpClientFactory = httpClientFactory;
			this.authenticators = authenticators;
			this._repository = repository;
		}

		public ICoalescenceRequest CreateRequest() {
			return new CoalescenceRequest(_httpClientFactory, authenticators, _repository.DefaultRequestOptions as CoalescenceRequestOptions);
		}

		public ICoalescenceRequest CreateRequest(string name) {
			if (_repository.RequestOptions.TryGetValue(name, out CoalescenceOptionsBase result)) {
				return new CoalescenceRequest(_httpClientFactory, authenticators, result as CoalescenceRequestOptions);
			}

			throw new IndexOutOfRangeException($"No Coalescence Factory available for name: {name}");
		}
	}
}
