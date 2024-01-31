using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EllipticBit.Coalescence.Shared;
using EllipticBit.Coalescence.Shared.Request;

namespace EllipticBit.Coalescence.Request
{
	internal sealed class CoalescenceMultipartContentBuilder : ICoalescenceMultipartContentBuilder
	{
		private readonly ICoalescenceRequestBuilder _builder;
		private readonly HttpContentScheme _scheme;
		private readonly List<CoalescenceContentItem> _content;
		private readonly CoalescenceRequestOptions _options;

		private string _subtype = null;
		private string _boundary = null;

		public CoalescenceMultipartContentBuilder(bool useForm, ICoalescenceRequestBuilder builder, CoalescenceRequestOptions options)
		{
			this._scheme = useForm ? HttpContentScheme.MultipartForm : HttpContentScheme.Multipart;
			this._content = new List<CoalescenceContentItem>();
			this._options = options;
			_builder = builder;
		}

		public ICoalescenceMultipartContentBuilder Serialized<T>(MultipartContentItem<T> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new CoalescenceContentItem(HttpContentScheme.Serialized, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ICoalescenceMultipartContentBuilder File(MultipartContentItem<byte[]> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new CoalescenceContentItem(HttpContentScheme.Binary, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ICoalescenceMultipartContentBuilder File(MultipartContentItem<Stream> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new CoalescenceContentItem(HttpContentScheme.Stream, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ICoalescenceMultipartContentBuilder Text(MultipartContentItem<string> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new CoalescenceContentItem(HttpContentScheme.Text, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ICoalescenceMultipartContentBuilder UrlEncoded(MultipartContentItem<Dictionary<string, string>> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new CoalescenceContentItem(HttpContentScheme.FormUrlEncoded, content.Content, HttpContentScheme.FormUrlEncoded.ToString(), content.Name, content.FileName));
			return this;
		}

		public ICoalescenceMultipartContentBuilder Content(MultipartContentItem<HttpContent> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new CoalescenceContentItem(content.Content, content.Name, content.FileName));
			return this;
		}

		public ICoalescenceMultipartContentBuilder Subtype(string subtype)
		{
			if (string.IsNullOrWhiteSpace(subtype)) throw new ArgumentException("Subtype cannot be null or whitespace.", nameof(subtype));
			this._subtype = subtype;
			return this;
		}

		public ICoalescenceMultipartContentBuilder Boundary(string boundary)
		{
			if (boundary.Length > 70) throw new ArgumentException("Boundary cannot be longer then 70 characters.", nameof(boundary));
			if (string.IsNullOrWhiteSpace(boundary)) throw new ArgumentException("Boundary cannot be null or whitespace.", nameof(boundary));
			this._boundary = boundary;
			return this;
		}

		public ICoalescenceRequestBuilder Compile() {
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
					content.Add(await ci.Build(_options.Serializers));
				}
			}
			else if (_scheme == HttpContentScheme.MultipartForm) {
				var content = string.IsNullOrWhiteSpace(_boundary) ? new MultipartFormDataContent() : new MultipartFormDataContent(_boundary);
				foreach (var ci in _content) {
					content.Add(await ci.Build(_options.Serializers), null, ci.Name);
				}
			}
			else {
				throw new InvalidOperationException("Invalid content scheme selected for Multipart content.");
			}

			return null;
		}
	}
}
