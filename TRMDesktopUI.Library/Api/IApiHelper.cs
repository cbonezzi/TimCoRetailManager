﻿using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
	public interface IApiHelper
	{
		HttpClient ApiClient { get; }
		void LogOffUser();
		Task<AuthenticatedUser> Authenticate(string username, string password);
		Task GetLoggedInUserInfo(string token);
	}
}