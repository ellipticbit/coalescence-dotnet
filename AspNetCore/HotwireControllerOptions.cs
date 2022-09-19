using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllipticBit.Hotwire.AspNetCore
{
	public sealed class HotwireControllerOptions
	{
		private readonly ImmutableDictionary<string, IHotwireSerializer> serializers;

		public HotwireControllerOptions() {

		}

		public HotwireControllerOptions(ImmutableDictionary<string, IHotwireSerializer> serializers)
		{
			this.serializers = serializers;
		}

		public IHotwireSerializer GetSerializer(string contentType) {
			if (serializers.TryGetValue(contentType.ToLowerInvariant(), out IHotwireSerializer serializer)) {
				return serializer;
			}

			throw new ArgumentOutOfRangeException(nameof(contentType), $"Unable to locate serializer for Content-Type: {contentType.ToLowerInvariant()}");
		}
	}
}
