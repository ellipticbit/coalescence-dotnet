using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.SignalR.Client;

namespace EllipticBit.Hotwire.SignalR
{
	internal class HotwireSignalRRepository : IHotwireSignalRRepository, IHotwireSignalRServiceBuilder
	{
		private static ImmutableDictionary<string, HubConnection> _hc = ImmutableDictionary<string, HubConnection>.Empty;
		private static HubConnection _dhc = null;

		internal HotwireSignalRRepository(HubConnection defaultConnection) {
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
