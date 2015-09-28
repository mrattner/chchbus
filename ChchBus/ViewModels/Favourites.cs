﻿using System.Collections.ObjectModel;
using System.Linq;

namespace ChchBus {
	/// <summary>
	/// Provides data to the Favourites view.
	/// </summary>
	class Favourites : CustomViewModel {
		/// <summary>
		/// Data model
		/// </summary>
		private DataStorage model;

		/// <summary>
		/// List of saved bus stops
		/// </summary>
		private ObservableCollection<DataStorage.Favourite> faves;
		public ObservableCollection<DataStorage.Favourite> Faves {
			get {
				return this.faves;
			}
			set {
				this.faves = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Constructor: Initialise the model.
		/// </summary>
		public Favourites () {
			this.model = new DataStorage();
			var savedStops = this.model.GetAllFavourites();
			this.Faves = new ObservableCollection<DataStorage.Favourite>(savedStops);
		}

		/// <summary>
		/// Adds a bus stop to the list of saved platforms.
		/// </summary>
		/// <param name="stopNumber">Platform number to add</param>
		/// <param name="platformName">Official name of platform</param>
		public void AddSavedStop (int stopNumber, string platformName) {
			var newFave = new DataStorage.Favourite() {
				PlatformNo = stopNumber,
				StopName = platformName,
				CustomName = null
			};
			this.model.AddFavourite(newFave);
			this.Faves.Add(newFave);
		}

		/// <summary>
		/// Removes a bus stop from the list of saved platforms.
		/// </summary>
		/// <param name="stopNumber">Platform number to remove</param>
		public void RemoveSavedStop (int stopNumber) {
			this.model.RemoveFavourite(stopNumber);
			// Have to do this the old-fashioned way because ObservableCollection
			// has no equivalent of "remove where..." method
			for (int i = 0; i < this.Faves.Count; i++) {
				var fave = this.Faves.ElementAt(i);
				if (fave.PlatformNo == stopNumber) {
					this.Faves.RemoveAt(i);
					return;
				}
			}
		}

		/// <summary>
		/// Fills the view model with fake data in the proper format.
		/// </summary>
		protected override void LoadDummyData () {
			base.LoadDummyData();
			this.Faves = new ObservableCollection<DataStorage.Favourite>();
			this.Faves.Add(new DataStorage.Favourite() {
				PlatformNo = 17932,
				StopName = "Pumpkin Ave near Gourd St",
				CustomName = "Home EB"
			});
			this.Faves.Add(new DataStorage.Favourite() {
				PlatformNo = 38197,
				StopName = "Pumpkin Ave near Gourd St",
				CustomName = "Home WB"
			});
			this.Faves.Add(new DataStorage.Favourite() {
				PlatformNo = 50627,
				StopName = "Duanbei International Airport",
				CustomName = null
			});
			this.Faves.Add(new DataStorage.Favourite() {
				PlatformNo = 51135,
				StopName = "Spooky Way near Halloween Ct",
				CustomName = "Eileen and Percy's house"
			});
			this.Faves.Add(new DataStorage.Favourite() {
				PlatformNo = 24402,
				StopName = "Bacon Terrace near Spam St",
				CustomName = "North side of Noble Park"
			});
		}
	}
}