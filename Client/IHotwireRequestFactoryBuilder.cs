namespace EllipticBit.Hotwire.Client
{
	public interface IHotwireRequestFactoryBuilder
	{
		IHotwireRequestFactoryBuilder AddHotwireRequestFactory(string name, HotwireRequestOptions options);
	}
}
