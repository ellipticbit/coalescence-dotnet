using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Hotwire.Client
{
	public static class HotwireServiceCollectionExtensions
	{
		public static IHotwireRequestFactoryBuilder AddHotwireClientServices(this IServiceCollection service, HotwireRequestOptions defaultOptions) {
			service.TryAddTransient<IHotwireRequestFactory, HotwireRequestFactory>();
			HotwireRequestFactory.SetDefaultOptions(defaultOptions);
			return new HotwireRequestFactory(null);
		}
	}
}
