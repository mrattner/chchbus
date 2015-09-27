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
		/// Invoked when a saved stop is deleted from the list.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void OnDelete_Click (object sender, RoutedEventArgs e) {
			var vm = DataContext as Favourites;
			var toRemove = sender as Button;
			vm.RemoveSavedStop((int)toRemove.Tag);
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
	}
}
