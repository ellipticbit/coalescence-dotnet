using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconMultipartContentBuilder
	{
		ILexiconMultipartContentBuilder Serialized<T>(T content, string name, HttpContentScheme scheme);
		ILexiconMultipartContentBuilder File(byte[] content, string name, string fileName = null, string contentType = null);
		ILexiconMultipartContentBuilder File(Stream content, string name, string fileName = null, string contentType = null);
		ILexiconMultipartContentBuilder Text(string content, string name, string contentType = null);
		ILexiconMultipartContentBuilder UrlEncoded(Dictionary<string, string> content, string name);
		ILexiconMultipartContentBuilder Content(HttpContent content, string name);
		ILexiconMultipartContentBuilder Subtype(string subtype);
		ILexiconMultipartContentBuilder Boundary(string boundary);
		ILexiconRequestBuilder Compile();
	}
}
