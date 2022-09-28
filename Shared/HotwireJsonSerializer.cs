using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EllipticBit.Hotwire.Shared
{
	public class HotwireJsonSerializer : IHotwireSerializer
	{
		private readonly JsonSerializerOptions settings;

		public HotwireJsonSerializer(JsonSerializerOptions settings, bool isDefault) {
			this.IsDefault = isDefault;
			this.settings = settings;
		}

		public string[] ContentTypes => new string[1] { "application/json" };

		public bool IsDefault { get; }

		public Task<T> Deserialize<T>(string input) {
			using var ms = new MemoryStream(Encoding.UTF8.GetBytes(input));
			return JsonSerializer.DeserializeAsync<T>(ms, settings).AsTask();
		}

		public async Task<string> Serialize<T>(T input) {
			using var ms = new MemoryStream();
			await JsonSerializer.SerializeAsync(ms, input, settings);
			return Encoding.UTF8.GetString(ms.ToArray());
		}
	}
}
