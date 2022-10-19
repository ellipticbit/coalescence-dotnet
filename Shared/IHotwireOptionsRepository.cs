using System.Collections.Immutable;

namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Defines the interface for the Hotwire Options repository.
	/// </summary>
	public interface IHotwireOptionsRepository
	{
		/// <summary>
		/// A dictionary of HTTP Request options.
		/// </summary>
		ImmutableDictionary<string, HotwireOptionsBase> RequestOptions { get; }
		/// <summary>
		/// Returns the default options used in HTTP Request implementations.
		/// </summary>
		HotwireOptionsBase DefaultRequestOptions { get; }
		/// <summary>
		/// A dictionary of HTTP Controller options.
		/// </summary>
		ImmutableDictionary<string, HotwireOptionsBase> ControllerOptions { get; }
		/// <summary>
		/// Returns the default options used in HTTP Controller implementations.
		/// </summary>
		HotwireOptionsBase DefaultControllerOptions { get; }
		/// <summary>
		/// A dictionary of WebSocket options.
		/// </summary>
		ImmutableDictionary<string, HotwireOptionsBase> WebSocketOptions { get; }
		/// <summary>
		/// Returns the default options used in WebSocket implementations.
		/// </summary>
		HotwireOptionsBase DefaultWebSocketOptions { get; }
	}
}
