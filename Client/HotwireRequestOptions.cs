using EllipticBit.Hotwire.Shared;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

namespace EllipticBit.Hotwire.Client
{
	public sealed class HotwireRequestOptions
	{
		public IHotwireAuthenticationHandler AuthenticationHandler { get; }
		public ImmutableDictionary<string, IHotwireSerializer> Serializers { get; }
		public IHotwireSerializer DefaultSerializer { get; }
		public string HttpClientId { get; }
		public int MaxRetryCount { get; set; } = 3;

		public HotwireRequestOptions(IHotwireAuthenticationHandler authenticationHandler, string httpClientId = null, IDictionary<string, IHotwireSerializer> serializers = null, string defaultSerializationContentType = null)
		{
			AuthenticationHandler = authenticationHandler ?? throw new ArgumentNullException(nameof(authenticationHandler));
			HttpClientId = httpClientId;

			if (serializers != null) {
				this.Serializers = serializers.ToImmutableDictionary();
				if (!this.Serializers.ContainsKey("application/json")) this.Serializers = this.Serializers.Add("application/json", new HotwireJsonSerializer(new JsonSerializerOptions()));
				if (!this.Serializers.ContainsKey("text/xml")) this.Serializers = this.Serializers.Add("text/xml", new HotwireXmlSerializer(new XmlSerializationOptions() { Indent = true }));
				if (!this.Serializers.ContainsKey("application/xml")) this.Serializers = this.Serializers.Add("application/xml", new HotwireXmlSerializer(new XmlSerializationOptions() { Indent = false }));
			}
		}
	}
}
