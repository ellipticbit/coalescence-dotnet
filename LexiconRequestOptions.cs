using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EllipticBit.Lexicon.Client
{
	public class LexiconRequestOptions
	{
		public JsonSerializerOptions JsonSerializerOptions { get; set; }
		public XmlSerializerOptions XmlSerializerOptions { get; set; }
		public Func<string, Task<string>> GetAuthentication { get; set; }
		public string HttpClientId { get; set; }
	}
}
