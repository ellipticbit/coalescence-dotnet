using System;
using System.Text.Json;

namespace EllipticBit.Lexicon.Client
{
	public sealed class LexiconRequestOptions
	{
		public JsonSerializerOptions JsonSerializerOptions { get; }
		public XmlSerializerOptions XmlSerializerOptions { get; }
		public ILexiconAuthenticationHandler AuthenticationHandler { get; }
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;

		public LexiconRequestOptions(ILexiconAuthenticationHandler authenticationHandler, string httpClientId = null)
		{
			JsonSerializerOptions = new JsonSerializerOptions();
			XmlSerializerOptions = new XmlSerializerOptions();
			AuthenticationHandler = authenticationHandler ?? throw new ArgumentNullException(nameof(authenticationHandler));
			HttpClientId = httpClientId;
		}
	}
}
