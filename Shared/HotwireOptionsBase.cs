using System.Collections.Generic;
using System.Collections.Immutable;
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
		protected HotwireOptionsBase() {
			Name = null;
			Serializers = new IHotwireSerializer[] {
				new HotwireJsonSerializer(new JsonSerializerOptions(), true),
				new HotwireXmlSerializer(new XmlSerializationOptions(), false)
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
		protected HotwireOptionsBase(string name, IEnumerable<IHotwireSerializer> serializers = null, IEnumerable<IHotwireAuthentication> authenticators = null)
		{
			Name = name;
			Serializers = serializers;
			Authenticators = authenticators;
		}
	}
}
