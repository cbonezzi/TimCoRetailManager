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
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryData _inventoryData;

		public InventoryController(IInventoryData inventoryData)
		{
			_inventoryData = inventoryData;
		}

		[HttpGet]
		[Authorize(Roles = "Manager,Admin")] //this is an OR operation
		public List<InventoryModel> Get()
		{
			return _inventoryData.GetInventory();
		}
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public void Post(InventoryModel item)
		{
			_inventoryData.SaveInventoryRecord(item);
		}
	}
}
