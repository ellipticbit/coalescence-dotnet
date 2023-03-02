using System;
using System.Net.Http;
using EllipticBit.Hotwire.Shared;

namespace EllipticBit.Hotwire.Request
{
	internal class HotwireRequestFactory : IHotwireRequestFactory
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IHotwireOptionsRepository _repository;

		public HotwireRequestFactory(IHttpClientFactory httpClientFactory, IHotwireOptionsRepository repository) {
			this._httpClientFactory = httpClientFactory;
			this._repository = repository;
		}

		public IHotwireRequest CreateRequest() {
			return new HotwireRequest(_httpClientFactory, _repository.DefaultRequestOptions as HotwireRequestOptions);
		}

		public IHotwireRequest CreateRequest(string name) {
			if (_repository.RequestOptions.TryGetValue(name, out HotwireOptionsBase result)) {
				return new HotwireRequest(_httpClientFactory, result as HotwireRequestOptions);
			}

			throw new IndexOutOfRangeException($"No HotWire Factory available for name: {name}");
		}
	}
}
