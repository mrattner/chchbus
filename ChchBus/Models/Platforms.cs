using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using System.IO;

namespace ChchBus {
	/// <summary>
	/// Interfaces with the Metroinfo API to get bus stop information.
	/// </summary>
	public class Platforms {
		/// <summary>
		/// URL for all stops, not grouped
		/// </summary>
		private static readonly string ALL_PLATFORMS_URL = 
			"http://rtt.metroinfo.org.nz/rtt/public/utility/file.aspx?ContentType=SQLXML&Name=JPPlatform";

		/// <summary>
		/// URL for SOME stops, grouped by key locations
		/// </summary>
		private static readonly string PLATFORM_GROUPS_URL = 
			"http://rtt.metroinfo.org.nz/rtt/public/utility/file.aspx?ContentType=SQLXML&Name=JPPlatformGroup";

		/// <summary>
		/// Used for fetching XML from metroinfo API
		/// </summary>
		private HttpClient client;

		/// <summary>
		/// Constructor: Sets up the HTTP client.
		/// </summary>
		public Platforms () {
			this.client = new HttpClient();
		}

		/// <summary>
		/// Get all bus stops whose name or location matches the given search string.
		/// </summary>
		/// <remarks>
		/// Should we cache this information?
		/// </remarks>
		/// <param name="pattern">Street name or location to search for</param>
		/// <returns>List of all matching bus stops</returns>
		public async Task<IEnumerable<Platform>> FindPlatforms (string pattern) {
			string allStopsXML = await client.GetStringAsync(new Uri(ALL_PLATFORMS_URL));
			XDocument allStops = XDocument.Load(allStopsXML);
			Stream platformGroupsXML = await client.GetStreamAsync(new Uri(PLATFORM_GROUPS_URL));
			XDocument platformGroups = XDocument.Load(platformGroupsXML);

			// Parse the XML, pick out stops that match the pattern
			List<Platform> platforms = new List<Platform>();
			return platforms;
		}

		/// <summary>
		/// Get information about a single bus stop.
		/// </summary>
		/// <param name="platformNo">Platform number of the bus stop</param>
		/// <returns></returns>
		public async Task<Platform> GetPlatform (int platformNo) {
			Stream allStopsXML = await client.GetStreamAsync(new Uri(ALL_PLATFORMS_URL));
			XDocument allStops = XDocument.Load(allStopsXML);
			XElement platform = allStops.Elements("Platform").First(element =>
				(int)element.Attribute("PlatformNo") == platformNo);
			if (platform == null) {
				return null;
			}

			string name = (string)platform.Attribute("Name");
			string road = (string)platform.Attribute("RoadName");

			string bearingToRoad = (string)platform.Attribute("BearingToRoad");
			double bearing = double.Parse(bearingToRoad, System.Globalization.NumberStyles.Float);

			XElement position = platform.Element("Position");
			string lati = (string)position.Attribute("Latitude");
			string longi = (string)position.Attribute("Longitude");
			double latitude = double.Parse(lati, System.Globalization.NumberStyles.Float);
			double longitude = double.Parse(longi, System.Globalization.NumberStyles.Float);

			return new Platform() {
				PlatformNo = platformNo,
				Name = name,
				Road = road,
				Bearing = bearing,
				Latitude = latitude,
				Longitude = longitude
			};
		}

		/// <summary>
		/// Represents a bus platform in the Chch Metro network.
		/// </summary>
		public class Platform {
			public int PlatformNo {
				get; set;
			}
			public string Name {
				get; set;
			}
			public string Road {
				get; set;
			}
			public double Bearing {
				get; set;
			}
			public double Latitude {
				get; set;
			}
			public double Longitude {
				get; set;
			}
		}
	}
}
