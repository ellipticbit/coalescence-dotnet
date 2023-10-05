using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EllipticBit.Coalescence.Shared
{
	/// <inheritdoc />
	public class CoalescenceJsonSerializer : ICoalescenceSerializer
	{
		private readonly JsonSerializerOptions settings;

		/// <summary>
		/// Creates a CoalescenceJsonSerializer
		/// </summary>
		/// <param name="settings">The System.Text.Json.JsonSerializerOptions used by this serializer.</param>
		/// <param name="isDefault">Specifies that this is the default serializer to be used when no Content-Type is provided.</param>
		public CoalescenceJsonSerializer(JsonSerializerOptions settings, bool isDefault) {
			this.IsDefault = isDefault;
			this.settings = settings;
		}

		/// <inheritdoc />
		public string[] ContentTypes => new string[1] { "application/json" };

		/// <inheritdoc />
		public bool IsDefault { get; }

		/// <inheritdoc />
		public Task<T> Deserialize<T>(string input) {
			using var ms = new MemoryStream(Encoding.UTF8.GetBytes(input));
			return JsonSerializer.DeserializeAsync<T>(ms, settings).AsTask();
		}

		/// <inheritdoc />
		public async Task<string> Serialize<T>(T input) {
			using var ms = new MemoryStream();
			await JsonSerializer.SerializeAsync(ms, input, settings);
			return Encoding.UTF8.GetString(ms.ToArray());
		}
	}
}
