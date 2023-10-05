using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllipticBit.Coalescence.Request;
using EllipticBit.Coalescence.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
	public enum TestEnum
	{
		One,
		Two,
		Three
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
			sb.AddCoalescenceServices().AddCoalescenceRequestOptions("test", new CoalescenceRequestOptions("test", "http-example-com"));
			sb.AddCoalescenceRequestServices();
			services = sb.BuildServiceProvider();
		}

		[TestMethod]
		public async Task BasicGet() {
			var factory = services.GetRequiredService<ICoalescenceRequestFactory>();
			var response = await factory.CreateRequest("test").Get().Path(TestEnum.One).Path(1).Send();
			var text = await response.AsText();
			Debug.WriteLine(text);
		}
	}
}
