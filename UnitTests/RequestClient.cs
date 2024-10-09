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
		private IServiceProvider services;

		[TestInitialize]
		public async Task Initialize() {
			var sb = new ServiceCollection();
			sb.AddHttpClient();
			sb.AddHttpClient("http-example-com", (http) => {
				http.BaseAddress = new Uri("http://example.com");
			});
			sb.AddHttpClient("technique-texting-webhook", client => {
				client.BaseAddress = new Uri("https://services.leadconnectorhq.com/");
			});
			sb.AddCoalescenceServices()
				.AddCoalescenceRequestOptions("test", new CoalescenceRequestOptions("test", "http-example-com"))
				.AddCoalescenceRequestOptions("technique-texting-webhook", new CoalescenceRequestOptions("technique-texting-webhook", "technique-texting-webhook"));
			sb.AddCoalescenceRequestServices();
			services = sb.BuildServiceProvider();
		}

		[TestMethod]
		public async Task BasicGet() {
			var factory = services.GetRequiredService<ICoalescenceRequestFactory>();
			var response = await factory.CreateRequest("test").Get().Authentication().Send();
			var text = await response.AsString();
			Debug.WriteLine(text);
		}
	}
}
