using Microsoft.Extensions.Options;

namespace EllipticBit.Coalescence.Shared
{
	public enum ZStdCompressionLevel
	{
		None = 0,
		Fastest = 1,
		Fast = 3,
		Balanced = 5,
		Slow = 9,
		Slower = 12,
		Slowest = 19
	}

	public class ZStdCompressionOptions : IOptions<ZStdCompressionOptions>
	{
		public ZStdCompressionLevel Level { get; set; } = ZStdCompressionLevel.Balanced;

		/// <inheritdoc />
		ZStdCompressionOptions IOptions<ZStdCompressionOptions>.Value => this;
	}
}
