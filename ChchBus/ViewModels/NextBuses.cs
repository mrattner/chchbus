using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ChchBus {
	/// <summary>
	/// Provides data to the Next Buses view.
	/// </summary>
	class NextBuses : CustomViewModel {
		/// <summary>
		/// Error message for nonexistent platform number
		/// </summary>
		private static readonly string PLATFORM_NUMBER_NOT_FOUND = "That platform number doesn't exist.";

		/// <summary>
		/// Message for no buses coming within the next hour
		/// </summary>
		private static readonly string NO_BUSES_COMING = "No buses are arriving at this platform within the next hour.";

		/// <summary>
		/// Seconds between refreshes
		/// </summary>
		private static readonly int REFRESH_INTERVAL = 10;

		/// <summary>
		/// Used for signalling the refresh task to cancel itself
		/// </summary>
		private CancellationTokenSource cancel;

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
		/// Platform whose ETA info is currently being fetched
		/// </summary>
		private int platformNo;
		public int PlatformNo {
			get {
				return this.platformNo;
			}
			set {
				this.platformNo = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// Name of platform whose ETA info is currently being fetched
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
		/// Constructor: Initialise the model and cancellation token source.
		/// </summary>
		public NextBuses () {
			this.model = new PlatformETAs();
			this.cancel = new CancellationTokenSource();
		}

		/// <summary>
		/// Gets the list of estimated arrival times at the given stop,
		/// sorted by arrival time.
		/// </summary>
		/// <param name="stopNumber">Bus platform number</param>
		public async void FetchETAs (int stopNumber) {
			this.IsLoading = true;
			this.ShowList = false;
			this.Error = null;

			// Cancel and throw away the previous refresh task
			this.cancel.Cancel();
			this.cancel.Dispose();

			// Start a new refresh task
			this.cancel = new CancellationTokenSource();
			CancellationToken ct = this.cancel.Token;

			while (true) {
				try {
					ct.ThrowIfCancellationRequested();
					Tuple<DateTime, string, List<PlatformETAs.ETA>> result =
						await this.model.GetETAs(stopNumber);
					if (result == null) {
						this.Error = PLATFORM_NUMBER_NOT_FOUND;
						this.ShowList = false;
						this.IsLoading = false;
						return;
					}

					this.LastUpdated = result.Item1;
					this.PlatformName = result.Item2;
					this.PlatformNo = stopNumber;
					List<PlatformETAs.ETA> etaList = result.Item3;
					etaList.Sort();
					this.ETAs = new ObservableCollection<PlatformETAs.ETA>(etaList);

					if (this.ETAs.Count == 0) {
						this.Error = NO_BUSES_COMING;
					}

					// Fetch was successful
					this.ShowList = true;
					this.IsLoading = false;

					// Wait before refreshing
					await Task.Delay(TimeSpan.FromSeconds(REFRESH_INTERVAL));
				} catch (OperationCanceledException) {
					// Stop refreshing but show no error message
					return;
				} catch (Exception e) {
					this.Error = e.Message;
					this.ShowList = false;
					this.IsLoading = false;
					return;
				}
			}
		}

		/// <summary>
		/// Fills the view model with fake data in the proper format.
		/// </summary>
		protected override void LoadDummyData () {
			base.LoadDummyData();
			this.LastUpdated = DateTime.Now;
			this.PlatformName = "Fake Rd near Dummy St";
			this.PlatformNo = 12345;
			this.ETAs = new ObservableCollection<PlatformETAs.ETA>();
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "H",
				Destination = "Narnia via Coat Closet",
				Mins = 4
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "666",
				Destination = "The Nether via Obsidian Portal",
				Mins = 4
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "G",
				Destination = "St Mungo's Hospital",
				Mins = 5
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "O",
				Destination = "Marmalade Forest",
				Mins = 9
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "72",
				Destination = "Minas Tirith",
				Mins = 10
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "666",
				Destination = "The Nether via Obsidian Portal",
				Mins = 13
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "50",
				Destination = "Old MacDonald's Farm",
				Mins = 15
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "R",
				Destination = "Yammeth Cretch",
				Mins = 15
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "B",
				Destination = "Museum",
				Mins = 17
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "F",
				Destination = "Serenity Valley",
				Mins = 20
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "O",
				Destination = "Marmalade Forest",
				Mins = 25
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "666",
				Destination = "The Nether via Obsidian Portal",
				Mins = 30
			});
			this.ETAs.Add(new PlatformETAs.ETA() {
				RouteNo = "G",
				Destination = "St Mungo's Hospital",
				Mins = 37
			});
		}
	}
}
