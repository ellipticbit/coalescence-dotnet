using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

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

		public async Task<HttpContent> Build(HotwireRequestOptions options)
		{
			if(this.Content is HttpContent content) return content;

			if (Scheme == HttpContentScheme.Binary)
			{
				return new ByteArrayContent((byte[])Content) { Headers = { ContentType = new MediaTypeHeaderValue(ContentType ?? Scheme.ToString()) } };
			}
			else if (Scheme == HttpContentScheme.Stream)
			{
				return new StreamContent((Stream)Content) { Headers = { ContentType = new MediaTypeHeaderValue(ContentType ?? Scheme.ToString()) } };
			}
			else if (Scheme == HttpContentScheme.Text)
			{
				return new StringContent((string)Content) { Headers = { ContentType = new MediaTypeHeaderValue(ContentType ?? Scheme.ToString()) } };
			}
			else if (Scheme == HttpContentScheme.Json)
			{
				using var ms = new MemoryStream();
				using var sr = new StreamReader(ms);
				await JsonSerializer.SerializeAsync(ms, Content, options.JsonSerializerOptions);
				return new StringContent(await sr.ReadToEndAsync()) { Headers = { ContentType = new MediaTypeHeaderValue(Scheme.ToString()) } };
			}
			else if (Scheme == HttpContentScheme.Xml)
			{
				var xml = new XmlMediaTypeFormatter();
				options.XmlSerializerOptions.ApplyOptions(xml);
				return new ObjectContent(Content.GetType(), Content, xml);
			}
			else if (Scheme == HttpContentScheme.FormUrlEncoded)
			{
				return new FormUrlEncodedContent((Dictionary<string, string>)Content);
			}

			return null;
		}
	}
}
