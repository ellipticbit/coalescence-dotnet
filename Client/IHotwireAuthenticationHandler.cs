using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Client
{
	public interface IHotwireAuthenticationHandler
	{
		Task<bool> CanContinue(string currentScheme, string tenantId);

		Task<string> Custom(string scheme, string tenantId);
		Task<(string username, string password)> Basic(string tenantId);
		Task<string> Bearer(string tenantId);
		//Task<DigestAuthenticationParameters> Digest();
		//Task<ScramAuthenticationParameters> Scram();
	}
}
