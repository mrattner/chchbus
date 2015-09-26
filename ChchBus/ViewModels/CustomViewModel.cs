using GalaSoft.MvvmLight;

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
		private string error = null;
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
	}
}
