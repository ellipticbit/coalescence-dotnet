using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Request
{
	public interface IHotwireResponse
	{
		IHotwireResponse ThrowOnFailureResponse();
		IHotwireResponse GetResponseError(out HotwireResponseError error);

		Dictionary<string, string[]> AsHeaders();
		Task<HttpContent> AsContent();
		Task<T> AsObject<T>();
		Task<byte[]> AsByteArray();
		Task<Stream> AsStream();
		Task<string> AsText();
		Task<Dictionary<string, string>> AsFormUrlEncoded();
	}
}
