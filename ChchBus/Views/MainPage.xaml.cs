using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace ChchBus {
	/// <summary>
	/// The framing page consists of navigation + content frame.
	/// </summary>
	public sealed partial class MainPage : Page {
		/// <summary>
		/// Constructor.
		/// </summary>
		public MainPage () {
			this.InitializeComponent();

			// Select a starting page
			this.mainFrame.Navigate(typeof(FavouritesPage));
		}

		/// <summary>
		/// Invoked when a navigation button is clicked.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void NavButton_Click (object sender, RoutedEventArgs e) {
			var selected = sender as ToggleButton;
			if (selected == null) {
				return;
			} else if (selected.Equals(this.saved)) {
				this.mainFrame.Navigate(typeof(FavouritesPage));
			} else if (selected.Equals(this.nextBuses)) {
				this.mainFrame.Navigate(typeof(NextBusesPage));
			} else if (selected.Equals(this.about)) {
				this.mainFrame.Navigate(typeof(AboutPage));
			}
			// Triggers the "Checked" event to uncheck the others, and
			// overrides the normal ToggleButton behaviour
			selected.IsChecked = true;
		}

		/// <summary>
		/// Invoked when a navigation toggle button is selected: uncheck the
		/// other toggle buttons.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void NavButton_Checked (object sender, RoutedEventArgs e) {
			var selected = sender as ToggleButton;
			if (selected == null) {
				return;
			} else if (selected.Equals(this.saved)) {
				this.nextBuses.IsChecked = false;
				this.about.IsChecked = false;
			} else if (selected.Equals(this.nextBuses)) {
				this.saved.IsChecked = false;
				this.about.IsChecked = false;
			} else if (selected.Equals(this.about)) {
				this.saved.IsChecked = false;
				this.nextBuses.IsChecked = false;
			}
		}

		/// <summary>
		/// Selects the appropriate nav button when a page is navigated to.
		/// Triggers the "Checked" event to uncheck the others, and overrides
		/// the normal ToggleButton behaviour.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void mainFrame_Navigated (object sender, NavigationEventArgs e) {
			if (e.Content is FavouritesPage) {
				this.saved.IsChecked = true;
			} else if (e.Content is NextBusesPage) {
				this.nextBuses.IsChecked = true;
			} else if (e.Content is AboutPage) {
				this.about.IsChecked = true;
			}
		}
	}
}
