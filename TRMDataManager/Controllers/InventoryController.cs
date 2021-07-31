﻿using System.Collections.Generic;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
	public class InventoryController : ApiController
	{
		[Authorize(Roles = "Manager,Admin")] //this is an OR operation
		public List<InventoryModel> Get()
		{
			InventoryData data = new InventoryData();
			return data.GetInventory();
		}

		[Authorize(Roles = "Admin")]
		public void Post(InventoryModel item)
		{
			InventoryData data = new InventoryData();
			data.SaveInventoryRecord(item);
		}
	}
}
