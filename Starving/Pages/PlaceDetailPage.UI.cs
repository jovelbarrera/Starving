using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Starving.Controls;
using Starving.Helpers;

namespace Starving.Pages
{
	public partial class PlaceDetailPage
	{
		private RelativeLayout _mainLayout;
		private RelativeLayout _reviewsPlaceholderLayout;
		private Label _nameLabel;
		private Label _vicinityLabel;
		private RoundedBoxView _openIndicator;
		private Image _photo;
		private Button _rateButton;
		private Button _seeLikes;
		private Button _wazeButton;
		private Map _map;
		private ListView _reviewsList;
		private Label _emptyReviews;

		public void InitializeComponents ()
		{
			Title = _place.Name;
			_mainLayout = new RelativeLayout ();

			_photo = new Image {
				Aspect = Aspect.AspectFill,
			};
			_mainLayout.Children.Add (_photo,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height / 3)
			);

			var photoOverlay = new BoxView {
				BackgroundColor = Color.Black,
				Opacity = 0.6
			};
			_mainLayout.Children.Add (photoOverlay,
				Constraint.RelativeToView (_photo, (p, v) => v.X),
				Constraint.RelativeToView (_photo, (p, v) => v.Y),
				Constraint.RelativeToView (_photo, (p, v) => v.Width),
				Constraint.RelativeToView (_photo, (p, v) => v.Height)
			);

			_nameLabel = new Label {
				Style = Styles.Title,
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.StartAndExpand,
			};
			_openIndicator = new RoundedBoxView {
				BackgroundColor = Color.FromHex ("#95a5a6"),
				CornerRadius = 5,
				HeightRequest = 10,
				WidthRequest = 10,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Start,
			};

			_vicinityLabel = new Label {
				TextColor = Color.White,
				Style = Styles.Subtitle,
				VerticalTextAlignment = TextAlignment.Start,
				VerticalOptions = LayoutOptions.Start,
			};

			_rateButton = new Button {
				Image = "ic_favorite_off.png",
				Style = Styles.InactiveButtonStyle,
				Text = "...",
				HeightRequest = 40,
				VerticalOptions = LayoutOptions.End,
			};
			_rateButton.Clicked += _rateButton_Clicked;
			_wazeButton = new Button {
				Image = "ic_waze_logo.png",
				Style = Styles.InactiveButtonStyle,
				FontSize = Device.GetNamedSize (NamedSize.Micro, typeof(Label)),
				Text = AppResources.TakeMeThere,
				HeightRequest = 40,
				VerticalOptions = LayoutOptions.End,
			};
			_wazeButton.Clicked += _wazeButton_Clicked;
			_seeLikes = new Button {
				Style = Styles.InactiveButtonStyle,
				FontSize = Device.GetNamedSize (NamedSize.Micro, typeof(Label)),
				Text = AppResources.SeeLikes,
				HeightRequest = 40,
				VerticalOptions = LayoutOptions.End,
			};
			_seeLikes.Clicked += _seeLikes_Clicked;

			var titleLayout = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness (20, 20, 20, 0),
				VerticalOptions = LayoutOptions.Start,
				Children = { _nameLabel, _openIndicator },
			};
			var subtitleLayout = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = new Thickness (20, 0, 20, 0),
				Children = { _vicinityLabel },
			};
			var buttonsLayout = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness (20, 0, 20, 20),
				VerticalOptions = LayoutOptions.End,
				HorizontalOptions = LayoutOptions.End,
				Children = { _seeLikes, _wazeButton, _rateButton }
			};

			var headerLayout = new StackLayout {
				Children = { titleLayout, subtitleLayout, buttonsLayout }
			};

			_mainLayout.Children.Add (headerLayout,
				Constraint.RelativeToView (_photo, (p, v) => v.X),
				Constraint.RelativeToView (_photo, (p, v) => v.Y),
				Constraint.RelativeToView (_photo, (p, v) => v.Width),
				Constraint.RelativeToView (_photo, (p, v) => v.Height)
			);

			_map = new Map (
				MapSpan.FromCenterAndRadius (
					new Position (_place.Geometry.Location.Latitude, _place.Geometry.Location.Longitude), Distance.FromMiles (0.3))) {
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			_mainLayout.Children.Add (_map,
				Constraint.Constant (0),
				Constraint.RelativeToView (_photo, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height / 3)
			);

			_emptyReviews = new Label {
				IsVisible = false,
				Style = Styles.Subtitle,
				HorizontalTextAlignment = TextAlignment.Center
			};
			_mainLayout.Children.Add (_emptyReviews,
				Constraint.Constant (0),
				Constraint.RelativeToView (_map, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height / 3)
			);

			_reviewsList = new ListView {
				RowHeight = 100,
				ItemTemplate = new DataTemplate (typeof(ReviewViewCell)),
				BackgroundColor = Color.White,
				IsVisible = false,
			};
			_reviewsList.ItemsSource = _reviewsCollection;
			_reviewsList.ItemTapped += _reviewsList_ItemTapped;
			_mainLayout.Children.Add (_reviewsList,
				Constraint.Constant (0),
				Constraint.RelativeToView (_map, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height / 3)
			);

			_reviewsPlaceholderLayout = DynamicsLayouts.Loading (AppResources.RetrievingReviews, Color.Transparent);
			_mainLayout.Children.Add (_reviewsPlaceholderLayout,
				Constraint.Constant (0),
				Constraint.RelativeToView (_map, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height / 3)
			);

			var createReviewImage = new Image {
				Source = "ic_create_review.png"
			};
			_mainLayout.Children.Add (createReviewImage,
				Constraint.RelativeToParent (p => p.Width - 100),
				Constraint.RelativeToParent (p => p.Height - 100),
				Constraint.Constant (80),
				Constraint.Constant (80)
			);
			var tapGesture = new TapGestureRecognizer ();
			tapGesture.Tapped += CreateReviewImage_Clicked;
			createReviewImage.GestureRecognizers.Add (tapGesture);

			Content = _mainLayout;
		}
	}
}


