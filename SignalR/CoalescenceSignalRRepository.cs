using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Coalescence.SignalR
{
	internal class CoalescenceSignalRRepository : ICoalescenceSignalRRepository, ICoalescenceSignalRServiceBuilder
	{
		private static ImmutableDictionary<string, HubConnection> _hc = ImmutableDictionary<string, HubConnection>.Empty;
		private static HubConnection _dhc = null;

		[ActivatorUtilitiesConstructor]
		public CoalescenceSignalRRepository() { }

		internal CoalescenceSignalRRepository(HubConnection defaultConnection) {
			_dhc = defaultConnection;
		}

		public HubConnection Get() {
			return _dhc;
		}

		public HubConnection Get(string name) {
			if (_hc.TryGetValue(name, out HubConnection hc)) {
				return hc;
			}

			throw new KeyNotFoundException(name);
		}

		public void AddHubConnection(string name, HubConnection connection) {
			_hc = _hc.Add(name, connection);
		}
	}
}
