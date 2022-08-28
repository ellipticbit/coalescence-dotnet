using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconMultipartContentBuilder
	{
		ILexiconMultipartContentBuilder Serialized<T>(MultipartContentItem<T> content);
		ILexiconMultipartContentBuilder File(MultipartContentItem<byte[]> content);
		ILexiconMultipartContentBuilder File(MultipartContentItem<Stream> content);
		ILexiconMultipartContentBuilder Text(MultipartContentItem<string> content);
		ILexiconMultipartContentBuilder UrlEncoded(MultipartContentItem<Dictionary<string, string>> content);
		ILexiconMultipartContentBuilder Content(MultipartContentItem<HttpContent> content);
		ILexiconMultipartContentBuilder Subtype(string subtype);
		ILexiconMultipartContentBuilder Boundary(string boundary);
		ILexiconRequestBuilder Compile();
	}
}
