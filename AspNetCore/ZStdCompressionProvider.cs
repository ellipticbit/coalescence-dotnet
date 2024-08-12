using System.IO;
using EllipticBit.Coalescence.Shared;
using Microsoft.AspNetCore.RequestDecompression;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using ZstdSharp;

namespace EllipticBit.Coalescence.AspNetCore
{
	public class ZStdCompressionProvider : ICompressionProvider
	{
		private readonly ZStdCompressionOptions options;

		public ZStdCompressionProvider(IOptions<ZStdCompressionOptions> options)
		{
			this.options = options.Value;
		}

		public Stream CreateStream(Stream outputStream)
		{
			var compressionStream = new CompressionStream(outputStream, (int)options.Level);
			return compressionStream;
		}

		public string EncodingName => "zstd";
		public bool SupportsFlush => true;
	}

	public class ZStdDecompressionProvider : IDecompressionProvider
	{
		public string EncodingName => "zstd";
		public bool SupportsFlush => true;

		public Stream GetDecompressionStream(Stream stream) {
			var compressionStream = new DecompressionStream(stream);
			return compressionStream;
		}
	}
}
