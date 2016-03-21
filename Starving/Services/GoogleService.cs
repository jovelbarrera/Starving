using RestSharp.Portable.HttpClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Starving.Models;
using RestSharp.Portable;
using System.Net.Http;
using Xamarin.Forms;
using Starving.Dependencies;


namespace Starving.Services
{
	public class GoogleService
	{
		private static GoogleService _instance;

		public static GoogleService Instance {
			get {
				if (_instance == null) {
					_instance = new GoogleService ();
				}
				return _instance;
			}
		}

		private RestClient client;

		public GoogleService ()
		{
			client = new RestClient ();
			client.BaseUrl = new Uri (string.Format (@"https://maps.googleapis.com/maps/api/place/"));
		}

		public async Task<List<Place>> NearPlaces (double latitude, double longitude)
		{
			var lat = latitude.ToString ().Replace (",", ".");
			var lon = longitude.ToString ().Replace (",", ".");
			var url = string.Format (@"search/json?location={0},{1}&rankby=distance&types=bakery|bar|cafe|" +
			          "food|restaurant|shopping_mall&key={2}", lat, lon, AppKeys.GoogleApiKey);

			var request = new RestRequest (url, Method.GET);
			try {
				var response = await client.Execute<GoogleResponse> (request);
				if (response.StatusCode == System.Net.HttpStatusCode.OK) {
					return response.Data.Results;
				} else {
					return new List<Place> ();
				}
			} catch (Exception e) {
				return new List<Place> ();
			}
		}

		public async Task<Place> GetPlace (string placeId)
		{
			var url = string.Format (@"details/json?placeid={0}&key={1}", placeId, AppKeys.GoogleApiKey);

			var request = new RestRequest (url, Method.GET);
			try {
				var response = await client.Execute<GoogleResponse> (request);
				if (response.StatusCode == System.Net.HttpStatusCode.OK) {
					return response.Data.Result;
				} else {
					return new Place ();
				}
			} catch (Exception e) {
				return new Place ();
			}
		}

		public async Task<byte[]>  PlacePhoto (string photoReference)
		{
			string url = string.Format (@"photo?maxwidth={0}&photoreference={1}&key={2}", 400, photoReference, AppKeys.GoogleApiKey);
			string address = client.BaseUrl + url;
			var Tools = DependencyService.Get<ITools> ();
			var bytesImage = Tools.DownloadImageFromURL (address);
			return bytesImage;
		}
	}
}