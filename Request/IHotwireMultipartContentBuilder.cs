using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using EllipticBit.Hotwire.Shared;

namespace EllipticBit.Hotwire.Request
{
	public interface IHotwireMultipartContentBuilder
	{
		IHotwireMultipartContentBuilder Serialized<T>(MultipartContentItem<T> content);
		IHotwireMultipartContentBuilder File(MultipartContentItem<byte[]> content);
		IHotwireMultipartContentBuilder File(MultipartContentItem<Stream> content);
		IHotwireMultipartContentBuilder Text(MultipartContentItem<string> content);
		IHotwireMultipartContentBuilder UrlEncoded(MultipartContentItem<Dictionary<string, string>> content);
		IHotwireMultipartContentBuilder Content(MultipartContentItem<HttpContent> content);
		IHotwireMultipartContentBuilder Subtype(string subtype);
		IHotwireMultipartContentBuilder Boundary(string boundary);
		IHotwireRequestBuilder Compile();
	}
}
