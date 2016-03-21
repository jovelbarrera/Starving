using System;

using Xamarin.Forms;
using Starving.Helpers;

namespace Starving.Pages
{
	public partial class PlacesPage
	{
		private RelativeLayout _mainLayout;
		private RelativeLayout _loadingLayoutPlaces;
		private ListView _placesListView;

		public void InitializeComponents ()
		{
			Title = AppConfig.ApplicationName;
			//Icon = "drawer_button.png";
			_mainLayout = new RelativeLayout {
				BackgroundColor = Styles.Colors.PlaceholderColor,
			};

			_placesListView = new ListView {
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(PlaceViewCell)),
				IsPullToRefreshEnabled = true,
				//Header = listTitle,
			};
			_placesListView.SeparatorVisibility = SeparatorVisibility.None;
			_placesListView.ItemTapped += _places_ItemTapped;
			_placesListView.Refreshing += ListView_Refreshing;
			_mainLayout.Children.Add (_placesListView,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height)
			);

			_loadingLayoutPlaces = DynamicsLayouts.Loading (AppResources.RetrievingPlaces, Styles.Colors.PlaceholderColor);
			_mainLayout.Children.Add (_loadingLayoutPlaces,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height)
			);

			Content = _mainLayout;
		}
	}
}


