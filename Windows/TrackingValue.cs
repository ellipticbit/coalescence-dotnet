using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace EllipticBit.Coalescence.Windows
{
	public interface ITrackingValue
	{
		bool IsKey { get; }
		bool IsCollection { get; }
		bool ValueChanged { get; }
		bool RemoteChanged { get; }
		string PropertyName { get; }

		void UpdateRemote(ITrackingValue value);
		void Reset();
	}

	public class TrackingValue<T> : ITrackingValue
	{
		private int _initialized = 0;

		public bool IsKey { get; } = false;
		public bool IsCollection { get; private protected set; } = false;
		protected Type ValueType { get; private protected set; }
		private protected bool IsValueTrackingObject {get; set; }
		public bool ValueChanged { get; internal set; } = false;
		public bool RemoteChanged { get; private protected set; }

		public string PropertyName { get; }
		protected T Original { get; private set; }

		private protected T _value;
		public T Value {
			get => _value;
			internal set {
				if (!EqualityComparer<T>.Default.Equals(_value, value)) {
					ValueChanged = true;
				}

				_value = value;

				if (Interlocked.CompareExchange(ref _initialized, 1, 0) == 0) {
					Original = value;
					ValueChanged = false;
				}
			}
		}

		public T Remote { get; internal set; }

		internal TrackingValue(string propertyName, T defaultValue, bool isKey = false) {
			this.ValueType = typeof(T);
			if (isKey) {
				this.IsKey = this.ValueType == typeof(ushort) || this.ValueType == typeof(uint) || this.ValueType == typeof(ulong) ||
							 this.ValueType == typeof(short) || this.ValueType == typeof(int) || this.ValueType == typeof(long) ||
							 this.ValueType == typeof(byte) || this.ValueType == typeof(sbyte) || this.ValueType == typeof(char) ||
							 this.ValueType == typeof(Guid) || this.ValueType == typeof(DateTime) || this.ValueType == typeof(DateTimeOffset) ||
							 this.ValueType == typeof(TimeSpan) || this.ValueType == typeof(string) || this.ValueType == typeof(byte[]);
			}

			this.IsValueTrackingObject = this.ValueType.IsSubclassOf(typeof(TrackingObject));
			this.PropertyName = propertyName;
			this.Original = defaultValue;
			this._value = defaultValue;
			this.Remote = default;
		}

		public virtual void UpdateRemote(ITrackingValue value) {
			if (value.PropertyName != PropertyName)
			{
				throw new ArgumentException(nameof(value), $"The property name of the remote value does not match the name for property: {PropertyName}");
			}

			if (value is not TrackingValue<T> tracking)
			{
				throw new ArgumentException(nameof(value), $"The specified tracking value does not match this property type for: {PropertyName}");
			}

			UpdateRemote(tracking.Value);
		}

		internal virtual void UpdateRemote(T value) {
			if (EqualityComparer<T>.Default.Equals(Value, value)) return;
			if (!ValueChanged) {
				Original = _value = value;
				ValueChanged = true;
			}
			else {
				Remote = value;
				RemoteChanged = true;
			}
		}

		public virtual void Reset() {
			_value = !RemoteChanged ? Original : Remote;
			Remote = default;
			RemoteChanged = false;
			ValueChanged = false;
		}
	}

	public sealed class TrackingCollection<T> : TrackingValue<ObservableCollection<T>>
	{
		internal TrackingCollection(string propertyName) : base(propertyName, new ObservableCollection<T>()) {
			this.ValueType = typeof(T);
			this.IsValueTrackingObject = this.ValueType.IsSubclassOf(typeof(TrackingObject));
			this.IsCollection = true;
		}

		public override void UpdateRemote(ITrackingValue value) {
			if (value.PropertyName != PropertyName) {
				throw new ArgumentException(nameof(value), $"The property name of the remote value does not match the name for property: {PropertyName}");
			}

			if (value is not TrackingCollection<T> tracking) {
				throw new ArgumentException(nameof(value), $"The specified tracking value does not match this property type for: {PropertyName}");
			}

			UpdateRemote(tracking.Value);
		}

		internal override void UpdateRemote(ObservableCollection<T> value) {
			if (IsValueTrackingObject) {
				var adds = value.OfType<ILocatableTrackingObject>().Where(a => !_value.OfType<ILocatableTrackingObject>().Select(b => b.ObjectTrackingKey).Contains(a.ObjectTrackingKey)).ToArray();
				foreach (var to in adds) {
					_value.Add((T)to);
				}

				var rems = _value.OfType<ILocatableTrackingObject>().Where(a => !value.OfType<ILocatableTrackingObject>().Select(b => b.ObjectTrackingKey).Contains(a.ObjectTrackingKey)).ToArray();
				foreach (var to in rems) {
					_value.Remove((T)to);
				}

				ValueChanged = (adds.Length + rems.Length) > 0;
				RemoteChanged = _value.OfType<TrackingObject>().Any(a => a.HasRemoteChanges);
			}
			else {
				Remote = value;
				RemoteChanged = true;
			}
		}

		public override void Reset() {
			var tr = Remote;
			base.Reset();
			_value = new ObservableCollection<T>(!RemoteChanged ? Original : tr);
			RemoteChanged = false;
		}
	}
}
