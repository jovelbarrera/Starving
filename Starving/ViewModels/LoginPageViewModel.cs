using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Starving.Droid.Helpers;
using Starving.Interfaces;
using Starving.Services.FirebaseServices;
using Xamarin.Forms;

namespace Starving.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private IFacebookCallback _facebookCallback;
        public IFacebookCallback CurrentFacebookCallback
        {
            get { return _facebookCallback; }
            set { SetProperty(ref _facebookCallback, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _isAwaiting;
        public bool IsAwaiting
        {
            get { return _isAwaiting; }
            set { SetProperty(ref _isAwaiting, value); }
        }

        public Command LoginCommand { get; set; }

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            IsAwaiting = true;
            CurrentFacebookCallback = new FacebookCallback
            {
                DialogService = DialogService,
                Callback = async (auth) => await LoginSuccessCallback(auth),
            };
            LoginCommand = new Command(async (o) => await Login(o));

            //TODO remove
            Email = "rej.barrera@gmail.com";
            Password = "123456";
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (!string.IsNullOrEmpty(Settings.CurrentUser))
            {
                IsAwaiting = true;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await RefreshToken();
                    await NavigationService.NavigateAsync("/NavigationPage/MainPage");
                });
            }
            else
            {
                IsAwaiting = false;
            }
        }

        private async Task Login(object e)
        {
            string firebaseApiKey = Application.Current.Resources["FirebaseApiKey"].ToString();
            var firebaseAuthProvider = new FirebaseAuthProvider(firebaseApiKey);
            try
            {
                await firebaseAuthProvider.LogIn(Email, Password, async (auth) => await LoginSuccessCallback(auth));
            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlertAsync("Login", "El usuario o clave no coinciden", "ACEPTAR");
            }
        }

        private async Task RefreshToken()
        {
            string firebaseApiKey = Application.Current.Resources["FirebaseApiKey"].ToString();
            string refreshToken = Settings.RefreshAuthToken;
            var firebaseAuthProvider = new FirebaseAuthProvider(firebaseApiKey);

            var token = await firebaseAuthProvider.RefreshToken(firebaseApiKey, refreshToken);
            Settings.AuthToken = token.AccessToken;
            Settings.RefreshAuthToken = token.RefreshToken;
        }

        #region Helpers

        public async Task LoginSuccessCallback(Firebase.Xamarin.Auth.FirebaseAuthLink auth)
        {
            if (auth == null || auth.User == null)
                throw new Exception("Unexpected Error");

            Settings.CurrentUser = JsonConvert.SerializeObject(auth.User);
            Settings.AuthToken = auth.FirebaseToken;
            Settings.RefreshAuthToken = auth.RefreshToken;
            await NavigationService.NavigateAsync("/NavigationPage/MainPage");
        }

        private class FacebookCallback : IFacebookCallback
        {
            public IPageDialogService DialogService { get; set; }
            public Action<Firebase.Xamarin.Auth.FirebaseAuthLink> Callback { get; set; }

            public void OnCancel()
            {
                throw new NotImplementedException();
            }

            public void OnError(Dictionary<string, object> error)
            {
                throw new NotImplementedException();
            }

            public void OnSuccess(Dictionary<string, object> result)
            {
                OnSuccessAsync(result["AccessToken"].ToString()).ConfigureAwait(false);
            }

            private async Task OnSuccessAsync(string facebookAccessToken)
            {
                string firebaseApiKey = Application.Current.Resources["FirebaseApiKey"].ToString();
                var firebaseAuthProvider = new FirebaseAuthProvider(firebaseApiKey);
                try
                {
                    await firebaseAuthProvider.LogIn(facebookAccessToken, Callback);
                }
                catch (Exception ex)
                {
                    await DialogService.DisplayAlertAsync("Login", "El usuario o clave no coinciden", "ACEPTAR");
                }
            }
        }

        #endregion
    }
}
