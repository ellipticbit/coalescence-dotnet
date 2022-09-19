using System;
using System.Text.Json;

namespace EllipticBit.Hotwire.Client
{
	public sealed class HotwireRequestOptions
	{
		public JsonSerializerOptions JsonSerializerOptions { get; set; }
		public XmlSerializerOptions XmlSerializerOptions { get; set; }
		public IHotwireAuthenticationHandler AuthenticationHandler { get; }
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;

		public HotwireRequestOptions(IHotwireAuthenticationHandler authenticationHandler, string httpClientId = null)
		{
			JsonSerializerOptions = new JsonSerializerOptions();
			XmlSerializerOptions = new XmlSerializerOptions();
			AuthenticationHandler = authenticationHandler ?? throw new ArgumentNullException(nameof(authenticationHandler));
			HttpClientId = httpClientId;
		}
	}
}
