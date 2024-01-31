using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared
{
	/// <inheritdoc />
	internal class CoalescenceNullAuthentication : ICoalescenceAuthentication
	{
		/// <inheritdoc />
		public string Name => null;

		/// <inheritdoc />
		public string Scheme => null;

		/// <inheritdoc />
		public Task<bool> ContinueOnFailure() {
			return Task.FromResult(true);
		}

		/// <inheritdoc />
		public Task<string> Get() {
			return Task.FromResult<string>(null);
		}

		/// <inheritdoc />
		public Task<bool> Validate(string header) {
			return Task.FromResult(true);
		}

		/// <inheritdoc />
		public async Task<T> Decode<T>(string header) where T : class {
			return await Task.FromResult<T>(null);
		}
	}
}
