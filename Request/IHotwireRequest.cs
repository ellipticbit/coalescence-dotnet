namespace EllipticBit.Hotwire.Request
{
	public interface IHotwireRequest
	{
		IHotwireRequestBuilder Get();
		IHotwireRequestBuilder Put();
		IHotwireRequestBuilder Post();
		IHotwireRequestBuilder Patch();
		IHotwireRequestBuilder Delete();
		IHotwireRequestBuilder Head();
	}
}
