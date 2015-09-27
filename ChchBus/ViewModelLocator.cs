using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ChchBus {
	/// <summary>
	/// Central location for all of the ViewModels in the app.
	/// </summary>
	/// <remarks>
	/// A ViewModelLocator is a class that centralises the definitions of all the ViewModels 
	/// in an app so that they can be cached and retrieved on demand, usually via dependency 
	/// injection. See http://blog.falafel.com/using-the-mvvm-light-toolkit-with-windows-10/
	/// </remarks>
	class ViewModelLocator {
		/// <summary>
		/// Constructor: Sets up the Inversion of Control provider and
		/// registers each individual ViewModel.
		/// </summary>
		public ViewModelLocator () {
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register<NextBuses>();
			SimpleIoc.Default.Register<Favourites>();
		}

		/// <summary>
		/// Exposes the registered NextBuses view model.
		/// </summary>
		public NextBuses NextBusesViewModel {
			get {
				return ServiceLocator.Current.GetInstance<NextBuses>();
			}
		}

		/// <summary>
		/// Exposes the registered Favourites view model.
		/// </summary>
		public Favourites FavouritesViewModel {
			get {
				return ServiceLocator.Current.GetInstance<Favourites>();
			}
		}
	}
}
