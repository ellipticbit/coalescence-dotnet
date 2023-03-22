using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace UnitTests
{
	public interface ITestServer
	{
		Task<string> Get(string name);
	}

	public interface ITestClient
	{
		Task<string> Get(string name);
	}

	internal class SignalRServer : ITestServer
	{
		private readonly HubConnection _hub;

		public SignalRServer(HubConnection hub) {
			this._hub = hub;
		}

		public Task<string> Get(string name) {
			return this._hub.InvokeCoreAsync<string>("Test", new object[] { name });
		}
	}

	public static class SignalRClientExtensions
	{
		public static void RegisterSignalRClientServices<TClient>(this IServiceCollection services) where TClient : class, ITestClient
		{
			services.TryAddTransient<ITestServer, SignalRServer>();
			services.TryAddTransient<ITestClient, TClient>();
		}

		public static void RegisterSignalRClientMethods(this HubConnection connection, IServiceProvider services)
		{
			connection.On("Customer.Test", (string name) => {
				var t = ActivatorUtilities.GetServiceOrCreateInstance<ITestClient>(services);
				return t.Get(name);
			});
		}
	}

	internal class SignalRClient : ITestClient
	{
		public Task<string> Get(string name) {
			return Task.FromResult(string.Empty);
		}
	}
}
