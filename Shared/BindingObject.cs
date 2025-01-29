using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EllipticBit.Coalescence.Shared
{
	/// <summary>
	/// Provides support for WPF and WinForms Data-binding.
	/// </summary>
	public abstract class BindingObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private bool _hasChanges = false;
		public bool HasChanges { get => _hasChanges; }

		/// <summary>
		/// Sets the value of a field and ensures the appropriate events are invoked.
		/// </summary>
		/// <param name="field">Reference to the field being updated.</param>
		/// <param name="value">The new value to set.</param>
		/// <param name="propertyName">The name of the property being set.</param>
		/// <typeparam name="T">The type of the field being set.</typeparam>
		protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
			bool tbhc = _hasChanges;

			field = value;

			_hasChanges = true;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			if (!tbhc) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
		}
	}
}
