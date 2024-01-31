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
		/// <returns>A Task that returns a boolean value indicating whether to continue execution.</returns>
		Task<bool> ContinueOnFailure();

		/// <summary>
		///	Retrieves a client token to send to the server.
		/// </summary>
		/// <returns></returns>
		Task<string> Get();

		/// <summary>
		/// Validates the authentication value provided by the client. If validation fails the request will not proceed further and will return an 401 Unauthorized error.
		/// </summary>
		/// <param name="header">The authentication value received from the client.</param>
		/// <returns>A Task that returns a boolean value indicating whether or the authentication is valid.</returns>
		Task<bool> Validate(string header);

		/// <summary>
		/// Decodes the authentication value provided by the client. May be used to include additional user data.
		/// </summary>
		/// <typeparam name="T">Object containing decoded authentication data.</typeparam>
		/// <param name="header">The authentication value received from the client.</param>
		/// <returns>A Task that returns the decoded authentication object.</returns>
		Task<T> Decode<T>(string header) where T : class;
	}
}
