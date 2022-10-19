namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Registers the base Hotwire services.
	/// </summary>
	public interface IHotwireServiceBuilder
	{
		/// <summary>
		/// Adds the specified Request implementation of <see cref="HotwireOptionsBase">HotwireOptionsBase</see>.
		/// </summary>
		/// <param name="name">The name of options used by Hotwire to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		IHotwireServiceBuilder AddHotwireRequestOptions(string name, HotwireOptionsBase options, bool isDefault = false);

		/// <summary>
		/// Adds the specified Controller implementation of <see cref="HotwireOptionsBase">HotwireOptionsBase</see>.
		/// </summary>
		/// <param name="name">The name of options used by Hotwire to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		IHotwireServiceBuilder AddHotwireControllerOptions(string name, HotwireOptionsBase options, bool isDefault = false);

		/// <summary>
		/// Adds the specified WebSocket implementation of <see cref="HotwireOptionsBase">HotwireOptionsBase</see>.
		/// </summary>
		/// <param name="name">The name of options used by Hotwire to access this options instance.</param>
		/// <param name="options">The Options class to register</param>
		/// <param name="isDefault">Specifies that this Options instance is the default Options instance. If no default is specified, the first registered Options instance will be used.</param>
		/// <returns>A reference to this builder.</returns>
		IHotwireServiceBuilder AddHotwireWebSocketOptions(string name, HotwireOptionsBase options, bool isDefault = false);
	}
}
