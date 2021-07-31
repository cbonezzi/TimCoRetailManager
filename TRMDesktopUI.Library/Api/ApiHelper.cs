using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
	public class ApiHelper : IApiHelper
	{
		public HttpClient _apiClient { get; set; }
		
		//this is an interesting way of mapping without using any additional tools, such as automapper
		private ILoggedInUserModel _loggedInUser;

		public ApiHelper(ILoggedInUserModel loggedInUser)
		{
			InitializeClient();
			_loggedInUser = loggedInUser;
		}

		//this will be a readonly property of the HttpClient
		//that will be available for the application to use
		public HttpClient ApiClient
		{
			get
			{
				return _apiClient;
			}
		}

		private void InitializeClient()
		{
			string api = ConfigurationManager.AppSettings["api"];

			_apiClient = new HttpClient();
			_apiClient.BaseAddress = new Uri(api);
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<AuthenticatedUser> Authenticate(string username, string password)
		{
			var data = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "password"),
				new KeyValuePair<string, string>("username", username),
				new KeyValuePair<string, string>("password", password)
			});

			using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
			{
				if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

				var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
				
				return result;

			}
		}

		public void LogOffUser()
		{
			_apiClient.DefaultRequestHeaders.Clear();
		}

		public async Task GetLoggedInUserInfo(string token)
		{
			//for every call that we make add the bearer token to the header.
			_apiClient.DefaultRequestHeaders.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

			using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
					_loggedInUser.CreatedDate = result.CreatedDate;
					_loggedInUser.EmailAddress = result.EmailAddress;
					_loggedInUser.FirstName = result.FirstName;
					_loggedInUser.Id = result.Id;
					_loggedInUser.LastName = result.LastName;
					_loggedInUser.Token = token;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
