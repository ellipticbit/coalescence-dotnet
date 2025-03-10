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
		public bool ContinueOnFailure => true;

		/// <inheritdoc />
		public Task<string> Get() {
			return Task.FromResult<string>(null);
		}
	}
}
