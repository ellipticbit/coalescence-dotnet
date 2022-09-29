namespace EllipticBit.Hotwire.Client
{
	public sealed class HotwireRequestOptions
	{
		public string DefaultSerializerContentType { get; }
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;

		public HotwireRequestOptions(string httpClientId = null, string defaultSerializationContentType = null)
		{
			DefaultSerializerContentType = defaultSerializationContentType;
			HttpClientId = httpClientId;
		}
	}
}
