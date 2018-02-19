using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Starving.Dependencies;
using Starving.Models;
using Xamarin.Forms;
using System.Linq;

namespace Starving.Services
{
	public class RateService : ParseService<RateService,Rate>
	{
		#region implemented abstract members of ParseService

		protected override string Resource {
			get {
				return "classes/Rates";
			}
		}

		#endregion

		public async Task<List<Rate>> GetRates (Place place)
		{			
			Dictionary<string, object> parameters = new Dictionary<string, object> ();
			parameters.Add ("include", "user");
			string @where = string.Format (@"{{""placeId"":""{0}""}}", place.PlaceId);
			parameters.Add ("where", @where);
			ParseResponse<Rate> response = await ExecuteQuery (parameters);
			return response.Results;
		}

		public async Task<int> GetRatesNumber (Place place)
		{			
			Dictionary<string, object> parameters = new Dictionary<string, object> ();
			parameters.Add ("count", 1);
			string @where = string.Format (@"{{""placeId"":""{0}""}}", place.PlaceId);
			parameters.Add ("where", @where);
			ParseResponse<Rate> response = await ExecuteQuery (parameters);
			return response.Count;
		}

		public async Task<bool> IsRated (Place place, User user)
		{			
			Dictionary<string, object> parameters = new Dictionary<string, object> ();
			parameters.Add ("include", "user");
			parameters.Add ("order", "-createdAt");
			parameters.Add ("limit", 100);
			parameters.Add ("count", 1);

			string query = "{\"user\":{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"" + user.ObjectId + "\"},\"placeId\":\"" + place.PlaceId + "\"}";
			parameters.Add ("where", query);

			ParseResponse<Rate> response = await ExecuteQuery (parameters);
			return response.Count > 0;
		}

		public async Task<bool> CreateRate (Rate rate)
		{
			rate.User.__type = "Pointer";
			rate.User.ClassName = "_User";

			Dictionary<string,object> rateDict =
				new Dictionary<string,object> {
					{ "placeId",rate.Place.PlaceId }, { "user",
						new Dictionary<string,object> {
							{ "objectId", rate.User.ObjectId },
							{ "__type", rate.User.__type },
							{ "className", rate.User.ClassName }
						}
					}			
				};
			var response = await Create (rateDict);
			return response != null;
		}

		public async Task<bool> DeleteRate (Rate rate)
		{
			List<Rate> rates = await GetRates (rate.Place);
			Rate currentRate = rates.Where (x => x.User.ObjectId == rate.User.ObjectId).FirstOrDefault ();
			return await Delete (currentRate.ObjectId);
		}
	}
}

