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
			this._scheme = useForm ? HttpContentScheme.MultipartForm : HttpContentScheme.Mutltipart;
			this._content = new List<LexiconContentItem>();
			this._options = options;
			_builder = builder;
		}

		public ILexiconMultipartContentBuilder Serialized<T>(T content, string name, HttpContentScheme scheme)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(scheme ?? this._scheme, content, (scheme ?? this._scheme).ToString(), name));
			return this;
		}

		public ILexiconMultipartContentBuilder File(byte[] content, string name, string fileName = null, string contentType = null) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Binary, content, contentType ?? HttpContentScheme.Binary.ToString(), name));
			return this;
		}

		public ILexiconMultipartContentBuilder File(Stream content, string name, string fileName = null, string contentType = null)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Stream, content, contentType ?? HttpContentScheme.Stream.ToString(), name));
			return this;
		}

		public ILexiconMultipartContentBuilder Text(string content, string name, string contentType = null) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.Text, content, contentType ?? HttpContentScheme.Text.ToString(), name));
			return this;
		}

		public ILexiconMultipartContentBuilder UrlEncoded(Dictionary<string, string> content, string name)
		{
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(HttpContentScheme.FormUrlEncoded, content, HttpContentScheme.FormUrlEncoded.ToString(), name));
			return this;
		}

		public ILexiconMultipartContentBuilder Content(HttpContent content, string name) {
			if (_scheme == HttpContentScheme.MultipartForm && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Must specify a name for Multipart Form Data content items.");
			_content.Add(new LexiconContentItem(content, name));
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

			if (_scheme == HttpContentScheme.Mutltipart) {
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
