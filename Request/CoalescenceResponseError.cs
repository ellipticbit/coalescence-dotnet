using System;
using System.Net;

namespace EllipticBit.Coalescence.Request
{
	public sealed class CoalescenceResponseError : Exception
	{
		public HttpStatusCode StatusCode { get; }
		public string Content { get; }

		internal CoalescenceResponseError(HttpStatusCode statusCode, string message, string content) : base(message)
		{
			StatusCode = statusCode;
			Content = content;
		}

		public override string ToString() {
			return $"HTTP {StatusCode}: {Message}";
		}
	}
}
