using EllipticBit.Hotwire.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Hotwire.Request
{
	public static class HotwireClientServiceCollectionExtensions
	{
		public static IServiceCollection AddHotwireClientServices(this IServiceCollection service)
		{
			service.TryAddTransient<IHotwireRequestFactory, HotwireRequestFactory>();
			return service;
		}

		/// <summary>
		/// Adds the specified instance of <see cref="HotwireRequestOptions">HotwireRequestOptions</see> to the options repository.
		/// </summary>
		/// <param name="name">The name of options used by Hotwire to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		public static IHotwireServiceBuilder AddHotwireRequestOptions(this IHotwireServiceBuilder builder, string name, HotwireRequestOptions options, bool isDefault = false) {
			return builder.AddHotwireRequestOptions(name, (HotwireOptionsBase)options, isDefault);
		}
	}
}
