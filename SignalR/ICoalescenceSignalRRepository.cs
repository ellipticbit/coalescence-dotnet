using Microsoft.AspNetCore.SignalR.Client;

namespace EllipticBit.Coalescence.SignalR
{
	public interface ICoalescenceSignalRRepository
	{
		HubConnection Get();
		HubConnection Get(string name);
	}
}
