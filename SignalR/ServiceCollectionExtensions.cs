using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Hotwire.SignalR
{
	public static class ServiceCollectionExtensions
	{
		public static IHotwireSignalRServiceBuilder AddHotwireSignalRServices(this IServiceCollection builder, HubConnection defaultConnection) {
			builder.TryAddTransient<IHotwireSignalRRepository, HotwireSignalRRepository>();

			return new HotwireSignalRRepository(defaultConnection);
		}
	}
}
