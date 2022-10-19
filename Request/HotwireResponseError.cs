using System;
using System.Net;

namespace EllipticBit.Hotwire.Request
{
	public sealed class HotwireResponseError : Exception
	{
		public HttpStatusCode StatusCode { get; }
		public string Content { get; }

		internal HotwireResponseError(HttpStatusCode statusCode, string message, string content) : base(message)
		{
			StatusCode = statusCode;
			Content = content;
		}

		public override string ToString() {
			return $"HTTP {StatusCode}: {Message}";
		}
	}
}
