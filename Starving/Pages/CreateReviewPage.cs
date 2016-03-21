using System;
using System.Collections.Generic;
using Starving.Models;
using Starving.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Starving.Dependencies;

namespace Starving.Pages
{
	public partial class CreateReviewPage : ContentPage
	{
		private Place _place;
		private Action _updatePlaceDetail;

		public CreateReviewPage (Place place, Action updatePlaceDetail)
		{
			_place = place;
			_updatePlaceDetail = updatePlaceDetail;
			InitializeComponents ();
			LoadData ().ConfigureAwait (false);
		}

		public async Task LoadData ()
		{
			_questionLabel.Text = AppResources.HaveBeenInThisPlace + " " + _place.Name + "?";
		}

		async void PostReviewButton_Clicked (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (_commentEditor.Text)) {
				DisplayAlert (AppResources.AppName, AppResources.ReviewFieldEmpty, AppResources.OK);
				return;
			}
			var parse = DependencyService.Get<IParse> ();
			User currentUser = parse.GetCurrentUser ();

			var review = new Dictionary<string,object> () { {
					"user",new Dictionary<string,object> () {
						{ "__type","Pointer" },
						{ "className","_User" },
						{ "objectId",currentUser.ObjectId }
					}
				},
				{ "comment",_commentEditor.Text },
				{ "placeId",_place.PlaceId }
			};

			var response = await ReviewsService.Instance.CreateReview (review);
			if (response != null) {
				_updatePlaceDetail.Invoke ();
				Navigation.PopAsync ();
			} else {
				// TODO handle when not sent
			}
		}

		void _commentEditor_TextChanged (object sender, TextChangedEventArgs e)
		{
			var editor = (Editor)sender;
			if (e.NewTextValue.Length > 100)
				editor.Text = e.OldTextValue;
		}
	}
}


