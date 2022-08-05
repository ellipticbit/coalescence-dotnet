using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconResponse
	{
		Task<HttpContent> AsContent();
		Task<T> AsObject<T>();
		Task<byte[]> AsByteArray();
		Task<Stream> AsStream();
		Task<string> AsText();
		Task<Dictionary<string, string>> AsFormUrlEncoded();
	}
}
