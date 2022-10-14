using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Hotwire.Shared
{
	internal class HotwireServiceBuilder : IHotwireServiceBuilder
	{
		private readonly IServiceCollection services;

		public HotwireServiceBuilder(IServiceCollection services) {
			this.services = services;
		}

		public IHotwireServiceBuilder AddHotwireOptions(HotwireOptionsBase options) {
			this.services.AddSingleton(options);
			return this;
		}
	}
}
