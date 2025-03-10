namespace EllipticBit.Coalescence.Shared.Request
{
	public interface ICoalescenceRequestFactory
	{
		ICoalescenceRequest CreateRequest(string name = null, string tenantId = null);
	}
}
