using System;
using System.Collections.Generic;
using Starving.Models;
using Starving.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using Starving.Controls;
using Starving.Dependencies;
using Starving.Helpers;

namespace Starving.Pages
{
	public partial class PlaceDetailPage : ContentPage
	{
		private Place _place;
		private IParse _parse;
		private User _user;
		private ObservableCollection<Review> _reviewsCollection;
		private bool isLikedUI;
		private bool reviewButtonIsClicked = false;

		public PlaceDetailPage (Place place)
		{
			_place = place;
			_parse = DependencyService.Get<IParse> ();
			_user = _parse.GetCurrentUser ();
			_reviewsCollection = new ObservableCollection<Review> ();
			InitializeComponents ();
			LoadData ().ConfigureAwait (false);
		}

		public async Task LoadData ()
		{
			_photo.Source = await AppConfig.FeaturedImage (_place);
			_nameLabel.Text = _place.Name;
			_vicinityLabel.Text = _place.Vicinity;

			if (_place.OpeningHours == null)
				_openIndicator.BackgroundColor = Styles.Colors.PlaceClosedColor;
			else
				_openIndicator.BackgroundColor = _place.OpeningHours.IsOpenNow ? Styles.Colors.PlaceOpenColor : Styles.Colors.PlaceClosedColor;

			int likes = await RateService.Instance.GetRatesNumber (_place);
			_rateButton.Text = likes.ToString ();

			var position = new Position (_place.Geometry.Location.Latitude, _place.Geometry.Location.Longitude);
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = _place.Name,
				Address = _place.Vicinity
			};
			_map.Pins.Add (pin);

			bool isRated = await RateService.Instance.IsRated (_place, _user);
			await UpdateButtonStyle (isRated);


			List<Review> reviews = await ReviewsService.Instance.GetReviews (_place.PlaceId);
			_mainLayout.Children.Remove (_reviewsPlaceholderLayout);
			if (reviews == null || reviews.Count == 0) {
				_emptyReviews.Text = AppResources.NoReviews;
				_emptyReviews.IsVisible = true;
				_reviewsList.IsVisible = false;
			} else {
				_emptyReviews.IsVisible = false;
				_reviewsList.IsVisible = true;
				foreach (var review in reviews) {
					if (!_reviewsCollection.Any (x => x.ObjectId == review.ObjectId))
						_reviewsCollection.Add (review);
				}
			}
		}

		async void _rateButton_Clicked (object sender, EventArgs e)
		{
			isLikedUI = !isLikedUI;
			InvertButtonStyle (isLikedUI);
			_rateButton.IsEnabled = false;
			bool isRated = await RateService.Instance.IsRated (_place, _user);
			var rate = new Rate {
				User = _user,
				Place = _place,
			};
			if (isRated) {
				await RateService.Instance.DeleteRate (rate);
			} else {
				await RateService.Instance.CreateRate (rate);
			}
			await UpdateButtonStyle (!isRated);
			_rateButton.IsEnabled = true;
		}

		private async Task UpdateButtonStyle (bool isRated)
		{
			if (isRated) {
				_rateButton.Style = Styles.ActiveButtonStyle;
				_rateButton.Image = "ic_favorite_on.png";
			} else {
				_rateButton.Style = Styles.InactiveButtonStyle;
				_rateButton.Image = "ic_favorite_off.png";
			}
			int ratesNumber = await RateService.Instance.GetRatesNumber (_place);
			_rateButton.Text = ratesNumber.ToString ();
		}

		private async void InvertButtonStyle (bool isLikedUI)
		{
			if (isLikedUI) {
				_rateButton.Style = Styles.ActiveButtonStyle;
				_rateButton.Image = "ic_favorite_on.png";
				var likes = int.Parse (_rateButton.Text);
				_rateButton.Text = (++likes).ToString ();
			} else {
				_rateButton.Style = Styles.InactiveButtonStyle;
				_rateButton.Image = "ic_favorite_off.png";
				var likes = int.Parse (_rateButton.Text);
				_rateButton.Text = (--likes).ToString ();
			}
		}

		async void CreateReviewImage_Clicked (object sender, EventArgs e)
		{
			if (!reviewButtonIsClicked) {
				reviewButtonIsClicked = true;
				await Navigation.PushAsync (new CreateReviewPage (_place, () => LoadData ()));
				reviewButtonIsClicked = false;
			}
		}

		void _reviewsList_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			var list = (ListView)sender;
			list.SelectedItem = null;
		}

		void _seeLikes_Clicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new LikesPage (_place));
		}

		void _wazeButton_Clicked (object sender, EventArgs e)
		{
			ITools service = DependencyService.Get<ITools> ();
			service.OpenWaze (_place.Geometry.Location.Latitude, _place.Geometry.Location.Longitude);
		}
	}
}


