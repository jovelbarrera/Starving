using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Parse;
using Xamarin.Facebook;
using Xamarin.Forms;
using System.Collections.Generic;
using Starving.Models;
using Starving.Dependencies;
using Starving.Droid.Dependencies;

[assembly:Dependency (typeof(DroidParseService))]
namespace Starving.Droid.Dependencies
{
	public class DroidParseService : IParse
	{
		#region IParse implementation

		public bool IsLoggedIn ()
		{
			return ParseUser.CurrentUser != null;
		}

		public async Task<bool> Signup (User user, byte[] profilePicture)
		{
			try {
				var newUser = new ParseUser () {
					Email = user.Email,
					Username = user.Email,
					Password = user.Password,
				};
				newUser.Add ("name", user.Name);

				if (profilePicture != null) {
					ParseFile file = new ParseFile ("profilePicture.jpg", profilePicture);
					await file.SaveAsync ();
					newUser.Add ("profilePicture", file);
				}

				await newUser.SignUpAsync ();

			} catch (ParseException pex) {
				string exMessage;
				if (pex.Code == ParseException.ErrorCode.UsernameTaken) {
					exMessage = pex.Message;
				} else if (pex.Code == ParseException.ErrorCode.EmailTaken) {
					exMessage = pex.Message;
				} else {
					exMessage = pex.Message;
				}
				AlertDialog.Builder alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage (exMessage);
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
			} catch (InvalidOperationException inv) {
				AlertDialog.Builder alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage (inv.Message);
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
			} catch (Exception ex) {
				AlertDialog.Builder alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage (ex.Message);
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
			}

			return IsLoggedIn ();
		}

		public async Task<bool> Login (string username, string password)
		{
			try {
				await ParseUser.LogInAsync (username, password);
			} catch (InvalidOperationException inv) {
				AlertDialog.Builder alert;
				alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage (inv.Message);
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
			} catch (Exception ex) {
				AlertDialog.Builder alert;
				alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage (ex.Message);
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
			}
			return ParseUser.CurrentUser != null;
		}

		public async Task<bool> UpdateUser (Dictionary<string, object> parameters, byte[] profilePicture, byte[] coverPicture)
		{
			if (parameters != null) {
				foreach (var parameter in parameters) {
					ParseUser.CurrentUser [parameter.Key] = parameter.Value;
				}
			}
			if (profilePicture != null) {
				ParseFile file = new ParseFile ("profilePicture.jpg", profilePicture);
				await file.SaveAsync ();
				ParseUser.CurrentUser ["profilePicture"] = file;
			}
			if (coverPicture != null) {
				ParseFile file = new ParseFile ("coverPicture.jpg", coverPicture);
				await file.SaveAsync ();
				ParseUser.CurrentUser ["coverPicture"] = file;
			}

			try {
				await ParseUser.CurrentUser.SaveAsync ();
				return true;
			} catch (Exception e) {
				return false;
			}
		}


		public void Logout ()
		{
			ParseUser.LogOut ();
		}

		public async Task ResetPassword (string email)
		{
			try {
				await ParseUser.RequestPasswordResetAsync (email);
			} catch (Exception e) {
				return;
			}
		}

		public User GetCurrentUser ()
		{
			var user = new User ();
			user.ObjectId = ParseUser.CurrentUser.ObjectId;
			user.Email = ParseUser.CurrentUser.Email;

			try {
				user.Name = ParseUser.CurrentUser.Get<string> ("name");
			} catch (Exception e) {
				user.Name = "User";
			}
//			try {
//				user.Country = ParseUser.CurrentUser.Get<string> ("country");
//			} catch (Exception e) {
//				user.Country = string.Empty;
//			}
			try {
				var photo = ParseUser.CurrentUser.Get<ParseFile> ("profilePicture");
				user.ProfilePicture = new File (){ Url = photo.Url.ToString () };
			} catch (Exception e) {
			}

//			try {
//				var photo = ParseUser.CurrentUser.Get<ParseFile> ("coverPicture");
//				user.CoverPicture = new File (){ Url = photo.Url.ToString () };
//			} catch (Exception e) {
//				user.CoverPicture = user.ProfilePicture;
//			}

			return user;
		}

		public async Task LoginWithFacebook (string userId, string accessToken, DateTime tokenExpiration, Action<bool> callback)
		{			
			var user = await ParseFacebookUtils.LogInAsync (userId, accessToken, tokenExpiration);
			var permissions = AccessToken.CurrentAccessToken.Permissions.ToList ();
			var isEmailPermissionsGranted = permissions.Any (i => i == "email");
			if (!isEmailPermissionsGranted) {
				AlertDialog.Builder alert;
				alert = new AlertDialog.Builder (MainActivity.AppContext);
				alert.SetTitle (AppConfig.ApplicationName);
				alert.SetMessage ("Please accept all asked permissions for facebook to singup");
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
				});
				alert.Show ();
				callback.Invoke (false);
			}
			var parameters = new Bundle ();
			parameters.PutString ("fields", "email, name, picture.width(500).height(500)");

			var request = new GraphRequest (
				              AccessToken.CurrentAccessToken,
				              "/" + userId,
				              parameters,
				              HttpMethod.Get,
				              new FBGraphRequestCallback (callback)
			              ).ExecuteAsync ();
		}

		public async Task GetFacebookUserData (string name, string email, string picture)
		{
			var webClient = new WebClient ();
			var profilePicture = webClient.DownloadData (picture);

			ParseFile file = new ParseFile ("profilePicture.jpg", profilePicture);
			await file.SaveAsync ();
			ParseUser.CurrentUser ["profilePicture"] = file;

			ParseUser.CurrentUser ["email"] = email;
			ParseUser.CurrentUser ["name"] = name;

			await ParseUser.CurrentUser.SaveAsync ();
		}

		#endregion
	}
}