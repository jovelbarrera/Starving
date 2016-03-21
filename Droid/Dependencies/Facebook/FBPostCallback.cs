using Xamarin.Facebook;
using Xamarin.Forms;

namespace Starving.Droid.Dependencies
{
	public class FBPostCallback : Java.Lang.Object, Xamarin.Facebook.GraphRequest.ICallback
	{
		public async void OnCompleted (GraphResponse p0)
		{
			if (p0.Error == null) {
				// TODO messagin center
				//MessagingCenter.Send<FBPostCallback, bool> (this, "FacebookDataRetriveDone", true);
			}				
		}
	}
}

