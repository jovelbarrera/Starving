using System;
using Xamarin.Forms;
using Starving.Models;
using Starving.Controls;

namespace Starving
{
	public class UserViewCell : ViewCell
	{
		private RoundedImage _profileImage;
		private Label _nameLabel;

		public UserViewCell ()
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
				Style = Styles.Subtitle,
				FontAttributes = FontAttributes.Bold,
			};
			mainLayout.Children.Add (_nameLabel,
				Constraint.RelativeToView (_profileImage, (p, v) => v.X + v.Width + 5),
				Constraint.RelativeToView (_profileImage, (p, v) => v.Y),
				Constraint.RelativeToView (_profileImage, (p, v) => p.Width - (v.X + v.Width) - 20),
				Constraint.Constant (40)
			);

			View = mainLayout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			Rate rate = this.BindingContext as Rate;
			if (rate == null)
				return;

			if (rate.User.ProfilePicture != null && !string.IsNullOrEmpty (rate.User.ProfilePicture.Url))
				_profileImage.Source = AppConfig.CacheImageSource (rate.User.ProfilePicture.Url);
			_nameLabel.Text = rate.User.Name;
		}
	}
}

