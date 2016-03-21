using Android.App;
using Android.Content.PM;
using Xamarin.Facebook;
using Android.Content;
using Android.OS;

namespace Starving.Droid
{
	[Activity (Label = "Starving", Icon = "@drawable/ic_launcher", Theme = "@style/AppTheme",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static ICallbackManager callbackManager;
		public static Context AppContext;
		public static MainActivity Instance;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			ToolbarResource = Resource.Layout.toolbar;

			Parse.ParseClient.Initialize (AppKeys.ParseApplicationId, AppKeys.ParseDotNetKey);
			Xamarin.Facebook.FacebookSdk.SdkInitialize (this.ApplicationContext);
			Xamarin.Insights.Initialize (global::Starving.Droid.XamarinInsights.ApiKey, this);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			Xamarin.FormsMaps.Init (this, bundle);

			AppContext = this;
			Instance = this;
			callbackManager = CallbackManagerFactory.Create ();

			LoadApplication (new App ());
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			callbackManager.OnActivityResult (requestCode, (int)resultCode, data);
		}
	}
}
