using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EllipticBit.Coalescence.Request;
using EllipticBit.Coalescence.Shared;
using EllipticBit.Coalescence.Shared.Request;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
	public enum TestEnum
	{
		One,
		Two,
		Three
	}

	internal class TextContact
	{

		[JsonPropertyName("full_name")]
		public string Name { get; set; }

		[JsonPropertyName("email")]
		public string Email { get; set; }

		[JsonPropertyName("phone")]
		public string Phone { get; set; }
	}

	[TestClass]
	public class RequestClient
	{
		private static IServiceProvider services = null;

		[TestInitialize]
		public async Task Initialize() {
			if (services != null) return;

			var sb = new ServiceCollection();
			sb.AddHttpClient();
			sb.AddHttpClient("http-example-com", (http) => {
				http.BaseAddress = new Uri("http://example.com");
			});
			sb.AddCoalescenceServices()
				.AddCoalescenceRequestOptions("test", new CoalescenceRequestOptions("test", "http-example-com"));
			sb.AddCoalescenceRequestServices();
			services = sb.BuildServiceProvider();
		}

		[TestMethod]
		public async Task BasicGet()
		{
			var factory = services.GetRequiredService<ICoalescenceRequestFactory>();
			await using var response = await factory.CreateRequest("test").Get().Authentication().Send();
			var text = await response.AsString();
			Debug.WriteLine(text);
		}

		[TestMethod]
		public async Task BasicDelete()
		{
			var factory = services.GetRequiredService<ICoalescenceRequestFactory>();
			var request = factory.CreateRequest("test").Delete()
				.Path("api", "test", "c")
				.Path("old", "a", "delete")
				.Serialized(new TextContact() { Email = "test@test.com", Name = "Test", Phone = "(111) 111-1111" });
			await using var response = await request.Send();
			var text = await response.AsString();
			Debug.WriteLine(text);
		}
	}
}
