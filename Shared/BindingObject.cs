using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EllipticBit.Coalescence.Shared
{
	public abstract class BindingObject : INotifyPropertyChanged, INotifyPropertyChanging
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;

		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
	}
}
