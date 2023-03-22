using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;

namespace EllipticBit.Hotwire.SignalR
{
	public interface IHotwireSignalRServiceBuilder
	{
		void AddHubConnection(string name, HubConnection connection);
	}
}
