using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductController : ControllerBase
	{
		private readonly IConfiguration _config;

		public ProductController(IConfiguration config)
		{
			_config = config;
		}

		// GET api/values
		public List<ProductModel> Get()
		{
			ProductData data = new ProductData(_config);

			return data.GetProducts();
		}
	}
}
