namespace EllipticBit.Coalescence.Shared.Request
{
	public interface ICoalescenceRequestFactory
	{
		ICoalescenceRequest CreateRequest();
		ICoalescenceRequest CreateRequest(string name);
	}
}
