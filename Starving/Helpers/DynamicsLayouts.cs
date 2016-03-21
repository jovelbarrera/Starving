using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Starving.Helpers
{
	public static class DynamicsLayouts
	{
		private static RelativeLayout _alertLayout;

		public static RelativeLayout Loading (string message, Color backgroundColor)
		{
			var chargingLayout = new RelativeLayout ();
			chargingLayout.BackgroundColor = backgroundColor;

			var activityIndicator = new ActivityIndicator {
				IsRunning = true,
				Color = Styles.Colors.PrimaryColor,
			};
			chargingLayout.Children.Add (activityIndicator,
				Constraint.RelativeToParent (p => p.Width / 2 - 15),
				Constraint.RelativeToParent (p => p.Height / 2 - 10 - 30),
				Constraint.Constant (30),
				Constraint.Constant (30)
			);

			var messageLabel = new Label {
				Text = message,
				Style = Styles.Subtitle,
				HorizontalTextAlignment = TextAlignment.Center
			};
			chargingLayout.Children.Add (messageLabel,
				Constraint.RelativeToView (activityIndicator, (p, v) => v.X + 15 - 100),
				Constraint.RelativeToView (activityIndicator, (p, v) => v.Y + v.Height + 10),
				Constraint.Constant (200),
				Constraint.Constant (40)
			);

			return chargingLayout;
		}

		public static void Alert (this RelativeLayout parentLayout, string message, EventHandler buttonAction = null)
		{
			parentLayout.RemoveAlert ();
			_alertLayout = new RelativeLayout ();
			_alertLayout.BackgroundColor = Color.FromHex ("#222222");

			var messageLabel = new Label {
				Text = message,
				TextColor = Color.White,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalTextAlignment = TextAlignment.Center
			};
			var calltoactionButton = new Button {
				Text = AppResources.TryAgain,
				//HorizontalOptions = LayoutOptions.End,
				Style = Styles.ActiveButtonStyle,
				HeightRequest = 40,
				HorizontalOptions = LayoutOptions.Center
			};
			calltoactionButton.Clicked += buttonAction;
			var contentLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness (20, 20, 20, 20),
				Children = {
					messageLabel,
					calltoactionButton
				}
			};

			parentLayout.Children.Add (_alertLayout,
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Height - 100),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.Constant (100)
			);
			_alertLayout.Children.Add (contentLayout,
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Height - 100),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.Constant (100)
			);
		}

		public static void RemoveAlert (this RelativeLayout parentLayout)
		{
			if (_alertLayout != null && parentLayout.Children.Contains (_alertLayout))
				parentLayout.Children.Remove (_alertLayout);
		}
	}
}


