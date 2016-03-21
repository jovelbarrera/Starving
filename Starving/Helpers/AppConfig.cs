using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Starving.Models;
using System.Linq;
using Starving.Services;
using System.IO;

namespace Starving
{
	public static class AppConfig
	{
		public static readonly string ApplicationName = AppResources.AppName;

		public static UriImageSource CacheImageSource (string url)
		{
			var source = new UriImageSource {
				Uri = new Uri (url),
//				CachingEnabled = true,
//				CacheValidity = new TimeSpan (5, 0, 0, 0)
			};
			return source;
		}

		public static async Task<ImageSource> FeaturedImage (Place place)
		{
			var imageSource = ImageSource.FromFile ("photo_placeholder.png");
			if (place.Photos != null && place.Photos.Length > 0) {
				Photos photo = place.Photos.FirstOrDefault ();

				if (photo != null) {			
					byte[] bytesImage = await GoogleService.Instance.PlacePhoto (photo.PhotoReference);
					if (bytesImage != null)
						imageSource = ImageSource.FromStream (() => new MemoryStream (bytesImage));
				}
			}
			return imageSource;
		}

		public static string HumanTimeDescription (DateTime createdAt, string verb)
		{
			var now = DateTime.Now.ToUniversalTime ();
			var substract = now - createdAt;

			if (substract.Seconds < 0)
				substract = now - now;
			
			if (substract.TotalSeconds < 60) {
				return AppResources.JustNow;
			} else if (substract.TotalSeconds < 60) {
				return substract.Seconds + " " + AppResources.SecondsAgo;
			} else if (substract.TotalMinutes < 60) {
				return substract.Minutes + " " + AppResources.MinutesAgo;
			} else if (substract.TotalHours < 24) {
				return substract.Hours + " " + AppResources.HoursAgo;
			} else if (substract.TotalDays <= 31) {
				return substract.Days + " " + AppResources.DaysAgo;
			} else {
				return verb + " " + createdAt.ToString ("MMMMM dd, yyyy");
			}
		}

		public static NavigationPage CreateNavigationPage (Page page)
		{
			NavigationPage navigationPage = new NavigationPage (page) {
				BarTextColor = Color.White,
				BarBackgroundColor = Styles.Colors.PrimaryColor,
			};
			return navigationPage;
		}
	}
}

