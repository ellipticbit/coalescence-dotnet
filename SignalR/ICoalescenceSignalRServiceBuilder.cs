using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;

namespace EllipticBit.Coalescence.SignalR
{
	public interface ICoalescenceSignalRServiceBuilder
	{
		void AddHubConnection(string name, HubConnection connection);
	}
}
