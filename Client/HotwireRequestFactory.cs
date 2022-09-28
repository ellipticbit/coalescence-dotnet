using EllipticBit.Hotwire.Shared;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;

namespace EllipticBit.Hotwire.Client
{
	internal class HotwireRequestFactory : IHotwireRequestFactory, IHotwireRequestFactoryBuilder
	{
		private static HotwireRequestOptions _defaultOptions;
		private static ImmutableDictionary<string, HotwireRequestOptions> _options = ImmutableDictionary<string, HotwireRequestOptions>.Empty;

		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IEnumerable<IHotwireSerializer> _serializers;

		public HotwireRequestFactory(IHttpClientFactory httpClientFactory, IEnumerable<IHotwireSerializer> serializers) {
			this._httpClientFactory = httpClientFactory;
		}

		public IHotwireRequest CreateRequest() {
			return new HotwireRequest(_httpClientFactory, _defaultOptions, _serializers);
		}

		public IHotwireRequest CreateRequest(string name) {
			if (_options.TryGetValue(name, out HotwireRequestOptions result)) {
				return new HotwireRequest(_httpClientFactory, result, _serializers);
			}

			throw new IndexOutOfRangeException($"No request available for name: {name}");
		}

		public IHotwireRequestFactoryBuilder AddHotwireRequestFactory(string name, HotwireRequestOptions options) {
			_options = _options.Add(name, options);
			return this;
		}

		internal static void SetDefaultOptions(HotwireRequestOptions defaultOptions) {
			if (_defaultOptions != null) throw new IndexOutOfRangeException("Default options have already been set.");
			_defaultOptions = defaultOptions;
		}
	}
}
