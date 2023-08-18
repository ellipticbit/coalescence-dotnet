using System.Collections.Generic;
using System.Text.Json;
using EllipticBit.Hotwire.Shared;

namespace EllipticBit.Hotwire.Request
{
	public sealed class HotwireRequestOptions : HotwireOptionsBase
	{
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;
		public string DateTimeFormatString { get; set; } = "O";

		public HotwireRequestOptions(string name, string httpClientId = null, JsonSerializerOptions jsonOptions = null, XmlSerializationOptions xmlOptions = null) : base(name, jsonOptions, xmlOptions)
		{
			HttpClientId = httpClientId;
		}

		public HotwireRequestOptions(string name, string httpClientId, IEnumerable<IHotwireSerializer> serializers, IEnumerable<IHotwireAuthentication> authenticators) : base(name, serializers, authenticators)
		{
			HttpClientId = httpClientId;
		}
	}
}
