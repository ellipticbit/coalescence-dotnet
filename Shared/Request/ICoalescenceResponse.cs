using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared.Request
{
	public interface ICoalescenceResponse : IDisposable, IAsyncDisposable
	{
		ICoalescenceResponse ThrowOnFailureResponse();

		CoalescenceResponseException AsError();
		IDictionary<string, string[]> AsHeaders();
		Task<HttpContent> AsContent();
		Task<T> AsDeserialized<T>();
		Task<byte[]> AsByteArray();
		Task<Stream> AsStream();
		Task<string> AsString();
		Task<IDictionary<string, string>> AsFormUrlEncoded();
	}
}
