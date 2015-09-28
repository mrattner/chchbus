using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChchBus {
	/// <summary>
	/// Page for viewing list of saved stops.
	/// </summary>
	public sealed partial class FavouritesPage : Page {
		/// <summary>
		/// Constructor.
		/// </summary>
		public FavouritesPage () {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when a saved stop is clicked on.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void ListView_ItemClick (object sender, ItemClickEventArgs e) {
			var selected = e.ClickedItem as DataStorage.Favourite;
			if (selected == null) {
				return;
			}
			this.Frame.Navigate(typeof(NextBusesPage), selected.PlatformNo);
		}

		/// <summary>
		/// Invoked when a custom name is entered for a list item.
		/// </summary>
		/// <param name="sender">The name entry box</param>
		/// <param name="args">Event arguments</param>
		private void nameBox_QuerySubmitted (AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
			var vm = DataContext as Favourites;
			var platform = sender.DataContext as DataStorage.Favourite;
			vm.ChangeCustomName(platform.PlatformNo, sender.Text);
			this.editFlyout.Hide();
		}

		/// <summary>
		/// Invoked when a saved stop is deleted from the list.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void OnDelete_Click (object sender, RoutedEventArgs e) {
			var vm = DataContext as Favourites;
			var button = sender as Button;
			var toRemove = button.DataContext as DataStorage.Favourite;
			vm.RemoveSavedStop(toRemove.PlatformNo);
		}

		/// <summary>
		/// Clear the text box in the flyout.
		/// TODO: I want to bind the contents of the textbox to CustomName,
		/// but the textbox doesn't update itself after text has been
		/// entered into it. Therefore I have to force it to clear.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void editFlyout_Closed (object sender, object e) {
			this.nameBox.Text = string.Empty;
		}
	}
}
