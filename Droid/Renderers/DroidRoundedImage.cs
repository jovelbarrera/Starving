using System;
using Android.Graphics;
using Starving.Controls;
using Starving.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof(RoundedImage), typeof(DroidRoundedImage))]
namespace Starving.Droid.Renderers
{
	public class DroidRoundedImage : ImageRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement == null) {
				if ((int)Android.OS.Build.VERSION.SdkInt < 18)
					SetLayerType (Android.Views.LayerType.Software, null);
			}
		}

		protected override bool DrawChild (Android.Graphics.Canvas canvas, Android.Views.View child, long drawingTime)
		{
			try {
				var element = (RoundedImage)Element;
				var radius = Math.Min (Width, Height) / 2;
				var strokeWidth = 10;
				radius -= strokeWidth / 2;
				var path = new Path ();
				path.AddCircle (Width / 2, Height / 2, radius, Path.Direction.Ccw);
				canvas.Save ();
				canvas.ClipPath (path);
				var result = base.DrawChild (canvas, child, drawingTime);
				canvas.Restore ();
				path = new Path ();
				path.AddCircle (Width / 2, Height / 2, radius, Path.Direction.Ccw);
				var paint = new Paint ();
				paint.AntiAlias = true;
				paint.StrokeWidth = (float)element.BorderWidth;
				paint.SetStyle (Paint.Style.Stroke);
				paint.Color = element.BorderColor.ToAndroid ();
				canvas.DrawPath (path, paint);
				paint.Dispose ();
				path.Dispose ();
				return result;
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
			return base.DrawChild (canvas, child, drawingTime);
		}
	}
}