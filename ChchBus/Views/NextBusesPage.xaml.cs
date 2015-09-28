using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ChchBus {
	/// <summary>
	/// Page for seeing the next buses to arrive at a stop.
	/// </summary>
	public sealed partial class NextBusesPage : Page {
		/// <summary>
		/// Constructor.
		/// </summary>
		public NextBusesPage () {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is navigated to.
		/// </summary>
		/// <param name="e">Event arguments: Parameter should contain a platform number</param>
		protected override void OnNavigatedTo (NavigationEventArgs e) {
			base.OnNavigatedTo(e);
			var vm = DataContext as NextBuses;
			if (e.Parameter != null) {
				vm.FetchETAs((int)e.Parameter);
			}
		}

		/// <summary>
		/// Invoked when a stop is entered into the box.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void TextBox_TextChanged (object sender, TextChangedEventArgs e) {
			if (Regex.IsMatch(entryBox.Text, @"^\d{5}$")) {
				// This is a platform number
				var vm = DataContext as NextBuses;
				vm.FetchETAs(int.Parse(entryBox.Text));
			}
		}

		/// <summary>
		/// Invoked when the save button is clicked.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void save_Click (object sender, Windows.UI.Xaml.RoutedEventArgs e) {
			var vm = DataContext as NextBuses;
			vm.ToggleSaved();
		}
	}
}
