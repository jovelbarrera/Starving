using System;
using Starving.Dependencies;
using Starving.Pages;
using Xamarin.Forms;
using Plugin.Connectivity;

namespace Starving
{
	public class App : Application
	{
		public App ()
		{
			if (Device.OS != TargetPlatform.WinPhone)
				AppResources.Culture = DependencyService.Get<ILocalize> ().GetCurrentCultureInfo ();

			var parse = DependencyService.Get<IParse> ();

			if (parse.IsLoggedIn ()) {
				parse.GetCurrentUser ();
				MainPage = new MainPage ();
			} else {
				MainPage = new LoginPage ();
			}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

