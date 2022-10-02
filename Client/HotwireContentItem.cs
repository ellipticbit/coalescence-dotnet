using System.Collections.Generic;
using EllipticBit.Hotwire.Shared;

using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace EllipticBit.Hotwire.Client
{
	internal sealed class HotwireContentItem
	{
		public HttpContentScheme Scheme { get; }
		public object Content { get; }
		public string ContentType { get; }
		public string Name { get; }
		public string FileName { get; }

		public HotwireContentItem(HttpContentScheme scheme, object content, string contentType, string name = null, string fileName = null)
		{
			Scheme = scheme;
			Content = content;
			ContentType = contentType;
			Name = name;
			FileName = fileName;
		}

		public HotwireContentItem(HttpContent content, string name = null, string fileName = null) {
			Scheme = HttpContentScheme.Content;
			Content = content;
			Name = name;
			FileName = fileName;
		}

		internal async Task<HttpContent> Build(IEnumerable<IHotwireSerializer> serializers) {
			if (Content is HttpContent content) return content;

			if (Scheme == HttpContentScheme.Binary)
			{
				return new ByteArrayContent((byte[])Content) { Headers = { ContentType = new MediaTypeHeaderValue(ContentType ?? "application/octet-stream") } };
			}
			else if (Scheme == HttpContentScheme.Stream)
			{
				return new StreamContent((Stream)Content) { Headers = { ContentType = new MediaTypeHeaderValue(ContentType ?? "application/octet-stream") } };
			}
			else if (Scheme == HttpContentScheme.Text)
			{
				return new StringContent((string)Content) { Headers = { ContentType = new MediaTypeHeaderValue(ContentType ?? "text/plain") } };
			}
			else if (Scheme == HttpContentScheme.Serialized)
			{
				var serializer = string.IsNullOrWhiteSpace(ContentType) ? serializers.GetHotwireSerializer(ContentType) : serializers.GetDefaultHotwireSerializer();
				return new StringContent(await serializer.Serialize(Content)) { Headers = { ContentType = new MediaTypeHeaderValue(serializer.ContentTypes.First()) } };
			}
			else if (Scheme == HttpContentScheme.FormUrlEncoded)
			{
				return new FormUrlEncodedContent((Dictionary<string, string>)Content);
			}

			return null;
		}
	}
}
