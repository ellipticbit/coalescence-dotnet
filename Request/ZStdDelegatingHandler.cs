﻿using System;
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
			if (request.Content != null && request.Content.Headers.ContentEncoding.Any(a => a.Equals("zstd", StringComparison.OrdinalIgnoreCase))) {
				var rs = await request.Content.ReadAsStreamAsync();
				request.Content = new StreamContent(new CompressionStream(rs, (int)options.Level));
			}

			var response = await base.SendAsync(request, cancellationToken);

			if (response.Content == null) return response;
			if (!response.Content.Headers.ContentEncoding.Any(a => a.Equals("zstd", StringComparison.OrdinalIgnoreCase))) return response;

			var tc = response.Content;
			response.Content = new StreamContent(new DecompressionStream(await tc.ReadAsStreamAsync()));
			foreach (var h in tc.Headers) {
				response.Content.Headers.Add(h.Key, h.Value);
			}

			return response;
		}
	}
}
