using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Starving.Models;

namespace Starving.Utils
{
    public static class LocationUtils
    {
        public static async Task<Location> GetCurrentLocation()
        {
            try
            {
                var results = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                return new Location
                {
                    Latitude = results.Latitude,
                    Longitude = results.Longitude
                };
            }
            catch (Exception ex)
            {
                Position lastKnownLocation = await CrossGeolocator.Current.GetLastKnownLocationAsync();
                return new Location
                {
                    Latitude = lastKnownLocation.Latitude,
                    Longitude = lastKnownLocation.Longitude
                };
            }
        }
    }
}
