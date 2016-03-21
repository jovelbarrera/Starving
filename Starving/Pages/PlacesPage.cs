using System;
using Xamarin.Forms;
using Starving.Models;
using System.Collections.Generic;
using Starving.Services;
using System.Threading.Tasks;
using Starving.Dependencies;
using Starving.Helpers;
using Plugin.Connectivity;

namespace Starving.Pages
{
	public partial class PlacesPage : ContentPage
	{
		public PlacesPage ()
		{
			InitializeComponents ();
			LoadData ().ConfigureAwait (false);	
		}

		private async Task LoadData ()
		{
			if (!CrossConnectivity.Current.IsConnected) {
				ShowAlert (AppResources.NoInternet);
			} else {
				var Tools = DependencyService.Get<ITools> ();
				Dictionary<string, double> currentLocation = await Tools.GetCurrentLocation ();

				if (currentLocation.Count == 0) {
					ShowAlert (AppResources.EnableGPS);
				} else {
					await LoadList (currentLocation);
				}
			}
		}

		private async Task LoadList (Dictionary<string, double> currentLocation)
		{
			_mainLayout.RemoveAlert ();
			_placesListView.IsVisible = true;
			List<Place> nearPlaces = await GoogleService.Instance.NearPlaces (currentLocation ["Latitude"], currentLocation ["Longitude"]);
			_mainLayout.Children.Remove (_loadingLayoutPlaces);
			if (nearPlaces == null || nearPlaces.Count == 0) {
				ShowAlert (AppResources.NoPlacesToShow);
			} else {
				_placesListView.ItemsSource = nearPlaces;
			}
		}

		private void ShowAlert (string message)
		{
			_mainLayout.Children.Remove (_loadingLayoutPlaces);
			_mainLayout.Alert (message, (s, e) => ListView_Refreshing (s, e));
			_placesListView.IsVisible = false;
		}

		void _places_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			var list = (ListView)sender;
			list.IsEnabled = false;
			var place = (Place)e.Item;
			list.SelectedItem = null;
			Navigation.PushAsync (new PlaceDetailPage (place));
			list.IsEnabled = true;
		}


		async void ListView_Refreshing (object sender, EventArgs e)
		{
			_placesListView.IsRefreshing = true;
			await LoadData ();
			_placesListView.IsRefreshing = false;
		}
	}
}


