using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Extension methods for registering Coalescence services.
	/// </summary>
	public static class CoalescenceServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the basic default Coalescence services to the service collection.
		/// </summary>
		/// <param name="service">The ServiceCollection that the services will be registered into.</param>
		/// <returns>Returns an <see cref="ICoalescenceServiceBuilder">ICoalescenceServiceBuilder</see> object.</returns>
		public static ICoalescenceServiceBuilder AddCoalescenceServices(this IServiceCollection service) {
			service.AddTransient<ICoalescenceOptionsRepository, CoalescenceServiceBuilder>();
			return new CoalescenceServiceBuilder();
		}
	}
}
