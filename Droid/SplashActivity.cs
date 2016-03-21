using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Android.Views;
using System.Threading;

namespace Starving.Droid
{
	[Activity (Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Xamarin.Insights.Initialize (global::Starving.Droid.XamarinInsights.ApiKey, this);
			/*var mainLayout = new RelativeLayout (this);

			ImageView imageView = new ImageView (this);
			imageView.SetImageResource (Resource.Drawable.ic_starving);
			RelativeLayout.LayoutParams imageViewParam = new RelativeLayout.LayoutParams (
				                                             RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
			imageViewParam.AddRule (LayoutRules.CenterInParent);
			mainLayout.AddView (imageView, imageViewParam);

			SetContentView (mainLayout);*/
			base.OnCreate (bundle);
			StartActivity (typeof(MainActivity));
		}
	}
}
