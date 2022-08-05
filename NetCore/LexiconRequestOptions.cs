using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	public class LexiconRequestOptions
	{
		public JsonSerializerOptions JsonSerializerOptions { get; }
		public XmlSerializerOptions XmlSerializerOptions { get; }
		public ILexiconAuthenticationHandler AuthenticationHandler { get; }
		public string HttpClientId { get; }

		public LexiconRequestOptions(ILexiconAuthenticationHandler authenticationHandler, string httpClientId = null)
		{
			JsonSerializerOptions = new JsonSerializerOptions();
			XmlSerializerOptions = new XmlSerializerOptions();
			AuthenticationHandler = authenticationHandler;
			HttpClientId = httpClientId;
		}
	}
}
