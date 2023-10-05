using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Request
{
	public interface ICoalescenceResponse : IAsyncDisposable
	{
		ICoalescenceResponse ThrowOnFailureResponse();
		ICoalescenceResponse GetResponseError(out CoalescenceResponseError error);

		Dictionary<string, string[]> AsHeaders();
		Task<HttpContent> AsContent();
		Task<T> AsObject<T>();
		Task<byte[]> AsByteArray();
		Task<Stream> AsStream();
		Task<string> AsText();
		Task<Dictionary<string, string>> AsFormUrlEncoded();
	}
}
