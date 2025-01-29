using System;
using System.Collections.Concurrent;
using System.Linq;

namespace EllipticBit.Coalescence.Windows
{
	internal class TrackingCache<T> where T : TrackingObject<T>
	{
		private static readonly ConcurrentDictionary<ulong, WeakReference<T>> objects = new();

		public T GetOrAdd(ulong key, T value) {
			var ol = objects.Values.ToArray();
			foreach (var tv in ol) {
				if (tv.TryGetTarget(out var target)) continue;

				objects.TryRemove(key, out var reference);
			}

			return objects.GetOrAdd(value.ObjectTrackingKey, new WeakReference<T>(value, false)).TryGetTarget(out T tmp) ? tmp : null;
		}

		public T Get(ulong key) {
			return objects.TryGetValue(key, out var value) ? value.TryGetTarget(out var target) ? target : null : null;
		}

		public T Remove(ulong key) {
			return objects.TryRemove(key, out var reference) ? reference.TryGetTarget(out var target) ? target : null : null;
		}
	}
}
