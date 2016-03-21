using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Starving.Services;
using Starving.Models;
using System.Collections.Generic;

namespace Starving
{
	public partial class LikesPage : ContentPage
	{
		private Place _place;

		public LikesPage (Place place)
		{
			_place = place;
			InitializeComponents ();
			LoadData ().ConfigureAwait (false);
		}

		public async Task LoadData ()
		{
			List<Rate> ratesList = await RateService.Instance.GetRates (_place);
			_mainLayout.Children.Remove (_loadingLayoutLikes);
			if (ratesList == null || ratesList.Count == 0) {
				_emptyLabel.Text = AppResources.NoLikesToShow;
			} else {
				_emptyLabel.IsVisible = false;
				_userList.IsVisible = true;
				_titleLablel.IsVisible = true;
				_userList.ItemsSource = ratesList;
			}
		}

		void _userList_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			var list = (ListView)sender;
			list.SelectedItem = null;
		}
	}
}


