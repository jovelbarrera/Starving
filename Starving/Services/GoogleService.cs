using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Starving.Models;
using Xamarin.Forms;

namespace Starving.Services
{
    public class GoogleService
    {
        private static GoogleService _instance;

        public static GoogleService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GoogleService();
                }
                return _instance;
            }
        }

        private FlurlClient client;

        private string _baseUrl { get => string.Format(@"https://maps.googleapis.com/maps/api/place/"); }

        private string _apiKey { get => (string)Application.Current.Resources["GoogleApiKey"]; }

        public GoogleService()
        {
            client = new FlurlClient(_baseUrl);
        }

        public async Task<List<Place>> NearPlaces(double latitude, double longitude)
        {
            var lat = latitude.ToString().Replace(",", ".");
            var lon = longitude.ToString().Replace(",", ".");

            try
            {
                var response = await _baseUrl
                    .AppendPathSegment("search/json")
                    .SetQueryParams(new
                    {
                        location = lat + "," + lon,
                        rankby = "distance",
                        types = "bakery|bar|cafe|food|restaurant|shopping_mall",
                        key = _apiKey
                    }).GetJsonAsync<GoogleResponse>();
                return response?.Results;
            }
            catch (Exception e)
            {
                return new List<Place>();
            }
        }

        public async Task<Place> GetPlace(string placeId)
        {
            try
            {
                var response = await _baseUrl
                    .AppendPathSegment("details/json")
                    .SetQueryParams(new
                    {
                        placeid = placeId,
                        key = _apiKey
                    }).GetJsonAsync<Result>();
                return response.Place;
            }
            catch (Exception e)
            {
                return new Place();
            }
        }

        public async Task<byte[]> PlacePhoto(string photoReference)
        {
            //    string url = string.Format
            //(@"photo?maxwidth={0}&photoreference={1}&key={2}", 400, photoReference, _apiKey);
            //string address = client.BaseUrl + url;
            //var Tools = DependencyService.Get<ITools>();
            //var bytesImage = Tools.DownloadImageFromURL(address);
            //return bytesImage;

            try
            {
                var response = await _baseUrl
                    .AppendPathSegment("photo")
                    .SetQueryParams(new
                    {
                        maxwidth = 128,
                        photoreference = photoReference,
                        key = _apiKey
                    }).GetBytesAsync();
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}