using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ChchBus {
	/// <summary>
	/// Provides data to the Next Buses view.
	/// </summary>
	class NextBuses : CustomViewModel {
		/// <summary>
		/// Error message for nonexistent platform number
		/// </summary>
		private static string PLATFORM_NUMBER_NOT_FOUND = "That platform number doesn't exist.";

		/// <summary>
		/// Message for no buses coming within the next hour
		/// </summary>
		private static string NO_BUSES_COMING = "No buses are arriving at this platform within the next hour.";

		/// <summary>
		/// Data model
		/// </summary>
		private PlatformETAs model;

		/// <summary>
		/// The last time that data was fetched
		/// </summary>
		private DateTime lastUpdated;
		public DateTime LastUpdated {
			get {
				return this.lastUpdated;
			}
			set {
				this.lastUpdated = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Name of the platform whose ETA info is being fetched
		/// </summary>
		private string platformName;
		public string PlatformName {
			get {
				return this.platformName;
			}
			set {
				this.platformName = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// True when the platform-specific info should be shown
		/// (when not loading and there is no error).
		/// </summary>
		private bool showList = false;
		public bool ShowList {
			get {
				return this.showList;
			}
			set {
				this.showList = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// List of arrival times with bus routes
		/// </summary>
		private ObservableCollection<PlatformETAs.ETA> etas;
		public ObservableCollection<PlatformETAs.ETA> ETAs {
			get {
				return this.etas;
			}
			set {
				this.etas = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Constructor: Initialise the model.
		/// </summary>
		public NextBuses () {
			this.model = new PlatformETAs();
		}

		/// <summary>
		/// Displays the list of estimated arrival times at the given stop,
		/// sorted by arrival time.
		/// </summary>
		/// <param name="stopNumber">Bus platform number</param>
		public async void FetchETAs (int stopNumber) {
			try {
				this.Error = "";
				this.IsLoading = true;
				this.ShowList = false;
				Tuple<DateTime, string, List<PlatformETAs.ETA>> result =
					await this.model.GetETAs(stopNumber);

				if (result == null) {
					this.Error = PLATFORM_NUMBER_NOT_FOUND;
					return;
				}

				this.LastUpdated = result.Item1;
				this.PlatformName = result.Item2;
				List<PlatformETAs.ETA> etaList = result.Item3;
				etaList.Sort();
				this.ETAs = new ObservableCollection<PlatformETAs.ETA>(etaList);
				this.ShowList = true;
				if (this.ETAs.Count == 0) {
					this.Error = NO_BUSES_COMING;
				}
			} catch (Exception e) {
				this.Error = e.Message;
				this.showList = false;
			} finally {
				this.IsLoading = false;
			}
		}

		/// <summary>
		/// Fills the view model with fake data in the proper format.
		/// </summary>
		protected override void LoadDummyData () {
			base.LoadDummyData();
			this.LastUpdated = DateTime.Now;
			this.PlatformName = "Fake Rd near Dummy St";
			this.ETAs = new ObservableCollection<PlatformETAs.ETA>();
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "H",
				Destination = "Narnia via Coat Closet",
				Mins = 4
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "666",
				Destination = "The Nether via Obsidian Portal",
				Mins = 13
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "G",
				Destination = "St Mungos Hospital",
				Mins = 37
			});
		}
	}
}
