using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public class ProductData : IProductData
	{
		private readonly IConfiguration _config;
		private readonly ISqlDataAccess _sql;

		public ProductData(IConfiguration config, ISqlDataAccess sql)
		{
			_config = config;
			_sql = sql;
		}

		public List<ProductModel> GetProducts()
		{
			var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");

			return output;
		}

		public ProductModel GetProductById(int productId)
		{
			var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();

			return output;
		}
	}
}
