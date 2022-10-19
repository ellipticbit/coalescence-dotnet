using System.Collections.Immutable;

namespace EllipticBit.Hotwire.Shared
{
	internal class HotwireServiceBuilder : IHotwireServiceBuilder, IHotwireOptionsRepository
	{
		private static ImmutableDictionary<string, HotwireOptionsBase> _ro = ImmutableDictionary<string, HotwireOptionsBase>.Empty;
		private static HotwireOptionsBase _dro = null;
		private static ImmutableDictionary<string, HotwireOptionsBase> _co = ImmutableDictionary<string, HotwireOptionsBase>.Empty;
		private static HotwireOptionsBase _dco = null;
		private static ImmutableDictionary<string, HotwireOptionsBase> _wso = ImmutableDictionary<string, HotwireOptionsBase>.Empty;
		private static HotwireOptionsBase _dwso = null;

		public ImmutableDictionary<string, HotwireOptionsBase> RequestOptions => _ro;
		public HotwireOptionsBase DefaultRequestOptions => _dro;
		public ImmutableDictionary<string, HotwireOptionsBase> ControllerOptions => _co;
		public HotwireOptionsBase DefaultControllerOptions => _dco;
		public ImmutableDictionary<string, HotwireOptionsBase> WebSocketOptions => _wso;
		public HotwireOptionsBase DefaultWebSocketOptions => _dwso;

		public IHotwireServiceBuilder AddHotwireRequestOptions(string name, HotwireOptionsBase options, bool isDefault = false) {
			_ro = _ro.Add(name, options);
			if (isDefault) _dro = options;
			else if (_dro == null) _dro = options;
			return this;
		}

		public IHotwireServiceBuilder AddHotwireControllerOptions(string name, HotwireOptionsBase options, bool isDefault = false) {
			_co = _co.Add(name, options);
			if (isDefault) _dco = options;
			else if (_dco == null) _dco = options;
			return this;
		}

		public IHotwireServiceBuilder AddHotwireWebSocketOptions(string name, HotwireOptionsBase options, bool isDefault = false) {
			_wso = _wso.Add(name, options);
			if (isDefault) _dwso = options;
			else if (_dwso == null) _dwso = options;
			return this;
		}
	}
}
