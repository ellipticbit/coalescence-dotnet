using EllipticBit.Hotwire.Shared;

using System.Net.Http;

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
	}
}
