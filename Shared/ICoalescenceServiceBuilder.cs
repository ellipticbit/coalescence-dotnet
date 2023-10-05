namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Registers the base Coalescence services.
	/// </summary>
	public interface ICoalescenceServiceBuilder
	{
		/// <summary>
		/// Adds the specified Request implementation of <see cref="CoalescenceOptionsBase">CoalescenceOptionsBase</see>.
		/// </summary>
		/// <param name="name">The name of options used by Coalescence to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		ICoalescenceServiceBuilder AddCoalescenceRequestOptions(string name, CoalescenceOptionsBase options, bool isDefault = false);

		/// <summary>
		/// Adds the specified Controller implementation of <see cref="CoalescenceOptionsBase">CoalescenceOptionsBase</see>.
		/// </summary>
		/// <param name="name">The name of options used by Coalescence to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		ICoalescenceServiceBuilder AddCoalescenceControllerOptions(string name, CoalescenceOptionsBase options, bool isDefault = false);

		/// <summary>
		/// Adds the specified WebSocket implementation of <see cref="CoalescenceOptionsBase">CoalescenceOptionsBase</see>.
		/// </summary>
		/// <param name="name">The name of options used by Coalescence to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		ICoalescenceServiceBuilder AddCoalescenceWebSocketOptions(string name, CoalescenceOptionsBase options, bool isDefault = false);
	}
}
