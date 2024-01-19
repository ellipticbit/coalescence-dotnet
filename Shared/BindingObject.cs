using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Provides support for WPF and WinForms Data-binding.
	/// </summary>
	public abstract class BindingObject : INotifyPropertyChanged, INotifyPropertyChanging
	{
		public event PropertyChangingEventHandler PropertyChanging;
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Sets the value of a field and ensures the appropriate events are invoked.
		/// </summary>
		/// <param name="field">Reference to the field being updated.</param>
		/// <param name="value">The new value to set.</param>
		/// <param name="propertyName">The name of the property being set.</param>
		/// <typeparam name="T">The type of the field being set.</typeparam>
		protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
