using System.Collections.Generic;

namespace EllipticBit.Coalescence.Shared.Request
{
	public interface ICoalescenceParameters
	{
		IDictionary<string, IEnumerable<string>> GetParameters();
	}
}
