using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace EllipticBit.Hotwire.Shared
{
#pragma warning disable CS1591
	public class XmlSerializationOptions
	{
		//DataContractSerializer options
		public bool SerializeReadOnlyTypes { get; set; } = true;
		public bool IgnoreExtensionDataObject { get; set; } = true;
		public int MaxItemsInObjectGraph { get; set; } = int.MaxValue;
		public bool PreserveObjectReferences { get; set; } = false;
		public IEnumerable<Type> KnownTypes { get; set; } = null;
		public DataContractResolver DataContractResolver { get; set; } = null;

		// XmlReader/XmlWriter options
		public bool CheckCharacters { get; set; } = true;
		public bool Indent { get; set; } = false;
		public string IndentChars { get; set; } = "  ";
		public bool IgnoreComments { get; set; } = true;
		public bool IgnoreProcessingInstructions { get; set; } = true;
		public bool IgnoreWhitespace { get; set; } = true;
		public bool DoNotEscapeUriAttributes { get; set; } = false;
		public bool NewLineOnAttributes { get; set; } = false;
		public string NewLineChars { get; set; } = "\r\n";
		public bool OmitXmlDeclaration { get; set; } = false;
		public ConformanceLevel ConformanceLevel { get; set; } = ConformanceLevel.Auto;
		public XmlSchemaValidationFlags ValidationFlags { get; set; } = XmlSchemaValidationFlags.None;
		public NamespaceHandling NamespaceHandling { get; set; } = NamespaceHandling.Default;
		public NewLineHandling NewLineHandling { get; set; } = NewLineHandling.None;
		public int MaxDepth { get; set; }

		//Shared options
		public Encoding Encoding { get; set; } = Encoding.UTF8;
		public bool UseXmlSerializer { get; set; }

		internal DataContractSerializerSettings GetDataContractSerializerSettings() {
			return new DataContractSerializerSettings() {
				SerializeReadOnlyTypes = SerializeReadOnlyTypes,
				KnownTypes = KnownTypes,
				IgnoreExtensionDataObject = IgnoreExtensionDataObject,
				MaxItemsInObjectGraph = MaxItemsInObjectGraph,
				PreserveObjectReferences = PreserveObjectReferences,
				DataContractResolver = DataContractResolver,
			};
		}

		internal XmlReaderSettings GetXmlReaderSettings()
		{
			return new XmlReaderSettings()
			{
				Async = false,
				CheckCharacters = CheckCharacters,
				CloseInput = true,
				ConformanceLevel = ConformanceLevel,
				ValidationFlags = ValidationFlags,
				IgnoreComments = IgnoreComments,
				IgnoreProcessingInstructions = IgnoreProcessingInstructions,
				IgnoreWhitespace = IgnoreWhitespace,
			};
		}

		internal XmlWriterSettings GetXmlWriterSettings()
		{
			return new XmlWriterSettings()
			{
				Async = false,
				CheckCharacters = CheckCharacters,
				Indent = Indent,
				Encoding = Encoding,
				CloseOutput = true,
				ConformanceLevel = ConformanceLevel,
				WriteEndDocumentOnClose = true,
				IndentChars = IndentChars,
				DoNotEscapeUriAttributes = DoNotEscapeUriAttributes,
				NewLineChars = NewLineChars,
				NamespaceHandling = NamespaceHandling,
				NewLineHandling = NewLineHandling,
				NewLineOnAttributes = NewLineOnAttributes,
				OmitXmlDeclaration = OmitXmlDeclaration
			};
		}
	}
#pragma warning restore CS1591
}
