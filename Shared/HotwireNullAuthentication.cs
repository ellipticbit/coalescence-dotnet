using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Shared
{
	internal class HotwireNullAuthentication : IHotwireAuthentication
	{
		public string Scheme => null;

		public Task<bool> ContinueOnFailure(string userId, string tenantId) {
			return Task.FromResult(true);
		}

		public Task<string> Get(string userId, string tenantId) {
			return Task.FromResult<string>(null);
		}

		public Task<bool> Validate(string header, string tenantId) {
			return Task.FromResult(true);
		}

		public async Task<T> Decode<T>(string header, string tenantId) where T : class {
			return await Task.FromResult<T>(null);
		}
	}
}
