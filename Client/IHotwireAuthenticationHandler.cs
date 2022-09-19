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
	/*
		NOTE: Skip esoteric forms of auth for now.

		public sealed class DigestAuthenticationParameters
		{
			public bool UseMD5 { get; } = false;
		}

		public sealed class ScramAuthenticationParameters
		{
			public bool UseSHA1 { get; } = false;
		}
	*/
}
