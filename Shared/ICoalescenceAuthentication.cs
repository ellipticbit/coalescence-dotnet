using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Interface defining a HTTP Authentication Scheme implementation.
	/// </summary>
	public interface ICoalescenceAuthentication
	{
		/// <summary>
		///	The HTTP Authentication Scheme implemented.
		/// </summary>
		string Scheme { get; }

		/// <summary>
		///	Determines whether or not the client should continue executing the method when the server returns a 401 error.
		/// </summary>
		/// <param name="userId">An optional user identifier to help locate the correct token.</param>
		/// <param name="tenantId">An optional tenant identifier to help locate the correct token.</param>
		/// <returns>A Task that returns a boolean value indicating whether or not to continue execution.</returns>
		Task<bool> ContinueOnFailure(string userId, string tenantId);

		/// <summary>
		///	Retrieves a client token to send to the server.
		/// </summary>
		/// <param name="userId">An optional user identifier to help locate the correct token.</param>
		/// <param name="tenantId">An optional tenant identifier to help locate the correct token.</param>
		/// <returns></returns>
		Task<string> Get(string userId, string tenantId);

		/// <summary>
		/// Validates the authentication value provided by the client. If validation fails the request will not proceed further and will return an 401 Unauthorized error.
		/// </summary>
		/// <param name="header"></param>
		/// <param name="tenantId">An optional tenant identifier to help locate the correct token.</param>
		/// <returns>A Task that returns a boolean value indicating whether or the authentication is valid.</returns>
		Task<bool> Validate(string header, string tenantId);

		/// <summary>
		/// Decodes the authentication value provided by the client. May be used to include additional user data.
		/// </summary>
		/// <typeparam name="T">Object containing decoded authentication data.</typeparam>
		/// <param name="header">The authentication value received from the client.</param>
		/// <param name="tenantId">An optional tenant identifier to help locate the correct token.</param>
		/// <returns>A Task that returns the decoded authentication object.</returns>
		Task<T> Decode<T>(string header, string tenantId) where T : class;
	}
}
