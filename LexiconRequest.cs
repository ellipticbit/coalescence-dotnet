using System.Net.Http;

namespace EllipticBit.Lexicon.Client
{
	internal sealed class LexiconRequest :ILexiconRequest
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly LexiconRequestOptions options;

		public LexiconRequest(IHttpClientFactory httpClientFactory, LexiconRequestOptions options) {
			this.httpClientFactory = httpClientFactory;
			this.options = options;
		}

		public ILexiconRequestBuilder Get() {
			return new LexiconRequestBuilder(HttpMethod.Get, httpClientFactory, options);
		}

		public ILexiconRequestBuilder Put() {
			return new LexiconRequestBuilder(HttpMethod.Put, httpClientFactory, options);
		}

		public ILexiconRequestBuilder Post() {
			return new LexiconRequestBuilder(HttpMethod.Post, httpClientFactory, options);
		}

		public ILexiconRequestBuilder Patch() {
			return new LexiconRequestBuilder(new HttpMethod("PATCH"), httpClientFactory, options);
		}

		public ILexiconRequestBuilder Delete() {
			return new LexiconRequestBuilder(HttpMethod.Delete, httpClientFactory, options);
		}

		public ILexiconRequestBuilder Head() {
			return new LexiconRequestBuilder(HttpMethod.Head, httpClientFactory, options);
		}
	}
}
