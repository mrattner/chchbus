using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace ChchBus {
	/// <summary>
	/// Interfaces with the MetroInfo API to get estimated times of arrival at a bus stop.
	/// </summary>
	class PlatformETAs {
		/// <summary>
		/// Used for fetching XML from metroinfo API
		/// </summary>
		private HttpClient client;

		/// <summary>
		/// URL for getting estimated times of arrival (requires platform number).
		/// Also gets alerts if there are any, but we do not check for alerts.
		/// </summary>
		private static string ETA_URL_BASE =
			"http://rtt.metroinfo.org.nz/rtt/public/utility/file.aspx?ContentType=SQLXML&Name=JPRoutePositionET2&PlatformNo=";

		/// <summary>
		/// Constructor: Initialise the HTTP client.
		/// </summary>
		public PlatformETAs () {
			this.client = new HttpClient();
		}

		/// <summary>
		/// Get the estimated times of arrival of all buses coming to a platform.
		/// </summary>
		/// <remarks>
		/// The results are not sorted.
		/// </remarks>
		/// <param name="platformId">Bus stop for which to retrieve ETAs</param>
		/// <returns>Tuple with (retrieval time, stop name, list of ETAs), or
		/// null if the platform was not found
		/// </returns>
		public async Task<Tuple<DateTime, string, List<ETA>>> GetETAs (int platformId) {
			Stream etaXML = await client.GetStreamAsync(new Uri(ETA_URL_BASE + platformId));
			XElement xdoc = XDocument.Load(etaXML).Root;
			XNamespace ns = xdoc.Name.Namespace;
			
			var content = xdoc.Element(ns + "Content");
			var platform = xdoc.Element(ns + "Platform");

			if (platform == null) {
				return null;
			}

			DateTime timeRetrieved = DateTime.Parse((string)content.Attribute("Expires"));
			string stopName = (string)platform.Attribute("Name");

			List<ETA> etaList = new List<ETA>();
			var routes = platform.Elements(ns + "Route");
			foreach (XElement route in routes) {
				string routeNo = (string)route.Attribute("RouteNo");
				// Each route has 1 or more destinations
				var destinations = route.Elements(ns + "Destination");
				foreach (XElement destination in destinations) {
					string dest = (string)destination.Attribute("Name");
					// Each destination has 1 or more trips
					var trips = destination.Elements(ns + "Trip");
					foreach (XElement trip in trips) {
						int etaMins = (int)trip.Attribute("ETA");
						etaList.Add(new ETA() {
							RouteNo = routeNo,
							Destination = dest,
							Mins = etaMins
						});
					}
				}
			}
			return new Tuple<DateTime, string, List<ETA>>(timeRetrieved, stopName, etaList);
		}

		/// <summary>
		/// Represents a Trip (estimated time of arrival of a particular bus).
		/// </summary>
		public class ETA : IComparable<ETA> {
			public string RouteNo {
				get; set;
			}
			public string Destination {
				get; set;
			}
			public int Mins {
				get; set;
			}

			public int CompareTo (ETA other) {
				return this.Mins.CompareTo(other.Mins);
			}
		}
	}
}
