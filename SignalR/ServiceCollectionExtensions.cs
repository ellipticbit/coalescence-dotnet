using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Coalescence.SignalR
{
	public static class ServiceCollectionExtensions
	{
		public static ICoalescenceSignalRServiceBuilder AddCoalescenceSignalRServices(this IServiceCollection builder, HubConnection defaultConnection) {
			builder.TryAddTransient<ICoalescenceSignalRRepository, CoalescenceSignalRRepository>();

			return new CoalescenceSignalRRepository(defaultConnection);
		}
	}
}
