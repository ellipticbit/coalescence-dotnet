using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Hashing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EllipticBit.Coalescence.Windows
{
	public interface ILocatableTrackingObject
	{
		ulong ObjectTrackingKey { get; }
	}

	public abstract class TrackingObjectBase : INotifyPropertyChanged, IJsonOnDeserialized
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[JsonIgnore]
		[IgnoreDataMember]
		private bool _hasChanges = false;
		[JsonIgnore]
		[IgnoreDataMember]
		public bool HasChanges => _hasChanges;

		[JsonIgnore]
		[IgnoreDataMember]
		private bool _hasTrackingChanges = false;
		[JsonIgnore]
		[IgnoreDataMember]
		public bool HasTrackingChanges => _hasTrackingChanges;

		[JsonIgnore]
		[IgnoreDataMember]
		private protected bool _hasRemoteChanges = false;
		[JsonIgnore]
		[IgnoreDataMember]
		public bool HasRemoteChanges => _hasRemoteChanges;

		private protected IDictionary<string, ITrackingValue> _properties = new Dictionary<string, ITrackingValue>();

		private TrackingValue<T> GetTrackingValue<T>(string propertyName)
		{
			if (!_properties.TryGetValue(propertyName, out ITrackingValue value))
			{
				throw new ArgumentException($"No property with the name '{propertyName}' exists.");
			}

			if (value is not TrackingValue<T> tv)
			{
				throw new ArgumentException($"Property '{propertyName}' is not of type '{typeof(T).FullName}'.");
			}

			return tv;
		}

		private TrackingCollection<T> GetTrackingCollection<T>(string propertyName)
		{
			if (!_properties.TryGetValue(propertyName, out ITrackingValue value))
			{
				throw new ArgumentException($"No property with the name '{propertyName}' exists.");
			}

			if (value is not TrackingCollection<T> tv)
			{
				throw new ArgumentException($"Property '{propertyName}' is not of type '{typeof(T).FullName}'.");
			}

			return tv;
		}

		private void TrackingObjectChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == nameof(HasTrackingChanges))
			{
				_hasTrackingChanges = true;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasTrackingChanges)));
			}

			if (args.PropertyName == nameof(HasRemoteChanges))
			{
				_hasRemoteChanges = true;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasRemoteChanges)));
			}
		}

		protected TrackingValue<T> RegisterProperty<T>(string propertyName, bool isKey = false, T defaultValue = default) {
			var t = new TrackingValue<T>(propertyName, defaultValue, isKey);

			try {
				_properties.Add(propertyName, t);
			} catch {
				throw new ArgumentException($"A property with the name '{propertyName}' already exists.");
			}

			return t;
		}

		protected TrackingCollection<T> RegisterCollectionProperty<T>(string propertyName) {
			var t = new TrackingCollection<T>(propertyName);

			try {
				_properties.Add(propertyName, t);
			} catch {
				throw new ArgumentException($"A property with the name '{propertyName}' already exists.");
			}

			return t;
		}

		protected virtual void RegistrationCompleted() {
			var td = _properties;
			_properties = td.ToFrozenDictionary();
		}

		public T GetValue<T>(string propertyName) => GetTrackingValue<T>(propertyName).Value;
		public T GetRemoteValue<T>(string propertyName) => GetTrackingValue<T>(propertyName).Remote;

		// ReSharper disable once MemberCanBePrivate.Global
		protected void SetValue<T>(TrackingValue<T> trackingValue, T value) {
			if (trackingValue == null) {
				throw new ArgumentNullException(nameof(trackingValue), $"The property requested has not been registered with the RegisterProperty method.");
			}

			if (!Application.Current?.Dispatcher.CheckAccess() ?? false) {
				throw new InvalidOperationException($"The property '{trackingValue.PropertyName}' can only be changed from the UI thread.");
			}

			if (value is TrackingObjectBase to) {
				to.PropertyChanged += TrackingObjectChanged;
			}

			if (trackingValue.Value is TrackingObjectBase co) {
				co.PropertyChanged -= TrackingObjectChanged;
			}

			trackingValue.Value = value;

			if (trackingValue.IsKey) RehashKey();

			_hasChanges = true;
			_hasTrackingChanges = true;

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(trackingValue.PropertyName));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasTrackingChanges)));
		}

		public Task SetValue<T>(string propertyName, T value) {
			var tv = GetTrackingValue<T>(propertyName);

			if (Application.Current?.Dispatcher.CheckAccess() ?? true) {
				SetValue(tv, value);
				return Task.CompletedTask;
			}

			return Application.Current?.Dispatcher.InvokeAsync(() => SetValue(tv, value), DispatcherPriority.DataBind).Task ?? Task.CompletedTask;
		}

		// ReSharper disable once MemberCanBePrivate.Global
		protected void SetCollection<T>(TrackingCollection<T> trackingCollection, IEnumerable<T> values) {
			if (!Application.Current?.Dispatcher.CheckAccess() ?? false) {
				throw new InvalidOperationException($"The property '{trackingCollection.PropertyName}' can only be changed from the UI thread.");
			}

			if (values != null) {
				IEnumerable<T> vl = values.ToList();
				foreach (var to in vl.OfType<TrackingObjectBase>()) {
					to.PropertyChanged += TrackingObjectChanged;
				}

				foreach (var to in trackingCollection.Value.OfType<TrackingObjectBase>()) {
					to.PropertyChanged -= TrackingObjectChanged;
				}

				trackingCollection.Value = new ObservableCollection<T>(vl);
			}
			else {
				trackingCollection.Value = null;
			}

			_hasChanges = true;
			_hasTrackingChanges = true;

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(trackingCollection.PropertyName));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasTrackingChanges)));
		}

		public Task SetCollection<T>(string propertyName, IEnumerable<T> values) {
			var tc = GetTrackingCollection<T>(propertyName);

			if (Application.Current?.Dispatcher.CheckAccess() ?? true)
			{
				SetCollection(tc, values);
				return Task.CompletedTask;
			}

			return Application.Current.Dispatcher.InvokeAsync(() => SetCollection(tc, values), DispatcherPriority.DataBind).Task ?? Task.CompletedTask;
		}

		public Task SetRemoteValue<T>(string propertyName, T value) {
			if (value is IEnumerable) {
				throw new ArgumentException("This method cannot be used on values implementing 'IEnumerable'. Use SetRemoteCollection instead.");
			}

			var tv = GetTrackingValue<T>(propertyName);

			return Application.Current?.Dispatcher.InvokeAsync(() => {
				tv.UpdateRemote(value);

				if (tv.ValueChanged) {
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
					tv.ValueChanged = false; //Used to reset the ValueChanged property.
				}
				else {
					_hasRemoteChanges = true;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasRemoteChanges)));
				}
			}, DispatcherPriority.DataBind).Task ?? Task.CompletedTask;
		}

		public Task SetRemoteCollection<T>(string propertyName, IEnumerable<T> values) {
			var tv = GetTrackingValue<ObservableCollection<T>>(propertyName);

			return Application.Current?.Dispatcher.InvokeAsync(() => {
				tv.Remote = new ObservableCollection<T>(values);
				_hasRemoteChanges = true;

				if (!tv.ValueChanged) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasRemoteChanges)));
			}, DispatcherPriority.DataBind).Task ?? Task.CompletedTask;
		}

		private void ResetInternal() {
			foreach (var tv in _properties.Values) {
				tv.Reset();
			}

			_hasChanges = false;
			_hasTrackingChanges = false;
			_hasRemoteChanges = false;

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasTrackingChanges)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasRemoteChanges)));
		}

		public Task Reset() {
			if (Application.Current?.Dispatcher.CheckAccess() ?? true) {
				ResetInternal();
				return Task.CompletedTask;
			}

			return Application.Current?.Dispatcher.InvokeAsync(ResetInternal, DispatcherPriority.DataBind).Task ?? Task.CompletedTask;
		}

		private void ResetInternal(string propertyName) {
			if (!_properties.TryGetValue(propertyName, out ITrackingValue value)) {
				throw new ArgumentException($"No property with the name '{propertyName}' exists.");
			}

			value.Reset();
		}

		public Task Reset(string propertyName) {
			if (Application.Current?.Dispatcher.CheckAccess() ?? true)
			{
				ResetInternal(propertyName);
				return Task.CompletedTask;
			}


			return Application.Current?.Dispatcher.InvokeAsync(() => ResetInternal(propertyName), DispatcherPriority.DataBind).Task ?? Task.CompletedTask;
		}

		public IEnumerable<ITrackingValue> GetChangedValues() {
			return _properties.Values.Where(a => a.ValueChanged);
		}

		private protected abstract void RehashKey();

		private protected void OnDeserializedBase() {
			_hasChanges = false;
			_hasTrackingChanges = false;
			_hasRemoteChanges = false;
		}

		private protected void OnPropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		[OnDeserialized]
		void IJsonOnDeserialized.OnDeserialized() {
			OnDeserializedBase();
		}
	}

	public abstract class TrackingObject : TrackingObjectBase
	{
		private protected override void RehashKey() { }

		protected sealed override void RegistrationCompleted() {
			base.RegistrationCompleted();
		}
	}

	public abstract class TrackingObject<T> : TrackingObjectBase, IJsonOnDeserialized, ILocatableTrackingObject
		where T : TrackingObject<T>
	{
		private static readonly TrackingCache<T> TrackedObjects = new();

		public static T GetTrackingObject(ulong hashKey) {
			return TrackedObjects.Get(hashKey);
		}

		private static T RegisterTrackingObject(T register, bool applyRemoteValues = true) {
			var obj = TrackedObjects.GetOrAdd(register._keyHash, register);

			if (applyRemoteValues && obj != null && !ReferenceEquals(register, obj)) {
				if (Application.Current?.Dispatcher.CheckAccess() ?? true) {
					foreach (var tv in register._properties.Values.Where(a => !a.IsKey)) {
						if (obj._properties.TryGetValue(tv.PropertyName, out ITrackingValue value)) {
							value.UpdateRemote(tv);
						}
					}
				}
				else {
					Application.Current?.Dispatcher.Invoke(() => {
						foreach (var tv in register._properties.Values.Where(a => !a.IsKey)) {
							if (obj._properties.TryGetValue(tv.PropertyName, out ITrackingValue value)) {
								value.UpdateRemote(tv);
							}
						}
					}, DispatcherPriority.Send);
				}

				obj._hasRemoteChanges = obj._properties.Values.Any(a => a.RemoteChanged);
				if (obj._hasRemoteChanges) obj.OnPropertyChanged(nameof(HasRemoteChanges));
			}

			return obj;
		}

		[JsonIgnore]
		[IgnoreDataMember]
		private ulong _keyHash = 0;
		[JsonIgnore]
		[IgnoreDataMember]
		public ulong ObjectTrackingKey => _keyHash;

		[JsonIgnore]
		[IgnoreDataMember]
		public bool IsRegistered {get; private set; }

		public T RegisterTrackingObject(bool applyRemoteValues = true) {
			var temp = RegisterTrackingObject((T)this, applyRemoteValues);

			IsRegistered = ReferenceEquals(this, temp);

			return temp;
		}

		public T UnregisterTrackingObject() {
			return TrackedObjects.Remove(this._keyHash);
		}

		protected sealed override void RegistrationCompleted() {
			if (_properties.Values.All(a => !a.IsKey)) {
				throw new InvalidOperationException("No key values registered.");
			}

			base.RegistrationCompleted();
		}

		[OnDeserialized]
		void IJsonOnDeserialized.OnDeserialized() {
			RegisterTrackingObject();

			base.OnDeserializedBase();
		}

		private protected override void RehashKey() {
			var kl = _properties.Values.Where(a => a.IsKey);
			var bytes = new List<byte>(256);

			foreach (var key in kl)
			{
				switch (key)
				{
					case TrackingValue<ushort> kvus:
						bytes.AddRange(BitConverter.GetBytes(kvus.Value));
						break;
					case TrackingValue<uint> kvui:
						bytes.AddRange(BitConverter.GetBytes(kvui.Value));
						break;
					case TrackingValue<ulong> kvul:
						bytes.AddRange(BitConverter.GetBytes(kvul.Value));
						break;
					case TrackingValue<short> kvss:
						bytes.AddRange(BitConverter.GetBytes(kvss.Value));
						break;
					case TrackingValue<int> kvsi:
						bytes.AddRange(BitConverter.GetBytes(kvsi.Value));
						break;
					case TrackingValue<long> kvsl:
						bytes.AddRange(BitConverter.GetBytes(kvsl.Value));
						break;
					case TrackingValue<byte> kvb:
						bytes.Add(kvb.Value);
						break;
					case TrackingValue<sbyte> kvsb:
						bytes.Add((byte)kvsb.Value);
						break;
					case TrackingValue<char> kvch:
						bytes.Add((byte)kvch.Value);
						break;
					case TrackingValue<Guid> kvg:
						bytes.AddRange(kvg.Value.ToByteArray());
						break;
					case TrackingValue<DateTime> kvdt:
						bytes.AddRange(Encoding.UTF8.GetBytes(kvdt.Value.ToString("O")));
						break;
					case TrackingValue<DateTimeOffset> kvdto:
						bytes.AddRange(Encoding.UTF8.GetBytes(kvdto.Value.ToString("O")));
						break;
					case TrackingValue<TimeSpan> kvdts:
						bytes.AddRange(Encoding.UTF8.GetBytes(kvdts.Value.ToString()));
						break;
					case TrackingValue<string> kvs:
						if (kvs.Value == null) break;
						bytes.AddRange(Encoding.UTF8.GetBytes(kvs.Value));
						break;
					case TrackingValue<byte[]> kvbl:
						if (kvbl.Value == null) break;
						bytes.AddRange(kvbl.Value);
						break;
				}
			}

			_keyHash = XxHash64.HashToUInt64(bytes.ToArray());
		}
	}
}
