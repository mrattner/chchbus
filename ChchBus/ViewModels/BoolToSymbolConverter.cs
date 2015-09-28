using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;

namespace ChchBus {
	/// <summary>
	/// Converts a boolean value to a solid or outline star symbol.
	/// </summary>
	class BoolToSymbolConverter : IValueConverter {
		public object Convert (object value, Type targetType, object parameter, string language) {
			if (!(value is bool)) {
				throw new ArgumentException();
			}
			return (bool)value ? Symbol.SolidStar : Symbol.OutlineStar;
		}

		public object ConvertBack (object value, Type targetType, object parameter, string language) {
			if (!(value is Symbol)) {
				throw new ArgumentException();
			}
			return (Symbol)value == Symbol.SolidStar;
		}
	}
}
