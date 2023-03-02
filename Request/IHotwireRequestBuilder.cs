using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Request
{
	public interface IHotwireRequestBuilder
	{
		IHotwireRequestBuilder Path(params string[] parameter);
		IHotwireRequestBuilder Path(string parameter);
		IHotwireRequestBuilder Path(byte[] parameter);
		IHotwireRequestBuilder Path<T>(T parameter) where T : unmanaged, IComparable, IFormattable, IComparable<T>, IEquatable<T>;
		IHotwireRequestBuilder Path<T>(T? parameter) where T : unmanaged, IComparable, IFormattable, IComparable<T>, IEquatable<T>;

		IHotwireRequestBuilder Query(string key, IEnumerable<string> values);
		IHotwireRequestBuilder Query(string key, IEnumerable<byte[]> values);
		IHotwireRequestBuilder Query<T>(string key, IEnumerable<T> values) where T : unmanaged, IComparable, IFormattable, IComparable<T>, IEquatable<T>;
		IHotwireRequestBuilder Query<T>(string key, IEnumerable<T?> values) where T : unmanaged, IComparable, IFormattable, IComparable<T>, IEquatable<T>;
		IHotwireRequestBuilder Query<T>(T parameters) where T : class, IHotwireParameters;

		IHotwireRequestBuilder Header(string key, IEnumerable<string> values);
		IHotwireRequestBuilder Header(string key, IEnumerable<byte[]> values);
		IHotwireRequestBuilder Header<T>(string key, IEnumerable<T> values) where T : unmanaged, IComparable, IFormattable, IComparable<T>, IEquatable<T>;
		IHotwireRequestBuilder Header<T>(string key, IEnumerable<T?> values) where T : unmanaged, IComparable, IFormattable, IComparable<T>, IEquatable<T>;
		IHotwireRequestBuilder Header<T>(T parameters) where T : class, IHotwireParameters;

		IHotwireRequestBuilder Serialized<T>(T content, string contentType = null);
		IHotwireRequestBuilder ByteArray(byte[] content, string contentType = null);
		IHotwireRequestBuilder Stream(Stream content, string contentType = null);
		IHotwireRequestBuilder Text(string content, string contentType = null);
		IHotwireRequestBuilder FormUrlEncoded(Dictionary<string, string> content);
		IHotwireRequestBuilder Content(HttpContent content);
		IHotwireMultipartContentBuilder Multipart();
		IHotwireMultipartContentBuilder MultipartForm();

		IHotwireRequestBuilder Authentication(string scheme);
		IHotwireRequestBuilder Tenant(string tenantId);
		IHotwireRequestBuilder User(string userId);

		IHotwireRequestBuilder NoRetry();
		IHotwireRequestBuilder Timeout(TimeSpan timeout);

		Task<IHotwireResponse> Send();
	}
}
