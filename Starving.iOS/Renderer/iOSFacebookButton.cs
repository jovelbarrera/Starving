using System;
using Facebook.LoginKit;
using Starving.Controls;
using Starving.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FacebookButton), typeof(iOSFacebookButton))]
namespace Starving.iOS.Renderers
{
    public class iOSFacebookButton : ViewRenderer
    {
        private LoginButton _loginButton;

        public iOSFacebookButton()
        {
            _loginButton = new LoginButton(Frame);
            _loginButton.ReadPermissions = new string[] { "email" };
            _loginButton.Center = Center;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;
            AddSubview(_loginButton);
        }

        //protected override void OnLayout(bool changed, int l, int t, int r, int b)
        //{
        //    base.OnLayout(changed, l, t, r, b);

        //    var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
        //    var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

        //    _loginButton.Measure(msw, msh);
        //    _loginButton.Layout(0, 0, r - l, b - t);
        //}
    }
}

