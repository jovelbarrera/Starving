using System;

using Xamarin.Forms;
using Starving.Models;
using Starving.Services;

namespace Starving.Pages
{
	public class MainPage : MasterDetailPage
	{
		public MainPage ()
		{
			UpdateMaster ();
			Detail = AppConfig.CreateNavigationPage (new PlacesPage ());
		}

		void ProfilePage_SearchPlaces_Clicked (object sender, EventArgs e)
		{
			Detail = AppConfig.CreateNavigationPage (new PlacesPage ());
			IsPresented = false;
			UpdateMaster ();
		}

		private async void ReviewsList_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			var list = (ListView)sender;
			Review review = (Review)e.Item;
			Place place = await GoogleService.Instance.GetPlace (review.PlaceId);
			list.SelectedItem = null;

			Detail = AppConfig.CreateNavigationPage (new PlaceDetailPage (place));
			IsPresented = false;
			UpdateMaster ();
		}

		private void UpdateMaster ()
		{
			var profilePage = new ProfilePage ();
			profilePage.ReviewsList.ItemTapped += ReviewsList_ItemTapped;
			profilePage.SearchPlaces.Clicked += ProfilePage_SearchPlaces_Clicked;
			Master = profilePage;
		}
	}
}


