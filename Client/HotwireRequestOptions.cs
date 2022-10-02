namespace EllipticBit.Hotwire.Client
{
	public sealed class HotwireRequestOptions
	{
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;

		public HotwireRequestOptions(string httpClientId = null)
		{
			HttpClientId = httpClientId;
		}
	}
}
