using System.Collections.Immutable;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Defines the interface for the Coalescence Options repository.
	/// </summary>
	public interface ICoalescenceOptionsRepository
	{
		/// <summary>
		/// A dictionary of HTTP Request options.
		/// </summary>
		ImmutableDictionary<string, CoalescenceOptionsBase> RequestOptions { get; }
		/// <summary>
		/// Returns the default options used in HTTP Request implementations.
		/// </summary>
		CoalescenceOptionsBase DefaultRequestOptions { get; }
		/// <summary>
		/// A dictionary of HTTP Controller options.
		/// </summary>
		ImmutableDictionary<string, CoalescenceOptionsBase> ControllerOptions { get; }
		/// <summary>
		/// Returns the default options used in HTTP Controller implementations.
		/// </summary>
		CoalescenceOptionsBase DefaultControllerOptions { get; }
		/// <summary>
		/// A dictionary of WebSocket options.
		/// </summary>
		ImmutableDictionary<string, CoalescenceOptionsBase> WebSocketOptions { get; }
		/// <summary>
		/// Returns the default options used in WebSocket implementations.
		/// </summary>
		CoalescenceOptionsBase DefaultWebSocketOptions { get; }
	}
}
