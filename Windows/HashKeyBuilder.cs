using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Linq;
using System.Text;

namespace EllipticBit.Coalescence.Windows
{
	public class HashKeyBuilder
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

	public static class HashKeyBuilderExtensions
	{
		// Single-Value Methods
		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, short value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, int value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, long value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, ushort value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, uint value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, ulong value)
		{
			helper.AddBytes(BitConverter.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, byte value)
		{
			helper.AddByte(value);
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, sbyte value)
		{
			helper.AddByte((byte)value);
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, char value)
		{
			helper.AddByte((byte)value);
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, Guid value)
		{
			helper.AddBytes(value.ToByteArray());
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, DateTime value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value.ToString("O")));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, DateTimeOffset value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value.ToString("O")));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, TimeSpan value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value.ToString()));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, string value)
		{
			helper.AddBytes(Encoding.UTF8.GetBytes(value));
			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, byte[] value)
		{
			helper.AddBytes(value);
			return helper;
		}

		// Multi-Value Methods

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<short> value)
		{
			foreach (var v in value) {
				helper.AddBytes(BitConverter.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<int> value)
		{
			foreach (var v in value) {
				helper.AddBytes(BitConverter.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<long> value)
		{
			foreach (var v in value) {
				helper.AddBytes(BitConverter.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<ushort> value)
		{
			foreach (var v in value) {
				helper.AddBytes(BitConverter.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<uint> value)
		{
			foreach (var v in value) {
				helper.AddBytes(BitConverter.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<ulong> value)
		{
			foreach (var v in value) {
				helper.AddBytes(BitConverter.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<byte> value)
		{
			foreach (var v in value) {
				helper.AddByte(v);
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<sbyte> value)
		{
			foreach (var v in value) {
				helper.AddByte((byte)v);
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<char> value)
		{
			foreach (var v in value) {
				helper.AddByte((byte)v);
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<Guid> value)
		{
			foreach (var v in value) {
				helper.AddBytes(v.ToByteArray());
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<DateTime> value)
		{
			foreach (var v in value) {
				helper.AddBytes(Encoding.UTF8.GetBytes(v.ToString("O")));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<DateTimeOffset> value)
		{
			foreach (var v in value) {
				helper.AddBytes(Encoding.UTF8.GetBytes(v.ToString("O")));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<TimeSpan> value)
		{
			foreach (var v in value) {
				helper.AddBytes(Encoding.UTF8.GetBytes(v.ToString()));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<string> value)
		{
			foreach (var v in value) {
				helper.AddBytes(Encoding.UTF8.GetBytes(v));
			}

			return helper;
		}

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, IEnumerable<byte[]> value)
		{
			foreach (var v in value) {
				helper.AddBytes(v);
			}

			return helper;
		}

		//Params enabled methods

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params short[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params int[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params long[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params ushort[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params uint[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params ulong[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params sbyte[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params char[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params Guid[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params DateTime[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params DateTimeOffset[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params TimeSpan[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params string[] value) => AddKey(helper, value.ToList());

		public static HashKeyBuilder AddKey(this HashKeyBuilder helper, params byte[][] value) => AddKey(helper, value.ToList());
	}
}
