using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using EllipticBit.Coalescence.Shared;

namespace EllipticBit.Coalescence.Request
{
	public sealed class CoalescenceRequestOptions : CoalescenceOptionsBase
	{
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;
		public int RetryDelay { get; set; } = 0;
		public string DateTimeFormatString { get; set; } = "O";
		public string DefaultAuthencationScheme { get; set; } = null;
		public Action<HttpResponseMessage> ClientErrorHandler { get; set; } = null;

		public CoalescenceRequestOptions(string name, string httpClientId = null, JsonSerializerOptions jsonOptions = null, XmlSerializationOptions xmlOptions = null) : base(name, jsonOptions, xmlOptions)
		{
			HttpClientId = httpClientId;
		}

		public CoalescenceRequestOptions(string name, string httpClientId, IEnumerable<ICoalescenceSerializer> serializers) : base(name, serializers)
		{
			HttpClientId = httpClientId;
		}
	}
}
