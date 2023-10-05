using System.Collections.Generic;
using System.Text.Json;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Provides a base class for Coalescence Options classes.
	/// </summary>
	public abstract class CoalescenceOptionsBase
	{
		/// <summary>
		/// Name used to identify this options set.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Read-only list of Serialization handlers.
		/// </summary>
		public IEnumerable<ICoalescenceSerializer> Serializers { get; }

		/// <summary>
		/// Read-only list of Authentication handlers.
		/// </summary>
		public IEnumerable<ICoalescenceAuthentication> Authenticators { get; }

		/// <summary>
		/// Default constructor for Coalescence options classes.
		/// </summary>
		/// <param name="name">Name of the options class used with creating a request.</param>
		/// <param name="jsonOptions">Serializer options for the default JSON serializer.</param>
		/// <param name="xmlOptions">Serializer options for the default XML serializer.</param>
		protected CoalescenceOptionsBase(string name, JsonSerializerOptions jsonOptions = null, XmlSerializationOptions xmlOptions = null) {
			Name = name;
			Serializers = new ICoalescenceSerializer[] {
				new CoalescenceJsonSerializer(jsonOptions ?? new JsonSerializerOptions(), true),
				new CoalescenceXmlSerializer(xmlOptions ?? new XmlSerializationOptions(), false)
			};
			Authenticators = new ICoalescenceAuthentication[] {
				new CoalescenceNullAuthentication()
			};
		}

		/// <summary>
		/// Constructor for derived Coalescence options classes.
		/// </summary>
		/// <param name="name">A name for this set options.</param>
		/// <param name="serializers">Optional parameter to initialize a custom set of Serialization handlers.</param>
		/// <param name="authenticators">Optional parameter to initialize a custom set of Authentication handlers.</param>
		protected CoalescenceOptionsBase(string name, IEnumerable<ICoalescenceSerializer> serializers, IEnumerable<ICoalescenceAuthentication> authenticators)
		{
			Name = name;
			Serializers = serializers ?? new ICoalescenceSerializer[] {
				new CoalescenceJsonSerializer(new JsonSerializerOptions(), true),
				new CoalescenceXmlSerializer(new XmlSerializationOptions(), false)
			};
			Authenticators = authenticators ?? new ICoalescenceAuthentication[] { new CoalescenceNullAuthentication() };
		}
	}
}
