using System;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Kadevjo.Droid.Dependencies;
using Starving.Dependencies;
using Starving.Helpers;
using Starving.Droid;
using Starving.Droid.Dependencies;

[assembly:Dependency (typeof(FacebookService))]
namespace Kadevjo.Droid.Dependencies
{
	public class FacebookService:IFacebook
	{
		public async Task<bool> ExistFacebookAccount ()
		{
			try {
				if (!string.IsNullOrEmpty (Settings.FacebookTokenString))
					return true;
				else
					return false;
			} catch (Exception ex) {
				return false;
			}
		}

		public async Task<bool> CreateFacebookAccount ()
		{
			if (!await ExistFacebookAccount ()) {
				var readingPermissions = new string[] { "public_profile", "email" };
				var publishPermissions = new string[] { "publish_actions" };
				var loginManager = LoginManager.Instance;

				MainActivity.callbackManager = CallbackManagerFactory.Create ();

				loginManager.LogInWithReadPermissions (MainActivity.Instance, readingPermissions);
				loginManager.LogInWithPublishPermissions (MainActivity.Instance, publishPermissions);
				loginManager.RegisterCallback (MainActivity.callbackManager, new FBLoginCallback ());
				return true;
			} else {
				return false;
			}
		}

		public async Task<bool> PostOnFacebook (string message)
		{
			if (!await ExistFacebookAccount ())
				return false;

			if (!string.IsNullOrEmpty (AccessToken.CurrentAccessToken.Token)) {
				try {
					var tokenString = Settings.FacebookTokenString;
					var fbId = Settings.FacebookUserId;
					var response = FacebookPosting (message, tokenString, fbId);
					return response;
				} catch (Exception ex) {
					return false;
				}
			} else {				
				var permissions = AccessToken.CurrentAccessToken.Permissions.ToList ();
				var permissionsGranted = permissions.Any (i => i == "publish_actions");

				if (permissionsGranted) {
					var response = FacebookPosting (message);
					return response;
				}
			}
			return false;
		}

		public bool FacebookPosting (string message, string tokenString = null, string fbId = null)
		{
			var path = "/" + fbId + "/feed";

			var parameters = new Bundle ();
			parameters.PutString ("message", message);

			var request = new GraphRequest (
				              AccessToken.CurrentAccessToken,
				              path,
				              parameters,
				              HttpMethod.Post,
				              new FBPostCallback ()
			              ).ExecuteAsync ();
			return true;
		}
	}
}

