using Xamarin.Facebook;
using Newtonsoft.Json;
using Parse;
using System;
using Xamarin.Forms;
using Android.App;

namespace Starving.Droid.Dependencies
{
	public class FBGraphRequestCallback : Java.Lang.Object, Xamarin.Facebook.GraphRequest.ICallback
	{
		public Action<bool> Callback;

		public FBGraphRequestCallback (Action<bool> callback)
		{
			Callback = callback;
		}

		public async void OnCompleted (GraphResponse p0)
		{
			if (p0.Error == null) {
				string result = JsonConvert.SerializeObject (p0.RawResponse);
				FBUserData fbUserData = JsonConvert.DeserializeObject<FBUserData> (p0.RawResponse);

				Picture picture = fbUserData.Picture;
				string user_id = fbUserData.Id;
				string name = fbUserData.Name;
				string email = fbUserData.Email;

				try {
					string getName = ParseUser.CurrentUser.Get<string> ("name");
					var parseService = new DroidParseService ();
					if (!(getName != null || !string.IsNullOrEmpty (getName)))
						await parseService.GetFacebookUserData (name, email, picture.Data.Url);
				} catch (Exception e) {
					var parseService = new DroidParseService ();
					await parseService.GetFacebookUserData (name, email, picture.Data.Url);
				}
				Callback.Invoke (true);
			} else {
				AlertDialog.Builder alert;
				alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage (p0.Error.ErrorMessage);
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
			}
		}
	}

	public class FBUserData
	{
		public string Result { get; set; }

		public string Id { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public Picture Picture { get; set; }
	}

	public class Picture
	{
		public Data Data { get; set; }
	}

	public class Data
	{
		public string Url { get; set; }

		public bool Is_silhouette { get; set; }

		public string Width { get; set; }

		public string Height { get; set; }
	}
}

