using System;
using Xamarin.Forms;
using Starving.Models;
using Starving.Services;
using System.Threading.Tasks;
using System.Linq;
using Starving.Dependencies;
using Starving.Controls;
using System.IO;
using System.Collections.Generic;

namespace Starving
{
	public class PlaceViewCell : ViewCell
	{
		private IParse _parse;
		private Place _place;
		private User _user;
		private Image _backgroundImage;
		private Label _nameLabel;
		private Label _vicinityLabel;
		private Button _rateButton;
		private bool isLikedUI;

		public PlaceViewCell ()
		{
			_backgroundImage = new Image {
				Aspect = Aspect.AspectFill,
				HeightRequest = 130,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			_nameLabel = new Label {
				Style = Styles.Title,
			};

			_vicinityLabel = new Label {
				Style = Styles.Subtitle,
			};

			_rateButton = new Button {
				Image = "ic_favorite_off.png",
				Style = Styles.InactiveButtonStyle,
				Text = "...",
				HeightRequest = 40,
				WidthRequest = 100,
				HorizontalOptions = LayoutOptions.Start
			};

			var infoLayout = new StackLayout {
				Padding = new Thickness (5, 0, 5, 0),
				Children = { _nameLabel, _vicinityLabel, _rateButton }
			};

			var containerLayout = new StackLayout {
				BackgroundColor = Color.White,
				Spacing = 0,
				Children = { _backgroundImage, infoLayout }
			};
			var mainLayout = new StackLayout {
				Padding = new Thickness (10, 10, 10, 10),
				Spacing = 0,
				Children = { containerLayout }
			};

			View = mainLayout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			Place place = this.BindingContext as Place;
			_place = place;

			if (_place == null)
				return;

			_parse = DependencyService.Get<IParse> ();
			_user = _parse.GetCurrentUser ();

			_nameLabel.Text = _place.Name;
			_vicinityLabel.Text = _place.Vicinity;
			LoadData ().ConfigureAwait (false);
		}

		private async Task LoadData ()
		{
			_backgroundImage.Source = await AppConfig.FeaturedImage (_place);
			bool isRated = await RateService.Instance.IsRated (_place, _user);
			isLikedUI = isRated;
			await UpdateButtonStyle (isRated);

			_rateButton.Clicked += async (sender, e) => {
				isLikedUI = !isLikedUI;
				InvertButtonStyle (isLikedUI);
				_rateButton.IsEnabled = false;
				isRated = await RateService.Instance.IsRated (_place, _user);
				var rate = new Rate {
					User = _user,
					Place = _place,
				};
				if (isRated) {
					await RateService.Instance.DeleteRate (rate);
				} else {
					await RateService.Instance.CreateRate (rate);
				}
				UpdateButtonStyle (!isRated);
				_rateButton.IsEnabled = true;
			};
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
	}
}

