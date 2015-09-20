using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChchBus {
	/// <summary>
	/// Page for seeing the next buses to arrive at a stop.
	/// </summary>
	public sealed partial class NextBusesPage : Page {
		/// <summary>
		/// Constructor.
		/// </summary>
		public NextBusesPage () {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when a stop is entered into the box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextBox_TextChanged (object sender, TextChangedEventArgs e) {
			if (Regex.IsMatch(entryBox.Text, @"^\d{5}$")) {
				// This is a platform number
				NextBuses vm = DataContext as NextBuses;
				vm.FetchETAs(int.Parse(entryBox.Text));
			}
		}
	}
}
