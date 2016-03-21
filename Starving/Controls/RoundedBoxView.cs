using System;
using Xamarin.Forms;

namespace Starving.Controls
{
	public class RoundedBoxView : BoxView
	{
		public static readonly BindableProperty CornerRadiusProperty = 
			BindableProperty.Create<RoundedBoxView,float>( 
				p => p.CornerRadius, 0 
			);

		public float CornerRadius {
			get { return (float)GetValue ( CornerRadiusProperty ); }
			set { SetValue ( CornerRadiusProperty, value ); }
		}

		public static readonly BindableProperty BorderWidthProperty = 
			BindableProperty.Create<RoundedBoxView,float>( 
				p => p.BorderWidth, 1
			);

		public float BorderWidth {
			get { return (float)GetValue ( BorderWidthProperty ); }
			set { SetValue ( BorderWidthProperty, value ); }
		}

		public static readonly BindableProperty BorderColorProperty = 
			BindableProperty.Create<RoundedBoxView,Color>( 
				p => p.BorderColor, Color.Transparent
			);

		public Color BorderColor {
			get { return (Color)GetValue ( BorderColorProperty ); }
			set { SetValue ( BorderColorProperty, value ); }
		}
	}
}


