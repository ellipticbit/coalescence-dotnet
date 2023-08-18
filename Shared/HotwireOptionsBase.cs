using System.Collections.Generic;
using System.Text.Json;

namespace EllipticBit.Hotwire.Shared
{
	/// <summary>
	/// Provides a base class for Hotwire Options classes.
	/// </summary>
	public abstract class HotwireOptionsBase
	{
		/// <summary>
		/// Name used to identify this options set.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Read-only list of Serialization handlers.
		/// </summary>
		public IEnumerable<IHotwireSerializer> Serializers { get; }

		/// <summary>
		/// Read-only list of Authentication handlers.
		/// </summary>
		public IEnumerable<IHotwireAuthentication> Authenticators { get; }

		/// <summary>
		/// Default constructor for Hotwire options classes.
		/// </summary>
		/// <param name="name">Name of the options class used with creating a request.</param>
		/// <param name="jsonOptions">Serializer options for the default JSON serializer.</param>
		/// <param name="xmlOptions">Serializer options for the default XML serializer.</param>
		protected HotwireOptionsBase(string name, JsonSerializerOptions jsonOptions = null, XmlSerializationOptions xmlOptions = null) {
			Name = name;
			Serializers = new IHotwireSerializer[] {
				new HotwireJsonSerializer(jsonOptions ?? new JsonSerializerOptions(), true),
				new HotwireXmlSerializer(xmlOptions ?? new XmlSerializationOptions(), false)
			};
			Authenticators = new IHotwireAuthentication[] {
				new HotwireNullAuthentication()
			};
		}

		/// <summary>
		/// Constructor for derived Hotwire options classes.
		/// </summary>
		/// <param name="name">A name for this set options.</param>
		/// <param name="serializers">Optional parameter to initialize a custom set of Serialization handlers.</param>
		/// <param name="authenticators">Optional parameter to initialize a custom set of Authentication handlers.</param>
		protected HotwireOptionsBase(string name, IEnumerable<IHotwireSerializer> serializers, IEnumerable<IHotwireAuthentication> authenticators)
		{
			Name = name;
			Serializers = serializers ?? new IHotwireSerializer[] {
				new HotwireJsonSerializer(new JsonSerializerOptions(), true),
				new HotwireXmlSerializer(new XmlSerializationOptions(), false)
			};
			Authenticators = authenticators ?? new IHotwireAuthentication[] { new HotwireNullAuthentication() };
		}
	}
}
