using Microsoft.AspNetCore.SignalR.Client;

namespace EllipticBit.Hotwire.SignalR
{
	public interface IHotwireSignalRRepository
	{
		HubConnection Get();
		HubConnection Get(string name);
	}
}
