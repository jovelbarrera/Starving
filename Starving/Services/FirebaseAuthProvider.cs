using System;
using System.Threading.Tasks;
using Starving.Services.FirebaseServices;
using Firebase.Xamarin.Auth;
using Flurl;
using Flurl.Http;
using Xamarin.Forms;

namespace Starving.Services.FirebaseServices
{
    public class FirebaseAuthProvider
    {
        private Firebase.Xamarin.Auth.FirebaseAuthProvider authProvider;

        public FirebaseAuthProvider(string firebaseApiKey)
        {
            var config = new FirebaseConfig(firebaseApiKey);
            authProvider = new Firebase.Xamarin.Auth.FirebaseAuthProvider(config);
        }

        public async Task LogIn(string email, string password, Action<FirebaseAuthLink> callback)
        {
            FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            callback(auth);
        }

        public async Task LogIn(string facebookAccessToken, Action<FirebaseAuthLink> callback)
        {
            FirebaseAuthLink auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, facebookAccessToken);
            callback(auth);
        }

        public async Task SignUp(string email, string password, Action<FirebaseAuthLink> callback)
        {
            FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            callback(auth);
        }

        public async Task ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
        }

        public async Task<FirebaseToken> RefreshToken(string apikey, string refreshToken)
        {
            //var query = new FirebaseQuery();
            //query.Add("key", apikey);
            ////string resource = string.Format("?key={0}", apikey);
            //var data = new Dictionary<string, object>
            //{
            //    {"grant_type","refresh_token"},
            //    {"refresh_token",refreshToken},
            //};
            //GenericResponse<FirebaseToken> result = await Execute<FirebaseToken, Dictionary<string, object>>(Resource, HttpMethod.Post, data, query);
            //return result.Model;

            try
            {
                var response = await Application.Current.Resources["FirebaseRefreshTokenUrl"].ToString()
                                                .SetQueryParam("key", apikey)
                                                .SetQueryParam("grant_type", "refresh_token")
                                                .SetQueryParam("refresh_token", refreshToken)
                                                .PostAsync(null)
                                                .ReceiveJson<FirebaseToken>();
                return response;
            }
            catch (Exception e)
            {
                return new FirebaseToken();
            }
        }
    }
}

