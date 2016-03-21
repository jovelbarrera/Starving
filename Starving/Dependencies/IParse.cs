using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Starving.Models;

namespace Starving.Dependencies
{
	public interface IParse
	{
		bool IsLoggedIn ();

		Task<bool> Signup (User user, byte[] profilePicture);

		Task<bool> Login (string username, string password);

		Task<bool> UpdateUser (Dictionary<string, object> parameters, byte[] profilePicture, byte[] coverPicture);

		void Logout ();

		Task ResetPassword (string email);

		User GetCurrentUser ();

		Task LoginWithFacebook (string userId, string accessToken, DateTime tokenExpiration, Action<bool> callback);

		Task GetFacebookUserData (string name, string email, string picture);
	}
}