using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Starving.Views
{
    public partial class PlaceDetailPage : ContentPage
    {
        public PlaceDetailPage()
        {
            InitializeComponent();
            map.Pins.Add(new Pin
            {
                Label = string.Empty,
                Position = new Position(double.Parse(Application.Current.Resources["PlaceLatitud"].ToString()),
                                        double.Parse(Application.Current.Resources["PlaceLongitude"].ToString()))
            });

            map.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(
                        double.Parse(Application.Current.Resources["PlaceLatitud"].ToString()),
                        double.Parse(Application.Current.Resources["PlaceLongitude"].ToString())),
                        Distance.FromMiles(1)
                )
            );
        }
    }
}