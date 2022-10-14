namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Registers the base Hotwire services.
	/// </summary>
	public interface IHotwireServiceBuilder
	{
		/// <summary>
		/// Adds the specified implementation of <see cref="HotwireOptionsBase">HotwireOptionsBase</see>.
		/// </summary>
		/// <param name="options">The Options class to register</param>
		/// <returns>A reference to this builder.</returns>
		IHotwireServiceBuilder AddHotwireOptions(HotwireOptionsBase options);
	}
}
