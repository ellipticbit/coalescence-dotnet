using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconResponse
	{
		ILexiconResponse ThrowOnFailureResponse();
		ILexiconResponse GetResponseError(out LexiconResponseError error);

		Dictionary<string, string[]> AsHeaders();
		Task<HttpContent> AsContent();
		Task<T> AsObject<T>();
		Task<byte[]> AsByteArray();
		Task<Stream> AsStream();
		Task<string> AsText();
		Task<Dictionary<string, string>> AsFormUrlEncoded();
	}
}
