using System.Collections.Generic;
using EllipticBit.Hotwire.Shared;

namespace EllipticBit.Hotwire.Request
{
	public sealed class HotwireRequestOptions : HotwireOptionsBase
	{
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;
		public string DateTimeFormatString { get; set; } = "O";

		public HotwireRequestOptions() : base()
		{
			HttpClientId = null;
		}

		public HotwireRequestOptions(string name, string httpClientId = null) : base(name)
		{
			HttpClientId = httpClientId;
		}

		public HotwireRequestOptions(string name, string httpClientId, IEnumerable<IHotwireSerializer> serializers, IEnumerable<IHotwireAuthentication> authenticators) : base(name, serializers, authenticators)
		{
			HttpClientId = httpClientId;
		}
	}
}
