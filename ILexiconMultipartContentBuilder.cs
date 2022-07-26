using System.Collections.Generic;
using System.IO;

namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconMultipartContentBuilder
	{
		ILexiconMultipartContentBuilder Part<T>(T content, HttpContentScheme scheme, string fileName = null);
		ILexiconMultipartContentBuilder Part(byte[] content, string fileName = null, string contentType = null);
		ILexiconMultipartContentBuilder Part(Stream content, string fileName = null, string contentType = null);
		ILexiconMultipartContentBuilder Part(string content, string fileName = null, string contentType = null);
		ILexiconMultipartContentBuilder Part(Dictionary<string, string> content, string fileName = null);
		ILexiconRequestBuilder Compile();
	}
}
