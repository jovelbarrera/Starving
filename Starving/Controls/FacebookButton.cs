using System;
using Xamarin.Forms;

namespace Starving.Controls
{
	public class FacebookEventArgs : EventArgs
	{
		public string UserId { get; set; }
		public string AccessToken { get; set; }
		public DateTime TokenExpiration { get; set; }
	}

	public class FacebookButton : Button
	{
		public event Action<object,FacebookEventArgs> OnLogin;

		public void Login( object sender, FacebookEventArgs args ) 
		{
			if( OnLogin != null ) {
				OnLogin ( sender, args );
			}	
		}
	}
}

