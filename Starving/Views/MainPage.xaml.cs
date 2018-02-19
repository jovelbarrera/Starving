using Starving.Models;
using Xamarin.Forms;

namespace Starving.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Hack for non selected menu-item
            if (e.Item == null)
                return;

            var place = e.Item as Place;
            Application.Current.Resources["PlaceLatitud"] = place.Geometry.Location.Latitude;
            Application.Current.Resources["PlaceLongitude"] = place.Geometry.Location.Longitude;

            ((ListView)sender).SelectedItem = null;
        }
    }
}

