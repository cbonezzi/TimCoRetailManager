using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public class SaleData : ISaleData
	{
		private readonly IProductData _productData;
		private readonly ISqlDataAccess _sql;

		public SaleData(IConfiguration config, IProductData productData, ISqlDataAccess sql)
		{
			_productData = productData;
			_sql = sql;
		}

		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			// TODO - Make this SOLID/DRY/Better
			List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

			// TODO - make this better
			var taxRate = ConfigHelper.GetTaxRate() / 100;

			foreach (var item in saleInfo.SaleDetails)
			{
				var detail = new SaleDetailDBModel
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity
				};

				// get the information about this product
				var productInfo = _productData.GetProductById(detail.ProductId);

				if (productInfo is null)
				{
					throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
				}

				detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

				if (productInfo.IsTaxable)
				{
					detail.Tax = (detail.PurchasePrice * taxRate);
				}

				details.Add(detail);
			}

			// create the sale model
			SaleDBModel sale = new SaleDBModel
			{
				SubTotal = details.Sum(x => x.PurchasePrice),
				Tax = details.Sum(x => x.Tax),
				CashierId = cashierId
			};

			sale.Total = sale.SubTotal + sale.Tax;

			// save the sale model
			try
			{
				_sql.StartTransaction("TRMData");

				// Save the sale model
				_sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

				// get the ID from the sale model
				sale.Id = _sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { CashierId = cashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

				// finish filling in the sale detail models
				foreach (var item in details)
				{
					item.SaleId = sale.Id;

					// Save the sale detail models
					_sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
				}

				_sql.CommitTransaction();
			}
			catch
			{
				_sql.RollbackTransaction();
				throw;
			}
		}

		public List<SaleReportModel> GetSaleReport()
		{
			var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");

			return output;
		}
	}
}
