using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	public sealed class LexiconMultipartContentBuilder : ILexiconMultipartContentBuilder
	{
		private readonly ILexiconRequestBuilder _builder;
		private readonly HttpContentScheme _scheme;
		private readonly List<LexiconContentItem> _content;
		private readonly LexiconRequestOptions _options;

		private string _subtype = null;
		private string _boundary = null;

		public LexiconMultipartContentBuilder(bool useForm, ILexiconRequestBuilder builder, LexiconRequestOptions options)
		{
			this._scheme = useForm ? HttpContentScheme.MultipartForm : HttpContentScheme.Multipart;
			this._content = new List<LexiconContentItem>();
			this._options = options;
			_builder = builder;
		}

		public ILexiconMultipartContentBuilder Serialized<T>(MultipartContentItem<T> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Parse(content.ContentType), content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ILexiconMultipartContentBuilder File(MultipartContentItem<byte[]> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Binary, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ILexiconMultipartContentBuilder File(MultipartContentItem<Stream> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Stream, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ILexiconMultipartContentBuilder Text(MultipartContentItem<string> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Text, content.Content, content.ContentType, content.Name, content.FileName));
			return this;
		}

		public ILexiconMultipartContentBuilder UrlEncoded(MultipartContentItem<Dictionary<string, string>> content)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.FormUrlEncoded, content.Content, HttpContentScheme.FormUrlEncoded.ToString(), content.Name, content.FileName));
			return this;
		}

		public ILexiconMultipartContentBuilder Content(MultipartContentItem<HttpContent> content) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(content.Name)) throw new ArgumentNullException(nameof(content.Name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(content.Content, content.Name, content.FileName));
			return this;
		}

		public ILexiconMultipartContentBuilder Subtype(string subtype)
		{
			if (string.IsNullOrWhiteSpace(subtype)) throw new ArgumentException("Subtype cannot be null or whitespace.", nameof(subtype));
			this._subtype = subtype;
			return this;
		}

		public ILexiconMultipartContentBuilder Boundary(string boundary)
		{
			if (boundary.Length > 70) throw new ArgumentException("Boundary cannot be longer then 70 characters.", nameof(boundary));
			if (string.IsNullOrWhiteSpace(boundary)) throw new ArgumentException("Boundary cannot be null or whitespace.", nameof(boundary));
			this._boundary = boundary;
			return this;
		}

		public ILexiconRequestBuilder Compile() {
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
					content.Add(await ci.Build(_options));
				}
			}
			else if (_scheme == HttpContentScheme.MultipartForm) {
				var content = string.IsNullOrWhiteSpace(_boundary) ? new MultipartFormDataContent() : new MultipartFormDataContent(_boundary);
				foreach (var ci in _content) {
					content.Add(await ci.Build(_options), null, ci.Name);
				}
			}
			else {
				throw new InvalidOperationException("Invalid content scheme selected for Multipart content.");
			}

			return null;
		}
	}
}
