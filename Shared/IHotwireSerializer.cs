using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Shared
{
	public interface IHotwireSerializer
	{
		Task<T> Deserialize<T>(string input);
		Task<string> Serialize<T>(T input);
	}
}
