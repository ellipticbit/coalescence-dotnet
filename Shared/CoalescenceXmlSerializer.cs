using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EllipticBit.Coalescence.Shared
{
	/// <inheritdoc />
	public class CoalescenceXmlSerializer : ICoalescenceSerializer
	{
		private readonly XmlSerializationOptions settings;

		/// <summary>
		/// Creates a CoalescenceXmlSerializer
		/// </summary>
		/// <param name="settings">The <see cref="XmlSerializationOptions">XmlSerializationOptions</see> used by this serializer.</param>
		/// <param name="isDefault">Specifies that this is the default serializer to be used when no Content-Type is provided.</param>
		public CoalescenceXmlSerializer(XmlSerializationOptions settings, bool isDefault) {
			this.IsDefault = isDefault;
			this.settings = settings;
		}

		/// <inheritdoc />
		public string[] ContentTypes => new string[] { "text/xml", "application/xml"};

		/// <inheritdoc />
		public bool IsDefault { get; }

		/// <inheritdoc />
		public Task<T> Deserialize<T>(string input) {
			if (!settings.UseXmlSerializer) {
				var dcs = new DataContractSerializer(typeof(T), settings.GetDataContractSerializerSettings());
				var rs = XmlDictionaryReader.CreateTextReader(settings.Encoding.GetBytes(input), XmlDictionaryReaderQuotas.Max);
				return Task.FromResult((T)dcs.ReadObject(rs, true));
			}
			else {
				var xs = new XmlSerializer(typeof(T));
				var xws = settings.GetXmlWriterSettings();
				using var ms = new MemoryStream(settings.Encoding.GetBytes(input));
				using var xr = XmlReader.Create(ms, settings.GetXmlReaderSettings());
				return Task.FromResult((T)xs.Deserialize(xr));
			}
		}

		/// <inheritdoc />
		public Task<string> Serialize<T>(T input) {
			if (!settings.UseXmlSerializer) {
				var dcs = new DataContractSerializer(typeof(T), settings.GetDataContractSerializerSettings());
				using var ms = new MemoryStream();
				using var ws = XmlDictionaryWriter.CreateTextWriter(ms, settings.Encoding);
				dcs.WriteObject(ws, input);
				ws.Close();
				return Task.FromResult(settings.Encoding.GetString(ms.ToArray()));
			}
			else {
				var xs = new XmlSerializer(typeof(T));
				var xws = settings.GetXmlWriterSettings();
				using var ms = new MemoryStream();
				using var xw = XmlWriter.Create(ms, xws);
				xs.Serialize(xw, input);
				return Task.FromResult(settings.Encoding.GetString(ms.ToArray()));
			}
		}
	}
}
