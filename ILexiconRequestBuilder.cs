using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	public interface ILexiconRequestBuilder
	{
		ILexiconRequestBuilder Path(params string[] parameter);
		ILexiconRequestBuilder Path(string parameter);
		ILexiconRequestBuilder Path(byte[] parameter);
		ILexiconRequestBuilder Path<T>(T parameter) where T : struct, IConvertible;
		ILexiconRequestBuilder Path<T>(T? parameter) where T : struct, IConvertible;

		ILexiconRequestBuilder Query(string key, IEnumerable<string> values);
		ILexiconRequestBuilder Query(string key, IEnumerable<byte[]> values);
		ILexiconRequestBuilder Query<T>(string key, IEnumerable<T> values) where T : struct, IConvertible;
		ILexiconRequestBuilder Query<T>(string key, IEnumerable<T?> values) where T : struct, IConvertible;

		ILexiconRequestBuilder Header(string key, IEnumerable<string> values);
		ILexiconRequestBuilder Header(string key, IEnumerable<byte[]> values);
		ILexiconRequestBuilder Header<T>(string key, IEnumerable<T> values) where T : struct, IConvertible;
		ILexiconRequestBuilder Header<T>(string key, IEnumerable<T?> values) where T : struct, IConvertible;

		ILexiconRequestBuilder SerializeContent<T>(T content, HttpContentScheme scheme);
		ILexiconRequestBuilder ByteArrayContent(byte[] content, string contentType = null);
		ILexiconRequestBuilder StreamContent(Stream content, string contentType = null);
		ILexiconRequestBuilder TextContent(string content, string contentType = null);
		ILexiconRequestBuilder FormUrlContent(Dictionary<string, string> content);

		ILexiconMultipartContentBuilder MultipartContent();
		ILexiconMultipartContentBuilder MultipartFormContent();

		ILexiconRequestBuilder Authenticate(string scheme = null);

		ILexiconRequestBuilder SuppressHttpResultExceptions();

		Task<HttpResponseMessage> Send();
		Task<T> Send<T>();
	}
}
