using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EllipticBit.Coalescence.Shared;

namespace EllipticBit.Coalescence.Request
{
	internal sealed class CoalescenceContentItem
	{
		public HttpContentScheme Scheme { get; }
		public object Content { get; }
		public string ContentType { get; }
		public string Name { get; }
		public string FileName { get; }

		public CoalescenceContentItem(HttpContentScheme scheme, object content, string contentType, string name = null, string fileName = null)
		{
			Scheme = scheme;
			Content = content;
			ContentType = contentType;
			Name = name;
			FileName = fileName;
		}

		public CoalescenceContentItem(HttpContent content, string name = null, string fileName = null) {
			Scheme = HttpContentScheme.Content;
			Content = content;
			Name = name;
			FileName = fileName;
		}

		internal async Task<HttpContent> Build(IEnumerable<ICoalescenceSerializer> serializers) {
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
				var serializer = !string.IsNullOrWhiteSpace(ContentType) ? serializers.GetCoalescenceSerializer(ContentType) : serializers.GetDefaultCoalescenceSerializer();
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
