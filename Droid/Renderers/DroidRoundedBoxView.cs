using System;
using Starving.Controls;
using Xamarin.Forms;
using System.ComponentModel;
using Starving.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer (typeof(RoundedBoxView), typeof(DroidRoundedBoxView))]
namespace Starving.Droid.Renderers
{
	public class DroidRoundedBoxView : BoxRenderer
	{
		private float BorderWidth;
		private Android.Graphics.Color BorderColor;

		protected override void OnElementChanged (ElementChangedEventArgs<BoxView> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement != null || this.Element == null)
				return;

			SetWillNotDraw (false);
			Invalidate ();
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			Invalidate ();

		}

		public override void Draw (Canvas canvas)
		{
			var box = Element as RoundedBoxView;
			var rect = new Rect ();
			var paint = new Paint () {
				Color = box.BackgroundColor.ToAndroid (),
				AntiAlias = true,
			};

			GetDrawingRect (rect);

			var radius = (float)(rect.Width () / box.Width * box.CornerRadius);
			canvas.DrawRoundRect (new RectF (rect), radius, radius, paint);
		}
	}
}

