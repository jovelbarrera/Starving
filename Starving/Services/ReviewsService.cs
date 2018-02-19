using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Starving.Dependencies;
using Starving.Models;
using Xamarin.Forms;

namespace Starving.Services
{
	public class ReviewsService : ParseService<ReviewsService,Review>
	{
		#region implemented abstract members of ParseService

		protected override string Resource {
			get {
				return "classes/Reviews";
			}
		}

		#endregion

		public async Task<List<Review>> GetReviews (string placeId)
		{			
			Dictionary<string, object> parameters = new Dictionary<string, object> ();

			parameters.Add ("include", "user");
			parameters.Add ("order", "-createdAt");
			parameters.Add ("limit", 100);

			var parse = DependencyService.Get<IParse> ();
			User currentUser = parse.GetCurrentUser ();
			//string query = "{\"user\":{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"" + currentUser.ObjectId + "\"},\"placeId\":\"" + placeId + "\"}";
			parameters.Add ("where", "{\"placeId\":\"" + placeId + "\"}");

			ParseResponse<Review> response = await ExecuteQuery (parameters);
			return response.Results;
		}

		public async Task<List<Review>> GetUserReviews (User user)
		{			
			Dictionary<string, object> parameters = new Dictionary<string, object> ();

			parameters.Add ("include", "user");
			parameters.Add ("order", "-createdAt");
			parameters.Add ("limit", 100);

			string @where = "{\"user\":{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"" + user.ObjectId + "\"} }";
			parameters.Add ("where", @where);

			ParseResponse<Review> response = await ExecuteQuery (parameters);
			return response.Results;
		}

		public async Task<Review> CreateReview (Dictionary<string,object> review)
		{
			return await Create (review);
		}
	}
}

