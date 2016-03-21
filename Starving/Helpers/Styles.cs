using System;
using Xamarin.Forms;

namespace Starving
{
	public static class Styles
	{
		public static class Colors
		{
			public static readonly Color PrimaryColor = Color.FromHex ("#f2635f");
			public static readonly Color SecondaryColor = Color.FromHex ("#666666");
			public static readonly Color ThirdColor = Color.FromHex ("#37AC72");
			public static readonly Color FacebookBlue = Color.FromHex ("#3b5998");

			public static readonly Color PlaceOpenColor = Color.FromHex ("#2ecc71");
			public static readonly Color PlaceClosedColor = Color.FromHex ("#95a5a6");
			public static readonly Color PlaceholderColor = Color.FromHex ("#ecf0f1");

			public static readonly Color ErrorColor = Color.FromHex ("#c0392b");
			public static readonly Color WarningColor = Color.FromHex ("#d35400");
			public static readonly Color SuccessColor = Color.FromHex ("#2ecc71");
			public static readonly Color BlueColor = Color.FromHex ("#2980b9");
		}

		public static readonly Style Title = new Style (typeof(Label)) {
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Styles.Colors.PrimaryColor },
				new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
				new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Medium, typeof(Label)) },
				new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
			}
		};

		public static readonly Style Subtitle = new Style (typeof(Label)) {
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Styles.Colors.SecondaryColor },
				new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
			}
		};

		public static readonly Style ActiveButtonStyle = new Style (typeof(Button)) {
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Styles.Colors.PrimaryColor },
				new Setter { Property = Button.TextColorProperty, Value = Color.White },
				new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
				new Setter { Property = Button.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Micro, typeof(Button)) },
			}
		};

		public static readonly Style InactiveButtonStyle = new Style (typeof(Button)) {
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Styles.Colors.PlaceholderColor },
				new Setter { Property = Button.TextColorProperty, Value = Styles.Colors.SecondaryColor },
				new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
				new Setter { Property = Button.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Micro, typeof(Button)) },
			}
		};
	}
}

