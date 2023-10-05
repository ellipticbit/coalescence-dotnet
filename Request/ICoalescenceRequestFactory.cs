namespace EllipticBit.Coalescence.Request
{
	public  interface ICoalescenceRequestFactory
	{
		ICoalescenceRequest CreateRequest();
		ICoalescenceRequest CreateRequest(string name);
	}
}
