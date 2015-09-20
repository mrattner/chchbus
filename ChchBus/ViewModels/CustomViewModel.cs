using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ChchBus {
	/// <summary>
	/// Adds some extra functionality to the base ViewModel class.
	/// </summary>
	class CustomViewModel : ViewModelBase {
		/// <summary>
		/// True if data is currently being loaded
		/// </summary>
		private bool isLoading = false;
		public virtual bool IsLoading {
			get {
				return this.isLoading;
			}
			set {
				this.isLoading = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Error message
		/// </summary>
		private string error = "";
		public string Error {
			get {
				return this.error;
			}
			set {
				this.error = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Constructor: If in design mode, loads dummy data.
		/// </summary>
		public CustomViewModel () {
			if (this.IsInDesignMode) {
				LoadDummyData();
			}
		}

		/// <summary>
		/// Fills the view model with fake data in the proper format.
		/// </summary>
		/// <remarks>
		/// Will be overridden in child classes.
		/// </remarks>
		protected virtual void LoadDummyData () { }

		/// <summary>
		/// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
		/// <see cref="Visibility.Collapsed"/>.
		/// </summary>
		public sealed class BoolToVis : IValueConverter {
			public object Convert (object value, Type targetType, object parameter, string language) {
				return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
			}

			public object ConvertBack (object value, Type targetType, object parameter, string language) {
				return value is Visibility && (Visibility)value == Visibility.Visible;
			}
		}
	}
}
