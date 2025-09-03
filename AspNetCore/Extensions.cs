using System;
using System.Collections.Generic;
using EllipticBit.Coalescence.AspNetCore.Constraints;

namespace EllipticBit.Coalescence.AspNetCore
{
	public static class Extensions
	{
		public static void AddCoalescenceConstraints(this IDictionary<string, Type> map) {
			map.Add("byte", typeof(ByteConstraint));
			map.Add("sbyte", typeof(SByteConstraint));
			map.Add("short", typeof(ShortConstraint));
			map.Add("ushort", typeof(UShortConstraint));
			map.Add("uint", typeof(UIntConstraint));
			map.Add("ulong", typeof(ULongConstraint));
			map.Add("DateTimeOffset", typeof(DateTimeOffsetConstraint));
			map.Add("TimeSpan", typeof(TimeSpanConstraint));
		}
	}
}
