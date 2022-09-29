using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Interface defining a serializer implementation.
	/// </summary>
	public interface IHotwireSerializer
	{
		/// <summary>
		/// List of MIME types that this serializer can serialize/deserialize.
		/// </summary>
		string[] ContentTypes { get; }

		/// <summary>
		/// Specifies whether or not this is the default serializer used by the system. In the case where multiple serializers have this value set to 'true' the first match will be used.
		/// </summary>
		bool IsDefault { get; }

		/// <summary>
		/// Deserializes the provided document string to the specified object type.
		/// </summary>
		/// <typeparam name="T">The type to deserialize into.</typeparam>
		/// <param name="input">The document string to deserialize.</param>
		/// <returns>A Task returning the deserialized object.</returns>
		Task<T> Deserialize<T>(string input);

		/// <summary>
		/// Serializes the provided object to a document string.
		/// </summary>
		/// <typeparam name="T">The type to serialize.</typeparam>
		/// <param name="input">The object to serialize.</param>
		/// <returns>A Task that contains a string with the serialized result.</returns>
		Task<string> Serialize<T>(T input);
	}
}
