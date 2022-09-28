using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticBit.Hotwire.Shared
{
	public interface IHotwireServiceBuilder
	{
		IHotwireServiceBuilder AddSerializer<T>(bool defaultSerializer = false) where T : class, IHotwireSerializer;
		IHotwireServiceBuilder AddAuthentication<T>(bool defaultAuthentication = false) where T : class, IHotwireAuthentication;
	}
}
