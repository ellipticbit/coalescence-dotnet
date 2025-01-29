using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using EllipticBit.Coalescence.Windows;

using Newtonsoft.Json.Linq;

namespace UnitTests
{
	[TestClass]
	public class Tracking
	{
		[TestMethod]
		[TestInitialize]
		public void TestTrackingInitialization() {
			var tt = new Tracking1();
			Assert.IsNotNull(tt);

			tt.Tracking = 1000;
			Assert.AreEqual(1000, tt.Tracking);

			tt.Nullable = null;
			Assert.IsNull(tt.Nullable);

			tt.SubTracking = new Tracking2();
			Assert.IsNotNull(tt.SubTracking, "SubTracking is null.");

			tt.Collection = new ObservableCollection<Tracking2>([new Tracking2() { Test = "Testing List 1" }, new Tracking2() { Test = "Testing List 2" }]);
			Assert.IsNotNull(tt.Collection, "Collection is null.");
			Assert.AreEqual(2, tt.Collection.Count);

			tt.IntCollection = new ObservableCollection<int>([1, 2, 3]);
			Assert.IsNotNull(tt.IntCollection, "IntCollection is null.");
			Assert.AreEqual(3, tt.IntCollection.Count);

			Assert.AreNotEqual(0UL, tt.ObjectTrackingKey, "Tracking Hash Key not set.");
			Assert.AreEqual(true, tt.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(true, tt.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
			Assert.AreEqual(false, tt.HasRemoteChanges, "HasRemoteChanges incorrectly set.");

			tt.Reset();
			tt.RegisterTrackingObject(false);
		}

		[TestMethod]
		public void TestBasicTracking() {
			var test = Tracking1.GetTrackingObject(new HashKeyHelper().AddKey(1000).HashKey);

			test.Nullable = 1000;
			Assert.AreEqual(true, test.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(true, test.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
			test.Reset();
			Assert.IsNull(test.Nullable);

			test.SubTracking.Test = "SubTracking Set";
			Assert.AreEqual(false, test.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(true, test.SubTracking.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(true, test.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
			test.Reset();

			test.Collection[0].Test = "Collection Track-through test.";
			Assert.AreEqual(false, test.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(true, test.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
			test.Reset();

			//None tracking object collections should not trigger the Changes properties.
			test.IntCollection[0] = 10;
			Assert.AreEqual(false, test.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(false, test.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
			test.Reset();
			Assert.AreEqual(1, test.IntCollection[0]);

			//We cannot track adds/removes to the ObservableCollection due to the INotifyCollectionChanged being a delegate and not an event.
			test.Collection.Add(new Tracking2() {Test = "Add Test"});

			Assert.AreEqual(false, test.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(false, test.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
			test.Reset();
		}

		[TestMethod]
		public void TestRemoteTracking() {
			var test = Tracking1.GetTrackingObject(new HashKeyHelper().AddKey(1000).HashKey);

			var json = System.Text.Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(test));

			var second = JsonSerializer.Deserialize<Tracking1>("{\"Tracking\":1000,\"Nullable\":null,\"SubTracking\":{\"Test\":null},\"Collection\":[{\"Test\":\"Testing List 3\"},{\"Test\":\"Testing List 4\"},{\"Test\":\"Testing List 2\"}],\"IntCollection\":[1,2,3,4]}");

			Assert.AreEqual(3, test.Collection.Count);
			Assert.AreEqual(false, test.HasChanges, "HasChanges incorrectly set.");
			Assert.AreEqual(true, test.HasRemoteChanges, "HasRemoteChanges incorrectly set.");
			Assert.AreEqual(false, test.HasTrackingChanges, "HasTrackingChanges incorrectly set.");
		}
	}

	public class Tracking1 : TrackingObject<Tracking1>
	{
		public Tracking1() {
			_tracking = RegisterProperty<int>(nameof(Tracking), true);
			_nullable = RegisterProperty<int?>(nameof(Nullable));
			_subTracking = RegisterProperty<Tracking2>(nameof(SubTracking));
			_collection = RegisterCollectionProperty<Tracking2>(nameof(Collection));
			_IntCollection = RegisterCollectionProperty<int>(nameof(IntCollection));
			RegistrationCompleted();
		}

		private readonly TrackingValue<int> _tracking;
		public int Tracking { get => _tracking.Value; set => SetValue(_tracking, value); }

		private readonly TrackingValue<int?> _nullable;
		public int? Nullable { get => _nullable.Value; set => SetValue(_nullable, value); }

		private readonly TrackingValue<Tracking2> _subTracking;
		public Tracking2 SubTracking { get => _subTracking.Value; set => SetValue(_subTracking, value); }

		private readonly TrackingCollection<Tracking2> _collection;
		public ObservableCollection<Tracking2> Collection { get => _collection.Value; set => SetCollection(_collection, value); }

		private readonly TrackingCollection<int> _IntCollection;
		public ObservableCollection<int> IntCollection { get => _IntCollection.Value; set => SetCollection(_IntCollection, value); }
	}

	public class Tracking2 : TrackingObject<Tracking2>
	{
		public Tracking2() {
			_test = RegisterProperty<string>(nameof(Test), true);
			RegistrationCompleted();
		}

		private readonly TrackingValue<string> _test;
		public string Test { get => _test.Value; set => SetValue(_test, value); }
	}
}
