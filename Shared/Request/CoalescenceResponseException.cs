using System;
using System.Net;

namespace EllipticBit.Coalescence.Shared.Request
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class CoalescenceResponseError : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public HttpStatusCode StatusCode { get; }
		/// <summary>
		/// 
		/// </summary>
		public string Content { get; }

		internal CoalescenceResponseError(HttpStatusCode statusCode, string message, string content) : base(message)
		{
			StatusCode = statusCode;
			Content = content;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return $"HTTP {StatusCode}: {Message}";
		}
	}
}
