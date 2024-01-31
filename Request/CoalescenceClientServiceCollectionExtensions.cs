using EllipticBit.Coalescence.Shared;
using EllipticBit.Coalescence.Shared.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Coalescence.Request
{
	public static class CoalescenceClientServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the necessary services to use the Coalescence Request client library services.
		/// </summary>
		/// <param name="service">The IServiceCollection to add the services.</param>
		/// <returns>Returns the reference IServiceCollection that this method is called from.</returns>
		public static IServiceCollection AddCoalescenceRequestServices(this IServiceCollection service) {
			service.TryAddTransient<ICoalescenceRequestFactory, CoalescenceRequestFactory>();
			return service;
		}

		/// <summary>
		/// Adds the specified instance of <see cref="CoalescenceRequestOptions">CoalescenceRequestOptions</see> to the options repository.
		/// </summary>
		/// <param name="name">The name of options used by Coalescence to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		public static ICoalescenceServiceBuilder AddCoalescenceRequestOptions(this ICoalescenceServiceBuilder builder, string name, CoalescenceRequestOptions options, bool isDefault = false) {
			return builder.AddCoalescenceRequestOptions(name, (CoalescenceOptionsBase)options, isDefault);
		}
	}
}
