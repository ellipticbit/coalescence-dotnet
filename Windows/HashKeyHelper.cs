using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Text;

namespace EllipticBit.Coalescence.Windows
{
	public class HashKeyHelper
	{
		private List<byte> hashBytes = new(256);

		internal void AddBytes(byte[] bytes)
		{
			hashBytes.AddRange(bytes);
		}

		internal void AddByte(byte bytes)
		{
			hashBytes.Add(bytes);
		}

		public ulong HashKey => XxHash64.HashToUInt64(hashBytes.ToArray());
	}

	public static class HashKeyHelperExtensions
	{
		public static HashKeyHelper AddKey(this HashKeyHelper helper, short value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, int value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, long value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, ushort value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, uint value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, ulong value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, byte value)
		{
			helper.AddByte(value);
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, sbyte value)
		{
			helper.AddByte((byte)value);
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, char value)
		{
			helper.AddByte((byte)value);
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, Guid value)
		{
			helper.AddBytes(value.ToByteArray());
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, DateTime value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value.ToString("O")));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, DateTimeOffset value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value.ToString("O")));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, TimeSpan value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value.ToString()));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, string value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value));
			return helper;
		}

		public static HashKeyHelper AddKey(this HashKeyHelper helper, byte[] value)
		{
			helper.AddBytes(value);
			return helper;
		}
	}
}
