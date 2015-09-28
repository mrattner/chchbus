using System.Collections.Generic;
using Windows.Storage;

namespace ChchBus {
	/// <summary>
	/// Stores and retrieves information from local app data.
	/// </summary>
	class DataStorage {
		// Keys for the data objects
		private static readonly string STOP_NAME = "stopName";
		private static readonly string CUSTOM_NAME = "customName";

		/// <summary>
		/// The local app data store
		/// </summary>
		private ApplicationDataContainer appData;

		/// <summary>
		/// Constructor: Load the current app data object.
		/// </summary>
		public DataStorage () {
			this.appData = ApplicationData.Current.LocalSettings;
		}

		/// <summary>
		/// Get information for a saved bus stop.
		/// </summary>
		/// <param name="platformNo">Platform number of saved bus stop</param>
		/// <returns>Object representing the saved stop</returns>
		public Favourite GetFavourite (int platformNo) {
			string key = platformNo.ToString();
			var entry = (ApplicationDataCompositeValue)this.appData.Values[key];
			if (entry == null) {
				throw new KeyNotFoundException("No such platform number in saved stops");
			}
			string stopName = (string)entry[STOP_NAME];
			string customName = (string)entry[CUSTOM_NAME];
			return new Favourite() {
				PlatformNo = platformNo,
				StopName = stopName,
				CustomName = customName
			};
		}

		/// <summary>
		/// Get a collection of all favourite stops.
		/// </summary>
		/// <returns>Collection of all saved stops</returns>
		public List<Favourite> GetAllFavourites () {
			var collection = new List<Favourite>();
			foreach (KeyValuePair<string, object> pair in this.appData.Values) {
				int stopNumber = int.Parse(pair.Key);
				var entry = (ApplicationDataCompositeValue)pair.Value;
				string stopName = (string)entry[STOP_NAME];
				string customName = (string)entry[CUSTOM_NAME];
				collection.Add(new Favourite() {
					PlatformNo = stopNumber,
					StopName = stopName,
					CustomName = customName
				});
			}
			return collection;
		}

		/// <summary>
		/// Store a bus stop in the favourites list.
		/// </summary>
		/// <param name="newStop">Platform to add</param>
		public void AddFavourite (Favourite newStop) {
			var newEntry = new ApplicationDataCompositeValue();
			newEntry[STOP_NAME] = newStop.StopName;
			// Only store a custom name if it isn't empty
			if (!string.IsNullOrEmpty(newStop.CustomName)) {
				newEntry[CUSTOM_NAME] = newStop.CustomName;
			}
			string key = newStop.PlatformNo.ToString();
			this.appData.Values[key] = newEntry;
		}

		/// <summary>
		/// Removes a bus stop from the favourites list.
		/// </summary>
		/// <param name="platformNo">Platform number to remove</param>
		public void RemoveFavourite (int platformNo) {
			string key = platformNo.ToString();
			this.appData.Values.Remove(key);
		}

		/// <summary>
		/// Give a custom name to a saved stop.
		/// </summary>
		/// <param name="platformNo"></param>
		public void ChangeName (int platformNo, string name) {
			string key = platformNo.ToString();
			var entry = (ApplicationDataCompositeValue)this.appData.Values[key];
			if (entry == null) {
				throw new KeyNotFoundException("No such platform number in saved stops");
			}
			entry[CUSTOM_NAME] = name;
			this.appData.Values[key] = entry;
		}

		/// <summary>
		/// Represents information about a saved bus stop.
		/// </summary>
		public class Favourite {
			public int PlatformNo {
				get; set;
			}
			public string StopName {
				get; set;
			}
			public string CustomName {
				get; set;
			}
		}
	}
}
