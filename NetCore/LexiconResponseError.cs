using System;
using System.Net;

namespace EllipticBit.Lexicon.Client
{
	public sealed class LexiconResponseError : Exception
	{
		public HttpStatusCode StatusCode { get; }
		public string Content { get; }

		internal LexiconResponseError(HttpStatusCode statusCode, string message, string content) : base(message)
		{
			StatusCode = statusCode;
			Content = content;
		}

		public override string ToString() {
			return $"HTTP {StatusCode}: {Message}";
		}
	}
}
