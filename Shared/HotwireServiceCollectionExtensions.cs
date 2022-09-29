using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Extension methods for registering Hotwire services.
	/// </summary>
	public static class HotwireServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the basic default Hotwire services to the service collection.
		/// </summary>
		/// <param name="service">The ServiceCollection that the services will be registered into.</param>
		/// <returns>Returns an <see cref="IHotwireServiceBuilder">IHotwireServiceBuilder</see> object.</returns>
		public static IHotwireServiceBuilder AddHotwireServices(this IServiceCollection service)
		{
			service.AddTransient<IHotwireSerializer, HotwireJsonSerializer>();
			service.AddTransient<IHotwireSerializer, HotwireXmlSerializer>();
			return new HotwireServiceBuilder(service);
		}
	}
}
