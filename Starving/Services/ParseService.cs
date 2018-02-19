using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace Starving.Services
{
	public class ParseResponse<T>
	{
		public List<T> Results { get; set; }
		public int Count { get; set; }
	}

	public abstract class ParseService<I,T> where I : new()
	{
		private static I instance;

		public static I Instance {
			get { 
				if (instance == null) {
					instance = new I ();
				}

				return instance;
			}
		}

		public static string BaseUrl {
			get { 
				return "https://api.parse.com/1";
			}
		}

		public static Dictionary<string,string> Headers {
			get { 
				return new Dictionary<string,string> () {
					{ "X-Parse-Application-Id",AppKeys.ParseApplicationId },
					{ "X-Parse-REST-API-Key",AppKeys.ParseRESTAPIKey }
				};
			}
		}

		private static RestClient client;

		protected static RestClient Client {
			get { 
				if (client == null) {
					if (string.IsNullOrEmpty (BaseUrl)) {
						throw new ArgumentNullException ("BaseUrl");
					}

					client = new RestClient (BaseUrl);

					if (Headers != null) {
						foreach (var kvp in Headers) {
							client.AddDefaultParameter (kvp.Key, kvp.Value, ParameterType.HttpHeader);
						}
					}
				}

				return client;
			}
		}

		protected abstract string Resource { get; }

		public async virtual Task<T> Create (object parameters)
		{
			var request = CreateRequest (Resource, Method.POST);
			request.AddJsonBody (parameters);
			try {
				var response = await Client.Execute<T> (request);
				return response.Data;
			} catch (Exception e) {
				return default(T);
			}
		}

		public async Task<T> Read (string id)
		{
			RestRequest request = CreateRequest (Resource + "/" + id, Method.GET);
			try {
				var response = await ExecuteObjectQuery (request);
				return response;
			} catch (Exception e) {
				return default(T);
			}
		}

		public async Task<ParseResponse<T>> ReadAll ()
		{
			RestRequest request = CreateRequest (Resource, Method.GET);
			try {
				var response = await ExecuteCollectionQuery (request);
				return response;
			} catch (Exception e) {
				return default(ParseResponse<T>);
			}
		}

		public async Task<bool> Update (string id, T entity)
		{
			var request = CreateRequest (Resource + "/" + id, Method.PUT);
			request.AddJsonBody (entity);
			try {
				var response = await ExecuteAction (request);
				return true;
			} catch (Exception e) {
				return false;
			}
		}

		public async Task<bool> Delete (string id)
		{
			RestRequest request = CreateRequest (Resource + "/" + id, Method.DELETE);
			try {
				var response = await ExecuteAction (request);
				return true;
			} catch (Exception e) {
				return false;
			}
		}

		protected virtual async Task<ParseResponse<T>>  ExecuteQuery (Dictionary<string,object> parameters)
		{
			RestRequest request = CreateRequest (Resource, Method.GET, parameters);
			try {
				var response = await ExecuteCollectionQuery (request);
				return response;
			} catch (Exception e) {
				return new ParseResponse<T> ();
			}
		}

		protected virtual async Task<T> ExecuteObjectQuery (RestRequest request)
		{
			var response = await Client.Execute<T> (request);
			if (response.StatusCode == HttpStatusCode.OK) {
				return response.Data;
			} else {
				return default(T);
			}
		}

		protected virtual async Task<ParseResponse<T>> ExecuteCollectionQuery (RestRequest request)
		{
			var response = await Client.Execute<ParseResponse<T>> (request);
			if (response.StatusCode == HttpStatusCode.OK) {
				return response.Data;
			} else {
				return new ParseResponse<T> ();
			}
		}

		protected virtual async Task<bool> ExecuteAction (Method method, Dictionary<string,object> parameters)
		{
			RestRequest request = CreateRequest (Resource, method, parameters);
			var response = await ExecuteAction (request);
			return response;
		}

		protected virtual async Task<bool> ExecuteAction (RestRequest request)
		{
			try {
				var response = await Client.Execute (request);
				return response.StatusCode == HttpStatusCode.OK;
			} catch (Exception e) {
				return false;
			}
		}

		protected RestRequest CreateRequest (string resource, Method method,
		                                     Dictionary<string,object> parameters = null)
		{
			RestRequest request = new RestRequest (resource, method);
			if (parameters != null) {
				foreach (KeyValuePair<string,object> kvp in parameters) {
					request.AddParameter (kvp.Key, kvp.Value, ParameterType.GetOrPost);
				}
			}

			return request;
		}

		protected string CreateObjectParameters (T entity)
		{
			return JsonConvert.SerializeObject (entity);
		}
	}
}

