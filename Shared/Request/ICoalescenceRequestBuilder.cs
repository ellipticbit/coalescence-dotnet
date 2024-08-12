using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared.Request
{
	public interface ICoalescenceRequestBuilder
	{
		ICoalescenceRequestBuilder Path(params string[] parameter);
		ICoalescenceRequestBuilder Path(string parameter);
		ICoalescenceRequestBuilder Path(byte[] parameter);
		ICoalescenceRequestBuilder Path<T>(T parameter) where T : unmanaged, IComparable;
		ICoalescenceRequestBuilder Path<T>(T? parameter) where T : unmanaged, IComparable;

		ICoalescenceRequestBuilder Query(string key, IEnumerable<string> values);
		ICoalescenceRequestBuilder Query(string key, IEnumerable<byte[]> values);
		ICoalescenceRequestBuilder Query<T>(string key, IEnumerable<T> values) where T : unmanaged, IComparable;
		ICoalescenceRequestBuilder Query<T>(string key, IEnumerable<T?> values) where T : unmanaged, IComparable;
		ICoalescenceRequestBuilder Query<T>(T parameters) where T : class, ICoalescenceParameters;

		ICoalescenceRequestBuilder Header(string key, IEnumerable<string> values);
		ICoalescenceRequestBuilder Header(string key, IEnumerable<byte[]> values);
		ICoalescenceRequestBuilder Header<T>(string key, IEnumerable<T> values) where T : unmanaged, IComparable;
		ICoalescenceRequestBuilder Header<T>(string key, IEnumerable<T?> values) where T : unmanaged, IComparable;
		ICoalescenceRequestBuilder Header<T>(T parameters) where T : class, ICoalescenceParameters;

		ICoalescenceRequestBuilder Serialized<T>(T content, string contentType = null);
		ICoalescenceRequestBuilder ByteArray(byte[] content, string contentType = null);
		ICoalescenceRequestBuilder Stream(Stream content, string contentType = null);
		ICoalescenceRequestBuilder Text(string content, string contentType = null);
		ICoalescenceRequestBuilder FormUrlEncoded(Dictionary<string, string> content);
		ICoalescenceRequestBuilder Content(HttpContent content);
		ICoalescenceMultipartContentBuilder Multipart();
		ICoalescenceMultipartContentBuilder MultipartForm();
		ICoalescenceRequestBuilder RequestContentEncoding(string encoding);
		ICoalescenceRequestBuilder ResponseContentEncoding(string encoding);

		ICoalescenceRequestBuilder Authentication(string scheme = null);

		ICoalescenceRequestBuilder NoRetry();
		ICoalescenceRequestBuilder Timeout(TimeSpan timeout);

		Task<ICoalescenceResponse> Send();
	}
}
