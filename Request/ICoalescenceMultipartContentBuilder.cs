using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using EllipticBit.Coalescence.Shared;

namespace EllipticBit.Coalescence.Request
{
	public interface ICoalescenceMultipartContentBuilder
	{
		ICoalescenceMultipartContentBuilder Serialized<T>(MultipartContentItem<T> content);
		ICoalescenceMultipartContentBuilder File(MultipartContentItem<byte[]> content);
		ICoalescenceMultipartContentBuilder File(MultipartContentItem<Stream> content);
		ICoalescenceMultipartContentBuilder Text(MultipartContentItem<string> content);
		ICoalescenceMultipartContentBuilder UrlEncoded(MultipartContentItem<Dictionary<string, string>> content);
		ICoalescenceMultipartContentBuilder Content(MultipartContentItem<HttpContent> content);
		ICoalescenceMultipartContentBuilder Subtype(string subtype);
		ICoalescenceMultipartContentBuilder Boundary(string boundary);
		ICoalescenceRequestBuilder Compile();
	}
}
