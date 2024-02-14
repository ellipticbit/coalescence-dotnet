using System;

namespace EllipticBit.Coalescence.Shared.Request
{
	public static class ICoalescenceRequestBuilderExtensions
	{
		public static ICoalescenceRequestBuilder Query(this ICoalescenceRequestBuilder builder, string key, string value) {
			if (value == null) return builder;
			return builder.Query(key, new [] { value });
		}

		public static ICoalescenceRequestBuilder Query(this ICoalescenceRequestBuilder builder, string key, byte[] value) {
			if (value == null) return builder;
			return builder.Query(key, new [] { value });
		}

		public static ICoalescenceRequestBuilder Query<T>(this ICoalescenceRequestBuilder builder, string key, T value) where T : unmanaged, IComparable, IFormattable {
			return builder.Query(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Query<T>(this ICoalescenceRequestBuilder builder, string key, T? value) where T : unmanaged, IComparable, IFormattable {
			if (value == null) return builder;
			return builder.Query(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header(this ICoalescenceRequestBuilder builder, string key, string value) {
			if (value == null) return builder;
			return builder.Header(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header(this ICoalescenceRequestBuilder builder, string key, byte[] value) {
			if (value == null) return builder;
			return builder.Header(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header<T>(this ICoalescenceRequestBuilder builder, string key, T value) where T : unmanaged, IComparable, IFormattable {
			return builder.Header(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header<T>(this ICoalescenceRequestBuilder builder, string key, T? value) where T : unmanaged, IComparable, IFormattable {
			if (value == null) return builder;
			return builder.Header(key, new[] { value });
		}
	}
}
