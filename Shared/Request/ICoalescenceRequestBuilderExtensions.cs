using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticBit.Coalescence.Shared.Request
{
	public static class ICoalescenceRequestBuilderExtensions
	{
		public static ICoalescenceRequestBuilder Query(this ICoalescenceRequestBuilder builder, string key, string value) {
			return builder.Query(key, new [] { value });
		}

		public static ICoalescenceRequestBuilder Query(this ICoalescenceRequestBuilder builder, string key, byte[] value) {
			return builder.Query(key, new [] { value });
		}

		public static ICoalescenceRequestBuilder Query<T>(this ICoalescenceRequestBuilder builder, string key, T value) where T : unmanaged, IComparable, IFormattable {
			return builder.Query(key, new[] { value });
		}
		public static ICoalescenceRequestBuilder Query<T>(this ICoalescenceRequestBuilder builder, string key, T? value) where T : unmanaged, IComparable, IFormattable {
			return builder.Query(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header(this ICoalescenceRequestBuilder builder, string key, string value) {
			return builder.Header(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header(this ICoalescenceRequestBuilder builder, string key, byte[] value) {
			return builder.Header(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header<T>(this ICoalescenceRequestBuilder builder, string key, T value) where T : unmanaged, IComparable, IFormattable {
			return builder.Header(key, new[] { value });
		}

		public static ICoalescenceRequestBuilder Header<T>(this ICoalescenceRequestBuilder builder, string key, T? value) where T : unmanaged, IComparable, IFormattable {
			return builder.Header(key, new[] { value });
		}
	}
}
