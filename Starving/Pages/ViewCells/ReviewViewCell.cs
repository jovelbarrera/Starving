using System;
using Xamarin.Forms;
using Starving.Models;
using Starving.Controls;

namespace Starving
{
	public class ReviewViewCell : ViewCell
	{
		private RoundedImage _profileImage;
		private Label _nameLabel;
		private Label _commentLabel;
		private Label _dateLabel;

		public ReviewViewCell ()
		{
			var mainLayout = new RelativeLayout ();

			_profileImage = new RoundedImage {
				Aspect = Aspect.AspectFill,
				BorderColor = Styles.Colors.SecondaryColor,
				BorderWidth = 1,
				Source = "ic_user_picture_placeholder.png",
			};
			mainLayout.Children.Add (_profileImage,
				Constraint.Constant (20),
				Constraint.Constant (20),
				Constraint.Constant (40),
				Constraint.Constant (40)
			);

			_nameLabel = new Label {
				FontSize = Device.GetNamedSize (NamedSize.Micro, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				TextColor = Styles.Colors.SecondaryColor,
				VerticalTextAlignment = TextAlignment.Start,
			};
			mainLayout.Children.Add (_nameLabel,
				Constraint.RelativeToView (_profileImage, (p, v) => v.X + v.Width + 5),
				Constraint.RelativeToView (_profileImage, (p, v) => v.Y),
				Constraint.RelativeToView (_profileImage, (p, v) => p.Width - (v.X + v.Width) - 20),
				Constraint.Constant (30)
			);

			_commentLabel = new Label {
				FontSize = Device.GetNamedSize (NamedSize.Micro, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Start,
			};
			mainLayout.Children.Add (_commentLabel,
				Constraint.RelativeToView (_nameLabel, (p, v) => v.X),
				Constraint.RelativeToView (_nameLabel, (p, v) => v.Y + v.Height),
				Constraint.RelativeToView (_nameLabel, (p, v) => v.Width),
				Constraint.Constant (30)
			);

			_dateLabel = new Label {
				FontSize = Device.GetNamedSize (NamedSize.Micro, typeof(Label)),
				FontAttributes = FontAttributes.Italic,
				VerticalTextAlignment = TextAlignment.Start,
				TextColor = Styles.Colors.SecondaryColor,
			};
			mainLayout.Children.Add (_dateLabel,
				Constraint.RelativeToView (_commentLabel, (p, v) => v.X),
				Constraint.RelativeToView (_commentLabel, (p, v) => v.Y + v.Height),
				Constraint.RelativeToView (_commentLabel, (p, v) => v.Width),
				Constraint.Constant (20)
			);

			View = mainLayout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			Review review = this.BindingContext as Review;
			if (review == null)
				return;

			if (review.User.ProfilePicture != null && !string.IsNullOrEmpty (review.User.ProfilePicture.Url))
				_profileImage.Source = AppConfig.CacheImageSource (review.User.ProfilePicture.Url);

			_nameLabel.Text = review.User.Name;
			_commentLabel.Text = review.Comment;
			DateTime date = review.CreatedAt;
			_dateLabel.Text = AppConfig.HumanTimeDescription (date, AppResources.Liked);
		}
	}
}

