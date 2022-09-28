using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Shared
{
	public interface IHotwireAuthentication
	{
		string Scheme { get; }

		Task<string> Get();
		Task<bool> Validate(string header);
		Task<T> Decode<T>(string header) where T : class;
	}
}
