using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared
{
	/// <inheritdoc />
	public class CoalescenceNullAuthentication : ICoalescenceAuthentication
	{
		/// <inheritdoc />
		public string Scheme => null;

		/// <inheritdoc />
		public Task<bool> ContinueOnFailure(string userId, string tenantId) {
			return Task.FromResult(true);
		}

		/// <inheritdoc />
		public Task<string> Get(string userId, string tenantId) {
			return Task.FromResult<string>(null);
		}

		/// <inheritdoc />
		public Task<bool> Validate(string header, string tenantId) {
			return Task.FromResult(true);
		}

		/// <inheritdoc />
		public async Task<T> Decode<T>(string header, string tenantId) where T : class {
			return await Task.FromResult<T>(null);
		}
	}
}
