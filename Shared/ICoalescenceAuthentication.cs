using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Interface defining a HTTP Authentication Scheme implementation.
	/// </summary>
	public interface ICoalescenceAuthentication
	{
		/// <summary>
		/// The name of this authentication implementation.
		/// </summary>
		string Name { get; }

		/// <summary>
		///	The HTTP Authentication Scheme implemented.
		/// </summary>
		string Scheme { get; }

		/// <summary>
		///	Determines whether the client should continue executing the method when the server returns a 401 error.
		/// </summary>
		/// <returns>Returns a boolean value indicating whether to continue execution.</returns>
		bool ContinueOnFailure { get; }

		/// <summary>
		///	Retrieves a complete credential to send to the server.
		/// </summary>
		/// <returns>The complete credential string.</returns>
		Task<string> Get();
	}
}
