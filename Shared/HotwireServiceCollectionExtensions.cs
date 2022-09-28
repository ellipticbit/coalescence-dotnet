using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticBit.Hotwire.Shared
{
	public static class HotwireServiceCollectionExtensions
	{
		public static IHotwireServiceBuilder AddHotwireServices(this IServiceCollection service)
		{
			service.AddTransient<IHotwireSerializer, HotwireJsonSerializer>();
			service.AddTransient<IHotwireSerializer, HotwireXmlSerializer>();
			return new HotwireServiceBuilder(service);
		}
	}
}
