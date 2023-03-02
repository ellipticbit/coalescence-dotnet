using System.Collections.Generic;

namespace EllipticBit.Hotwire.Request
{
	public interface IHotwireParameters
	{
		IDictionary<string, IEnumerable<string>> GetParameters();
	}
}
