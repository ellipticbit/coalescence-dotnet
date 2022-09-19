namespace EllipticBit.Hotwire.Client
{
	public  interface IHotwireRequestFactory
	{
		IHotwireRequest CreateRequest();
		IHotwireRequest CreateRequest(string name);
	}
}
