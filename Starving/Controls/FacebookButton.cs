using System;
using Starving.Interfaces;
using Xamarin.Forms;

namespace Starving.Controls
{
    public class FacebookButton : View
    {
        public static BindableProperty FacebookCallbackProperty =
            BindableProperty.Create(nameof(FacebookCallback),
                                    typeof(IFacebookCallback),
                                    typeof(FacebookButton),
                                    default(IFacebookCallback));
        public IFacebookCallback FacebookCallback
        {
            get { return (IFacebookCallback)GetValue(FacebookCallbackProperty); }
            set
            {
                SetValue(FacebookCallbackProperty, value);
            }
        }
    }
}


