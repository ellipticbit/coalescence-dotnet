using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EllipticBit.Coalescence.AspNetCore.Constraints
{
	public class UIntConstraint : IRouteConstraint
	{
		public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection) {
			if (values.TryGetValue(routeKey, out var value) && value != null)
			{
				return uint.TryParse(value.ToString(), out _);
			}
			return false;
		}
	}
}
