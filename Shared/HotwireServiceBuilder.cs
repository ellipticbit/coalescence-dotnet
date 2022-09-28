using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Hotwire.Shared
{
	internal class HotwireServiceBuilder : IHotwireServiceBuilder
	{
		private readonly IServiceCollection services;

		public HotwireServiceBuilder(IServiceCollection services) {
			this.services = services;
		}

		public IHotwireServiceBuilder AddSerializer<T>(bool defaultSerializer = false) where T : class, IHotwireSerializer
		{
			this.services.AddTransient<IHotwireSerializer, T>();
			return this;
		}

		public IHotwireServiceBuilder AddAuthentication<T>(bool defaultAuthentication = false) where T : class, IHotwireAuthentication
		{
			this.services.AddTransient<IHotwireAuthentication, T>();
			return this;
		}
	}
}
