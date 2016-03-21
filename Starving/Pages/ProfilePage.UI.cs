using System;
using Starving.Controls;
using Xamarin.Forms;
using Starving.Helpers;

namespace Starving.Pages
{
	public partial class ProfilePage
	{
		private RelativeLayout _mainLayout;
		private RoundedImage _picture;
		private Label _nameLabel;
		private Label _latestReviewsTitleLabel;
		public ListView ReviewsList;
		private RelativeLayout _reviewsPlaceholderLayout;
		private Label _emptyReviews;
		public Button SearchPlaces;

		private void InitializeComponents ()
		{
			Icon = "drawer_button.png";
			Title = "Starving";

			_mainLayout = new RelativeLayout {
				BackgroundColor = Color.White,
			};

			var headerBoxview = new BoxView {
				BackgroundColor = Styles.Colors.PrimaryColor,
			};
			_mainLayout.Children.Add (headerBoxview,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height / 3)
			);

			_picture = new RoundedImage {
				Aspect = Aspect.AspectFill,
				BorderColor = Styles.Colors.SecondaryColor,
				BorderWidth = 1,
				Source = "ic_user_picture_placeholder.png",
			};
			_mainLayout.Children.Add (_picture,
				Constraint.RelativeToParent (p => p.Width / 2 - 50),
				Constraint.RelativeToParent (p => p.Height / 3 - 150),
				Constraint.Constant (100),
				Constraint.Constant (100)
			);

			_nameLabel = new Label {
				Style = Styles.Subtitle,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
			};
			_mainLayout.Children.Add (_nameLabel,
				Constraint.Constant (20),
				Constraint.RelativeToView (_picture, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width - 40),
				Constraint.Constant (40)
			);

			SearchPlaces = new Button {
				Style = Styles.ActiveButtonStyle,
				Text = AppResources.SearchPlaces
			};
			_mainLayout.Children.Add (SearchPlaces,
				Constraint.RelativeToView (headerBoxview, (p, v) => v.X + 20),
				Constraint.RelativeToView (headerBoxview, (p, v) => v.Y + v.Height + 10),
				Constraint.RelativeToParent (p => p.Width - 40),
				Constraint.Constant (40)
			);
			var separator = new BoxView {
				BackgroundColor = Styles.Colors.PlaceholderColor,
			};
			_mainLayout.Children.Add (separator,
				Constraint.RelativeToView (SearchPlaces, (p, v) => v.X - 20),
				Constraint.RelativeToView (SearchPlaces, (p, v) => v.Y + v.Height + 10),
				Constraint.RelativeToView (SearchPlaces, (p, v) => v.Width + 40),
				Constraint.Constant (1)
			);

			_latestReviewsTitleLabel = new Label {
				Style = Styles.Subtitle,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Styles.Colors.PrimaryColor,
			};
			_mainLayout.Children.Add (_latestReviewsTitleLabel,
				Constraint.RelativeToView (SearchPlaces, (p, v) => v.X),
				Constraint.RelativeToView (separator, (p, v) => v.Y + v.Height + 10),
				Constraint.RelativeToView (SearchPlaces, (p, v) => v.Width),
				Constraint.Constant (40)
			);

			_emptyReviews = new Label {
				IsVisible = false,
				Style = Styles.Subtitle,
				HorizontalTextAlignment = TextAlignment.Center
			};
			_mainLayout.Children.Add (_emptyReviews,
				Constraint.Constant (0),
				Constraint.RelativeToView (_latestReviewsTitleLabel, (p, v) => v.Y + v.Height),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToView (_latestReviewsTitleLabel, (p, v) => p.Height - (v.Y + v.Height))
			);

			ReviewsList = new ListView {
				RowHeight = 100,
				ItemTemplate = new DataTemplate (typeof(ReviewViewCell)),
				BackgroundColor = Color.White,
				IsVisible = false,
			};
			ReviewsList.ItemsSource = _reviewsCollection;
			_mainLayout.Children.Add (ReviewsList,
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.X),
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.Y),
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.Width),
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.Height)
			);

			_reviewsPlaceholderLayout = DynamicsLayouts.Loading (AppResources.RetrievingReviews, Color.Transparent);
			_mainLayout.Children.Add (_reviewsPlaceholderLayout,
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.X),
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.Y),
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.Width),
				Constraint.RelativeToView (_emptyReviews, (p, v) => v.Height)
			);

			Content = _mainLayout;
		}
	}
}


