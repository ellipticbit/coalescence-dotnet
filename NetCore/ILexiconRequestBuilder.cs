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

		ILexiconRequestBuilder Serialized<T>(T content, HttpContentScheme scheme);
		ILexiconRequestBuilder ByteArray(byte[] content, string contentType = null);
		ILexiconRequestBuilder Stream(Stream content, string contentType = null);
		ILexiconRequestBuilder Text(string content, string contentType = null);
		ILexiconRequestBuilder FormUrlEncoded(Dictionary<string, string> content);
		ILexiconRequestBuilder Content(HttpContent content);
		ILexiconMultipartContentBuilder Multipart();
		ILexiconMultipartContentBuilder MultipartForm();

		ILexiconRequestBuilder BasicAuthentication(string tenantId = null);
		ILexiconRequestBuilder BearerAuthentication(string tenantId = null);
		ILexiconRequestBuilder CustomAuthentication(string scheme, string tenantId = null);

		ILexiconRequestBuilder Timeout(TimeSpan timeout);

		Task<ILexiconResponse> Send();
	}
}
