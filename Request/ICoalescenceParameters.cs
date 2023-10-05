using System.Collections.Generic;

namespace EllipticBit.Coalescence.Request
{
	public interface ICoalescenceParameters
	{
		IDictionary<string, IEnumerable<string>> GetParameters();
	}
}
