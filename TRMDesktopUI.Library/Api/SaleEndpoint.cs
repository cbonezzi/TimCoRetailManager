﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
	public class SaleEndpoint : ISaleEndpoint
	{
		private IApiHelper _apiHelper;

		public SaleEndpoint(
			IApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task PostSale(SaleModel sale)
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
			{
				if (response.IsSuccessStatusCode)
				{
					// Log successful call?
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
		//public async Task<List<ProductModel>> GetAll()
		//{
		//	using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
		//	{
		//		if (response.IsSuccessStatusCode)
		//		{
		//			var result = await response.Content.ReadAsAsync<List<ProductModel>>();
		//			return result;
		//		}
		//		else
		//		{
		//			throw new Exception(response.ReasonPhrase);
		//		}
		//	}
		//}
	}
}
