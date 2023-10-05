namespace EllipticBit.Coalescence.Request
{
	public interface ICoalescenceRequest
	{
		ICoalescenceRequestBuilder Get();
		ICoalescenceRequestBuilder Put();
		ICoalescenceRequestBuilder Post();
		ICoalescenceRequestBuilder Patch();
		ICoalescenceRequestBuilder Delete();
		ICoalescenceRequestBuilder Head();
	}
}
