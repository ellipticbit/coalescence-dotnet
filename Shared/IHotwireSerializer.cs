using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Shared
{
	public interface IHotwireSerializer
	{
		string[] ContentTypes { get; }
		bool IsDefault { get; }

		Task<T> Deserialize<T>(string input);
		Task<string> Serialize<T>(T input);
	}
}
