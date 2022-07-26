using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Lexicon.Client
{
	public static class LexiconRequestServiceCollectionExtensions
	{
		public static ILexiconRequestFactoryBuilder AddLexiconServices(this IServiceCollection service, LexiconRequestOptions defaultOptions) {
			service.TryAddTransient<ILexiconRequestFactory, LexiconRequestFactory>();
			LexiconRequestFactory.SetDefaultOptions(defaultOptions);
			return new LexiconRequestFactory(null);
		}
	}
}
