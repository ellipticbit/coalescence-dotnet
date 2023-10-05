using System.Collections.Immutable;

namespace EllipticBit.Coalescence.Shared
{
	internal class CoalescenceServiceBuilder : ICoalescenceServiceBuilder, ICoalescenceOptionsRepository
	{
		private static ImmutableDictionary<string, CoalescenceOptionsBase> _ro = ImmutableDictionary<string, CoalescenceOptionsBase>.Empty;
		private static CoalescenceOptionsBase _dro = null;
		private static ImmutableDictionary<string, CoalescenceOptionsBase> _co = ImmutableDictionary<string, CoalescenceOptionsBase>.Empty;
		private static CoalescenceOptionsBase _dco = null;
		private static ImmutableDictionary<string, CoalescenceOptionsBase> _wso = ImmutableDictionary<string, CoalescenceOptionsBase>.Empty;
		private static CoalescenceOptionsBase _dwso = null;

		public ImmutableDictionary<string, CoalescenceOptionsBase> RequestOptions => _ro;
		public CoalescenceOptionsBase DefaultRequestOptions => _dro;
		public ImmutableDictionary<string, CoalescenceOptionsBase> ControllerOptions => _co;
		public CoalescenceOptionsBase DefaultControllerOptions => _dco;
		public ImmutableDictionary<string, CoalescenceOptionsBase> WebSocketOptions => _wso;
		public CoalescenceOptionsBase DefaultWebSocketOptions => _dwso;

		public ICoalescenceServiceBuilder AddCoalescenceRequestOptions(string name, CoalescenceOptionsBase options, bool isDefault = false) {
			_ro = _ro.Add(name, options);
			if (isDefault) _dro = options;
			else if (_dro == null) _dro = options;
			return this;
		}

		public ICoalescenceServiceBuilder AddCoalescenceControllerOptions(string name, CoalescenceOptionsBase options, bool isDefault = false) {
			_co = _co.Add(name, options);
			if (isDefault) _dco = options;
			else if (_dco == null) _dco = options;
			return this;
		}

		public ICoalescenceServiceBuilder AddCoalescenceWebSocketOptions(string name, CoalescenceOptionsBase options, bool isDefault = false) {
			_wso = _wso.Add(name, options);
			if (isDefault) _dwso = options;
			else if (_dwso == null) _dwso = options;
			return this;
		}
	}
}
