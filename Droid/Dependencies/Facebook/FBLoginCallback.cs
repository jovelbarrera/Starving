using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Starving.Helpers;

namespace Starving.Droid.Dependencies
{
	public class FBLoginCallback : Java.Lang.Object, Xamarin.Facebook.IFacebookCallback
	{
		public void OnCancel ()
		{
			//throw new NotImplementedException ();
		}

		public void OnError (FacebookException p0)
		{
			//throw new NotImplementedException ();
		}

		public void OnSuccess (Java.Lang.Object p0)
		{
			var result = (LoginResult)p0;
			var token = result.AccessToken;

			var tokenString = token.Token;
			Settings.FacebookTokenString = tokenString;
			var fbId = token.UserId;
			Settings.FacebookUserId = fbId;
		}
	}
}

