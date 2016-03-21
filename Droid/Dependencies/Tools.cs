using System;
using System.Net;
using Starving.Dependencies;
using Xamarin.Forms;
using Starving.Droid.Tools;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Geolocation;
using Android.Content;

[assembly:Dependency (typeof(Tools))]
namespace Starving.Droid.Tools
{
	public class Tools:ITools
	{
		public byte[] DownloadImageFromURL (string address)
		{
			var webClient = new WebClient ();
			byte[] bytesImage;
			try {
				bytesImage = webClient.DownloadData (address);
			} catch (Exception ex) {
				bytesImage = null;
			}
			return bytesImage;
		}

		public async Task<Dictionary<string, double>> GetCurrentLocation ()
		{			
			try {
				var locator = new Geolocator (MainActivity.AppContext) { DesiredAccuracy = 1000 };
				var position = await locator.GetPositionAsync (timeout: 100000);
				var dictionary = new Dictionary<string, double> ();
				dictionary.Add ("Latitude", position.Latitude);
				dictionary.Add ("Longitude", position.Longitude);
				return dictionary;
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
				return new Dictionary<string, double> ();
			}
		}

		public void OpenWaze (double latitude, double longitude)
		{
			try {
				var url = string.Format ("waze://?ll={0},{1}", latitude, longitude);
				Intent intent = new Intent (Intent.ActionView, Android.Net.Uri.Parse (url));
				Forms.Context.StartActivity (intent);
			} catch (ActivityNotFoundException ex) {
				Intent intent =
					new Intent (Intent.ActionView, Android.Net.Uri.Parse ("market://details?id=com.waze"));
				Forms.Context.StartActivity (intent);
			}
		}
	}
}

