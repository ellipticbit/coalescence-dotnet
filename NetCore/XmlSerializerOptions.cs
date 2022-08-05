using System.Net.Http.Formatting;

namespace EllipticBit.Lexicon.Client
{
	public class XmlSerializerOptions
	{
		public bool Indent { get; set; }
		public int MaxDepth { get; set; }
		public bool UseXmlSerializer { get; set; }

		internal void ApplyOptions(XmlMediaTypeFormatter xml) {
			xml.Indent = Indent;
			xml.MaxDepth = MaxDepth;
			xml.UseXmlSerializer = UseXmlSerializer;
		}
	}
}
