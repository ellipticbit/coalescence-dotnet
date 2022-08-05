using System;
using System.Collections.Immutable;
using System.Net.Http;

namespace EllipticBit.Lexicon.Client
{
	internal class LexiconRequestFactory : ILexiconRequestFactory, ILexiconRequestFactoryBuilder
	{
		private static LexiconRequestOptions _defaultOptions;
		private static ImmutableDictionary<string, LexiconRequestOptions> _options = ImmutableDictionary<string, LexiconRequestOptions>.Empty;

		private readonly IHttpClientFactory _httpClientFactory;

		public LexiconRequestFactory(IHttpClientFactory httpClientFactory) {
			this._httpClientFactory = httpClientFactory;
		}

		public ILexiconRequest CreateRequest() {
			return new LexiconRequest(_httpClientFactory, _defaultOptions);
		}

		public ILexiconRequest CreateRequest(string name) {
			if (_options.TryGetValue(name, out LexiconRequestOptions result)) {
				return new LexiconRequest(_httpClientFactory, result);
			}

			throw new IndexOutOfRangeException($"No request available for name: {name}");
		}

		public ILexiconRequestFactoryBuilder AddLexiconRequestFactory(string name, LexiconRequestOptions options) {
			LexiconRequestFactory._options = LexiconRequestFactory._options.Add(name, options);
			return this;
		}

		internal static void SetDefaultOptions(LexiconRequestOptions defaultOptions) {
			if (LexiconRequestFactory._defaultOptions != null) throw new IndexOutOfRangeException("Default options have already been set.");
			LexiconRequestFactory._defaultOptions = defaultOptions;
		}
	}
}
