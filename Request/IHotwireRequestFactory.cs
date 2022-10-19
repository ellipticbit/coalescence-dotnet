namespace EllipticBit.Hotwire.Request
{
	public  interface IHotwireRequestFactory
	{
		IHotwireRequest CreateRequest();
		IHotwireRequest CreateRequest(string name);
	}
}
