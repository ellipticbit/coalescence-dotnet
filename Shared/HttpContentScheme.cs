namespace EllipticBit.Coalescence.Shared
{
#pragma warning disable CS1591
	public class HttpContentScheme
	{
		private readonly int _value;

		protected HttpContentScheme(int value) {
			this._value = value;
		}

		public static HttpContentScheme Content => new HttpContentScheme(0);
		public static HttpContentScheme Serialized => new HttpContentScheme(1);
		public static HttpContentScheme Text => new HttpContentScheme(2);
		public static HttpContentScheme Binary => new HttpContentScheme(3);
		public static HttpContentScheme Stream => new HttpContentScheme(4);
		public static HttpContentScheme FormUrlEncoded => new HttpContentScheme(5);
		public static HttpContentScheme Multipart => new HttpContentScheme(6);
		public static HttpContentScheme MultipartForm => new HttpContentScheme(7);

		public override bool Equals(object obj) {
			return (obj as HttpContentScheme)!._value == _value;
		}

		protected bool Equals(HttpContentScheme other) {
			return _value == other._value;
		}

		public override int GetHashCode() {
			return _value;
		}

		public static bool operator ==(HttpContentScheme a, HttpContentScheme b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}

			// Return true if the fields match:
			return a._value == b._value;
		}

		public static bool operator !=(HttpContentScheme a, HttpContentScheme b)
		{
			return !(a == b);
		}
	}
#pragma warning restore CS1591
}
