﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SaleController : ControllerBase
	{
		private readonly IConfiguration _config;

		public SaleController(IConfiguration config)
		{
			_config = config;
		}

		[Authorize(Roles = "Cashier")]
		[HttpGet]
		public void Post(SaleModel sale)
		{
			SaleData data = new SaleData(_config);

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			data.SaveSale(sale, userId);
		}

		[Authorize(Roles = "Admin,Manager")]
		[Route("GetSalesReport")]
		[HttpGet]
		public List<SaleReportModel> GetSalesReport()
		{
			SaleData data = new SaleData(_config);
			return data.GetSaleReport();
		}
	}
}
