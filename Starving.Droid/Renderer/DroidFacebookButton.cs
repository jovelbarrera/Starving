using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using Starving.Controls;
using Starving.Droid.Renderers;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookButton), typeof(DroidFacebookButton))]
namespace Starving.Droid.Renderers
{
    public class DroidFacebookButton : ViewRenderer
    {
        private LoginButton _loginButton;
        private FacebookCallback _facebookCallback;

        public DroidFacebookButton(Context context) : base(context)
        {
            var _activity = context as Activity;
            _loginButton = (LoginButton)_activity.LayoutInflater.Inflate(Resource.Layout.FacebookButton, this, false);

            _facebookCallback = new FacebookCallback();

            _loginButton.SetReadPermissions(new string[] { "email" });
            _loginButton.RegisterCallback(MainActivity.FacebookCallbackManager, _facebookCallback);

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;
            _facebookCallback.Element = Element as FacebookButton;
            AddView(_loginButton);
        }

        private class FacebookCallback : Java.Lang.Object, IFacebookCallback
        {
            public FacebookButton Element;

            public void OnCancel()
            {
                Element.FacebookCallback.OnCancel();
            }

            public void OnError(FacebookException error)
            {
                string json = JsonConvert.SerializeObject(error);
                Dictionary<string, object> e = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Element.FacebookCallback.OnError(e);
            }

            public void OnSuccess(Java.Lang.Object result)
            {
                var loginResult = result as LoginResult;
                var r = new Dictionary<string, object>
                {
                    {"AccessToken", loginResult.AccessToken.Token},
                    {"Permissions", loginResult.AccessToken.Permissions.ToArray()},
                    {"UserId", loginResult.AccessToken.UserId},
                };
                Element.FacebookCallback.OnSuccess(r);
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            _loginButton.Measure(msw, msh);
            _loginButton.Layout(0, 0, r - l, b - t);
        }
    }
}

