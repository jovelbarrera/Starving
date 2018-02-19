using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Starving.Models;
using Starving.Services;
using Starving.Views;
using Xamarin.Forms;

namespace Starving.ViewModels
{
    public class PlaceDetailPageViewModel : ViewModelBase
    {
        private Place _place;
        public Place Place
        {
            get { return _place; }
            set { SetProperty(ref _place, value); }
        }

        public PlaceDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var place = parameters["place"] as Place;
            Title = place.Name;
            Place = place;
        }
    }
}
