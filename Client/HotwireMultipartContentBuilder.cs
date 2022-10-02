using EllipticBit.Hotwire.Shared;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Client
{
	internal sealed class HotwireMultipartContentBuilder : IHotwireMultipartContentBuilder
	{
		private readonly IHotwireRequestBuilder _builder;
		private readonly HttpContentScheme _scheme;
		private readonly List<HotwireContentItem> _content;
		private readonly HotwireRequestOptions _options;
		private readonly IEnumerable<IHotwireSerializer> _serializers;

		private string _subtype = null;
		private string _boundary = null;

		public HotwireMultipartContentBuilder(bool useForm, IHotwireRequestBuilder builder, HotwireRequestOptions options, IEnumerable<IHotwireSerializer> serializers)
		{
			this._scheme = useForm ? HttpContentScheme.MultipartForm : HttpContentScheme.Multipart;
			this._content = new List<HotwireContentItem>();
			this._options = options;
			this._serializers = serializers;
			_builder = builder;
		}

		public IHotwireMultipartContentBuilder Serialized<T>(MultipartContentItem<T> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new HotwireContentItem(HttpContentScheme.Serialized, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public IHotwireMultipartContentBuilder File(MultipartContentItem<byte[]> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new HotwireContentItem(HttpContentScheme.Binary, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public IHotwireMultipartContentBuilder File(MultipartContentItem<Stream> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new HotwireContentItem(HttpContentScheme.Stream, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public IHotwireMultipartContentBuilder Text(MultipartContentItem<string> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new HotwireContentItem(HttpContentScheme.Text, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public IHotwireMultipartContentBuilder UrlEncoded(MultipartContentItem<Dictionary<string, string>> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new HotwireContentItem(HttpContentScheme.FormUrlEncoded, content.Content, HttpContentScheme.FormUrlEncoded.ToString(), content.Name, content.FileName));
			return this;
		}

		public IHotwireMultipartContentBuilder Content(MultipartContentItem<HttpContent> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new HotwireContentItem(content.Content, content.Name, content.FileName));
			return this;
		}

		public IHotwireMultipartContentBuilder Subtype(string subtype)
		{
			if (string.IsNullOrWhiteSpace(subtype)) throw new ArgumentException("Subtype cannot be null or whitespace.", nameof(subtype));
			this._subtype = subtype;
			return this;
		}

		public IHotwireMultipartContentBuilder Boundary(string boundary)
		{
			if (boundary.Length > 70) throw new ArgumentException("Boundary cannot be longer then 70 characters.", nameof(boundary));
			if (string.IsNullOrWhiteSpace(boundary)) throw new ArgumentException("Boundary cannot be null or whitespace.", nameof(boundary));
			this._boundary = boundary;
			return this;
		}

		public IHotwireRequestBuilder Compile() {
			return _builder;
		}

		internal async Task<HttpContent> Build() {
			if (!_content.Any()) return null;

			if (_scheme == HttpContentScheme.Multipart) {
				var content = (!string.IsNullOrWhiteSpace(_subtype) && !string.IsNullOrWhiteSpace(_boundary)) ? new MultipartContent(_subtype, _boundary) :
					(!string.IsNullOrWhiteSpace(_subtype)) ? new MultipartContent(_subtype) :
					new MultipartContent();
				foreach (var ci in _content)
				{
					content.Add(await ci.Build(_serializers));
				}
			}
			else if (_scheme == HttpContentScheme.MultipartForm) {
				var content = string.IsNullOrWhiteSpace(_boundary) ? new MultipartFormDataContent() : new MultipartFormDataContent(_boundary);
				foreach (var ci in _content) {
					content.Add(await ci.Build(_serializers), null, ci.Name);
				}
			}
			else {
				throw new InvalidOperationException("Invalid content scheme selected for Multipart content.");
			}

			return null;
		}
	}
}
