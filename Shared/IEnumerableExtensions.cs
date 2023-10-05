using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace EllipticBit.Coalescence.Shared
{
#pragma warning disable CS1591
	public static class IEnumerableExtensions
	{
		public static ICoalescenceSerializer GetDefaultCoalescenceSerializer(this IEnumerable<ICoalescenceSerializer> serializers)
		{
			return serializers.FirstOrDefault(a => a.IsDefault) ?? serializers.First();
		}

		public static ICoalescenceSerializer GetCoalescenceSerializer(this IEnumerable<ICoalescenceSerializer> serializers, string contentType)
		{
			var serializer = serializers.FirstOrDefault(a => a.ContentTypes.Any(b => b.Equals(contentType, StringComparison.OrdinalIgnoreCase)));
			if (serializer == null) throw new ArgumentOutOfRangeException(nameof(contentType), $"Unable to locate serializer for specified content-type: {contentType}");
			return serializer;
		}

		public static ICoalescenceAuthentication GetCoalescenceAuthentication(this IEnumerable<ICoalescenceAuthentication> serializers, string scheme)
		{
			var auth = serializers.FirstOrDefault(a => a.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase)) ?? serializers.FirstOrDefault(a => a.Scheme == null);
			if (auth == null) throw new ArgumentOutOfRangeException(nameof(scheme), $"Unable to locate authentication handler for specified scheme: {scheme}");
			return auth;
		}
	}
#pragma warning restore CS1591
}
