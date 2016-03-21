using System;
using Starving.Controls;
using Xamarin.Forms;

namespace Starving.Pages
{
	public partial class LoginPage
	{
		private RelativeLayout _mainLayout;
		private Image _starvingLogo;
		private Label _descriptionLabel;
		private Button _facebookButton;

		public void InitializeComponents ()
		{
			_mainLayout = new RelativeLayout ();

			var backgroundImge = new Image {
				Source = "login_background.png",
				Aspect = Aspect.Fill
			};
			_mainLayout.Children.Add (backgroundImge,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height)
			);

			_starvingLogo = new Image {
				Source = "ic_starving.png",
				Aspect = Aspect.AspectFill,
			};
			_mainLayout.Children.Add (_starvingLogo,
				Constraint.RelativeToParent (p => p.Width / 2 - 150),
				Constraint.RelativeToParent (p => p.Height / 2 - 250),
				Constraint.Constant (300),
				Constraint.Constant (300)
			);

			_descriptionLabel = new Label {
				TextColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Center,
			};
			_mainLayout.Children.Add (_descriptionLabel,
				Constraint.Constant (20),
				Constraint.RelativeToParent (p => p.Height / 2),
				Constraint.RelativeToParent (p => p.Width - 40),
				Constraint.Constant (50)
			);

			_facebookButton = new Button {
				Image = "ic_facebook.png",
				TextColor = Color.White,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Styles.Colors.FacebookBlue,
			};
			_facebookButton.Clicked += _facebookButton_Clicked;
			_mainLayout.Children.Add (_facebookButton,
				Constraint.RelativeToParent (p => p.Width / 2 - 125),
				Constraint.RelativeToView (_descriptionLabel, (p, v) => v.Y + v.Height),
				Constraint.Constant (250),
				Constraint.Constant (50)
			);

			Content = _mainLayout;
		}
	}
}


