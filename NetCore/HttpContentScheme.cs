namespace EllipticBit.Lexicon.Client
{
	public class HttpContentScheme
	{
		private readonly int _value;

		protected HttpContentScheme(int value) {
			this._value = value;
		}

		public static HttpContentScheme Json => new HttpContentScheme(1);
		public static HttpContentScheme Xml => new HttpContentScheme(2);
		public static HttpContentScheme Text => new HttpContentScheme(3);
		public static HttpContentScheme Binary => new HttpContentScheme(4);
		public static HttpContentScheme Stream => new HttpContentScheme(5);
		public static HttpContentScheme FormUrlEncoded => new HttpContentScheme(6);
		public static HttpContentScheme Mutltipart => new HttpContentScheme(7);
		public static HttpContentScheme MultipartForm => new HttpContentScheme(8);

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

		public override string ToString() {
			switch (_value) {
				case 1: return "application/json";
				case 2: return "application/xml";
				case 3: return "text/plain";
				case 4: return "application/octet-stream";
				case 5: return "application/octet-stream";
				case 6: return "application/x-www-form-urlencoded";
				case 7: return "multipart/*";
				case 8: return "multipart/form-data";
				default: return "application/octet-stream";
			}
		}
	}
}
