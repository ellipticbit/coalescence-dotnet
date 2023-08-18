using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllipticBit.Hotwire.Request;
using EllipticBit.Hotwire.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
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
			sb.AddHotwireServices().AddHotwireRequestOptions("test", new HotwireRequestOptions("test", "http-example-com"));
			sb.AddHotwireRequestServices();
			services = sb.BuildServiceProvider();
		}

		[TestMethod]
		public async Task BasicGet() {
			var factory = services.GetRequiredService<IHotwireRequestFactory>();
			var response = await factory.CreateRequest("test").Get().Send();
			var text = await response.AsText();
			Debug.WriteLine(text);
		}
	}
}
