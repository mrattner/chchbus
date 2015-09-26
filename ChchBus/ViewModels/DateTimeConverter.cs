using System;
using Windows.UI.Xaml.Data;

namespace ChchBus {
	/// <summary>
	/// Converts a DateTime to a formatted string like "18:15:14 06-Jan-15".
	/// </summary>
	class DateTimeConverter : IValueConverter {
		public object Convert (object value, Type targetType, object parameter, string language) {
			if (!(value is DateTime)) {
				throw new ArgumentException();
			}
			return string.Format("{0:HH:mm:ss dd-MMM-yy}", (DateTime)value);
		}

		public object ConvertBack (object value, Type targetType, object parameter, string language) {
			return DateTime.Parse(value.ToString());
		}
	}
}
