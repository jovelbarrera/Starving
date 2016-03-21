using System;
using Java.Interop;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Starving.Droid.Dependencies;
using Starving.Dependencies;

[assembly:Dependency (typeof(DroidFacebookButton))]
namespace Starving.Droid.Dependencies
{
	public class DroidFacebookButton : IFacebookButton
	{
		private Action<FacebookEvent> callback;

		public DroidFacebookButton ()
		{
			MainActivity.callbackManager = CallbackManagerFactory.Create ();

			LoginManager.Instance.RegisterCallback (MainActivity.callbackManager, new FacebookCallback<LoginResult> {
				HandleSuccess = (result) => {
					var expires = result.AccessToken.Expires;
					var epoch = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					var expiration = epoch.AddMilliseconds (expires.Time);

					if (callback != null) {
						FacebookEvent data = new FacebookEvent () {
							AccessToken = result.AccessToken.Token,
							TokenExpiration = expiration,
							UserId = result.AccessToken.UserId
						};
						callback (data);
					} else {
						throw new Exception ("Login callback is not defined");
					}
				},
				HandleCancel = () => {
					if (callback != null) {
						callback (default(FacebookEvent));
					} else {
						throw new Exception ("Login callback is not defined");
					}
				},
				HandleError = ( error) => {
					if (callback != null) {
						callback (default(FacebookEvent));
					} else {
						throw new Exception ("Login callback is not defined");
					}
				}
			});
		}

		#region IFacebook implementation

		public void LoginWithReadPermissions (string[] permissions, Action<FacebookEvent> callback)
		{
			this.callback = callback;
			LoginManager.Instance.SetLoginBehavior (LoginBehavior.NativeWithFallback);
			LoginManager.Instance.LogInWithReadPermissions (MainActivity.Instance, permissions);
		}

		public void LoginWithWritePermissions (string[] permissions, Action<FacebookEvent> callback)
		{
			this.callback = callback;
			LoginManager.Instance.SetLoginBehavior (LoginBehavior.NativeWithFallback);
			LoginManager.Instance.LogInWithReadPermissions (MainActivity.Instance, permissions);
		}

		public void Logout ()
		{
			LoginManager.Instance.LogOut ();
		}

		#endregion
	}

	public class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
	{
		public Action HandleCancel { get; set; }

		public Action<FacebookException> HandleError { get; set; }

		public Action<TResult> HandleSuccess { get; set; }

		public void OnCancel ()
		{
			var handler = HandleCancel;
			if (handler != null)
				handler ();
		}

		public void OnError (FacebookException error)
		{
			var handler = HandleError;
			if (handler != null)
				handler (error);
		}

		public void OnSuccess (Java.Lang.Object result)
		{
			var handler = HandleSuccess;
			if (handler != null)
				handler (result.JavaCast<TResult> ());
		}
	}
}