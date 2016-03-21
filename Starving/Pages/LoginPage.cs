using System;

using Xamarin.Forms;
using Starving.Dependencies;
using System.Threading.Tasks;
using Starving.Models;

namespace Starving.Pages
{
	public partial class LoginPage : ContentPage
	{
		private IParse _parseService;

		public LoginPage ()
		{
			_parseService = DependencyService.Get<IParse> ();
			InitializeComponents ();
			LoadData ();
		}

		private void LoadData ()
		{
			_descriptionLabel.Text = AppResources.AppDescription;
			_facebookButton.Text = AppResources.LoginWithFacebook;
		}

		private async void _facebookButton_Clicked (object sender, EventArgs e)
		{
			DisableControls ();
			try {
				IFacebookButton facebook = DependencyService.Get<IFacebookButton> ();
				facebook.LoginWithReadPermissions (new string[]{ "email" }, async (d) => {
					if (d == null) {
						EnableControls ();
						DisplayAlert (AppConfig.ApplicationName, "Facebook acount was not linked", "OK");
						return;
					}
					await _parseService.LoginWithFacebook (d.UserId, d.AccessToken, d.TokenExpiration, s => FacebookLoginCallback (s));
				});
			} catch (Exception ex) {
				string er = ex.Message;
			}
		}

		public async void FacebookLoginCallback (bool success)
		{
			if (success) {
				User currentUser = _parseService.GetCurrentUser ();
				App.Current.MainPage = new MainPage ();
			} else {
				EnableControls ();
				await DisplayAlert (AppConfig.ApplicationName, "Facebook acount was not linked", "OK");
			}
		}

		private void DisableControls ()
		{
			_facebookButton.IsEnabled = false;
		}

		private void EnableControls ()
		{
			_facebookButton.IsEnabled = true;
		}
	}
}


