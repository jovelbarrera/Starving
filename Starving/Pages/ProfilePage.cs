using System;
using Starving.Dependencies;
using Starving.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Starving.Services;
using System.Collections.Generic;
using System.Linq;

namespace Starving.Pages
{
	public partial class ProfilePage:ContentPage
	{
		private ObservableCollection<Review> _reviewsCollection;

		public ProfilePage ()
		{
			_reviewsCollection = new ObservableCollection<Review> ();
			InitializeComponents ();
			LoadData ().ConfigureAwait (false);
		}

		private async Task LoadData ()
		{
			var parse = DependencyService.Get<IParse> ();
			User currentUser = parse.GetCurrentUser ();
			if (currentUser.ProfilePicture != null && !string.IsNullOrEmpty (currentUser.ProfilePicture.Url)) {
				_picture.Source = AppConfig.CacheImageSource (currentUser.ProfilePicture.Url);
			}
			_nameLabel.Text = currentUser.Name;
			_latestReviewsTitleLabel.Text = AppResources.MyLatestReviews;

			List<Review> reviews = await ReviewsService.Instance.GetUserReviews (currentUser);
			_mainLayout.Children.Remove (_reviewsPlaceholderLayout);
			if (reviews == null || reviews.Count == 0) {
				_emptyReviews.Text = AppResources.NoReviews;
				_emptyReviews.IsVisible = true;
				ReviewsList.IsVisible = false;
			} else {
				_emptyReviews.IsVisible = false;
				ReviewsList.IsVisible = true;
				foreach (var review in reviews) {
					if (!_reviewsCollection.Any (x => x.ObjectId == review.ObjectId))
						_reviewsCollection.Add (review);
				}
			}
		}
	}
}


