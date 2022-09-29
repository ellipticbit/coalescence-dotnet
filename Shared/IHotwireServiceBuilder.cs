namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Registers the base Hotwire services.
	/// </summary>
	public interface IHotwireServiceBuilder
	{
		/// <summary>
		/// Adds a Hotwire Serialization handler.
		/// </summary>
		/// <typeparam name="T">The type of the serialization handler.</typeparam>
		/// <param name="defaultSerializer">Whether or not this is the default serialization handler.</param>
		/// <returns>A reference to the this builder.</returns>
		IHotwireServiceBuilder AddSerializer<T>(bool defaultSerializer = false) where T : class, IHotwireSerializer;

		/// <summary>
		/// Adds a Hotwire HTTP authentication handler.
		/// </summary>
		/// <typeparam name="T">The type of the authentication handler.</typeparam>
		/// <param name="defaultAuthentication">Whether or not this is the default authentication handler.</param>
		/// <returns>A reference to the this builder.</returns>
		IHotwireServiceBuilder AddAuthentication<T>(bool defaultAuthentication = false) where T : class, IHotwireAuthentication;
	}
}
