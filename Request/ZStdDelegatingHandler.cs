using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EllipticBit.Coalescence.Shared;
using ZstdSharp;

namespace EllipticBit.Coalescence.Request
{
	/// <inheritdoc />
	public class ZStdDelegatingHandler : DelegatingHandler
	{
		private readonly ZStdCompressionOptions options;

		public ZStdDelegatingHandler(ZStdCompressionOptions options)
		{
			this.options = options;
		}

		/// <inheritdoc />
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			if (request.Content.Headers.ContentEncoding.Any(a => a.Equals("zstd", StringComparison.OrdinalIgnoreCase))) {
				request.Content = new StreamContent(new CompressionStream(await request.Content.ReadAsStreamAsync(), (int)options.Level));
			}

			var response = await base.SendAsync(request, cancellationToken);

			if (!response.Content.Headers.ContentEncoding.Any(a => a.Equals("zstd", StringComparison.OrdinalIgnoreCase))) return response;

			response.Content = new StreamContent(new DecompressionStream(await response.Content.ReadAsStreamAsync()));
			return response;
		}
	}
}
