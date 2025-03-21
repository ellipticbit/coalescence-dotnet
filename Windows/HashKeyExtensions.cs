using System;

namespace EllipticBit.Coalescence.Windows
{
	public static class HashKeyExtensions
	{
		public static ulong AsHashKey(this short value) {
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this int value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this long value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this ushort value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this uint value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this ulong value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this byte value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this sbyte value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this char value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this Guid value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this DateTime value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this DateTimeOffset value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this TimeSpan value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this string value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}

		public static ulong AsHashKey(this byte[] value)
		{
			return (new HashKeyBuilder()).AddKey(value).HashKey;
		}
	}
}
