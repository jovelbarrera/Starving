using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Starving.Models;
using Starving.Services;
using Starving.Views;
using Xamarin.Forms;

namespace Starving.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;
        private ObservableCollection<Place> _places;
        public ObservableCollection<Place> Places
        {
            get { return _places; }
            set { SetProperty(ref _places, value); }
        }

        public Command ItemTappedCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            Title = "Places";
            Places = new ObservableCollection<Place>();
            ItemTappedCommand = new Command((e) => OpenDetail(e));
            _dialogService = dialogService;
        }

        public void OpenDetail(object e)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("place", e as Place);
            NavigationService.NavigateAsync(nameof(PlaceDetailPage), parameters, false).ConfigureAwait(false);
        }

        public override void OnNavigatedTo(Prism.Navigation.NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadData().ConfigureAwait(false);
        }

        private async Task LoadData()
        {
            try
            {
                bool locationPermissionGranted = false;
                do
                {
                    locationPermissionGranted = await Utils.PermissionUtils.ManagePermissions(_dialogService, Permission.Location, "Search nearest places");
                } while (!locationPermissionGranted);

                Location location = await Utils.LocationUtils.GetCurrentLocation();

                Application.Current.Resources["CurrentLatitud"] = location.Latitude;
                Application.Current.Resources["CurrentLongitud"] = location.Longitude;

                var nearPlaces = await GoogleService.Instance.NearPlaces(location.Latitude, location.Longitude);
                nearPlaces.ForEach(p => Places.Add(p));

                //var place = await GoogleService.Instance.GetPlace(nearPlaces[0].PlaceId);
                //var placePhoto = await GoogleService.Instance.PlacePhoto(place.Photos[0].PhotoReference);

                foreach (var place in nearPlaces)
                {
                    if (place?.Photos != null && place.Photos.Length > 0)
                    {
                        var placePhoto = await GoogleService.Instance.PlacePhoto(place.Photos[0].PhotoReference);
                        place.PictureBytes = placePhoto;
                        int index = Places.IndexOf(place);
                        Places.Remove(place);
                        Places.Insert(index, place);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
    }
}