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

		public static ICoalescenceAuthentication GetCoalescenceAuthentication(this IEnumerable<ICoalescenceAuthentication> serializers, string name)
		{
			var authentications = serializers as ICoalescenceAuthentication[] ?? serializers.ToArray();
			if (string.IsNullOrEmpty(name)) return authentications.FirstOrDefault(a => a.Name == null);
			var auth = authentications.Where(a => a.Name != null).FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			if (auth == null) throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate authentication handler: {name}");
			return auth;
		}
	}
#pragma warning restore CS1591
}
