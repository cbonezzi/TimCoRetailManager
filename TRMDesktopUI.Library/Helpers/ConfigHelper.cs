using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Helpers
{
	// this will eventually be a singleton and it will come from the appsettings.
	//public class ConfigHelper : IConfigHelper
	//{
	//	// TODO: Move this from config to the API
	//	public decimal GetTaxRate()
	//	{
	//		string rateText = ConfigurationManager.AppSettings["taxRate"];

	//		bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);

	//		if (IsValidTaxRate is false)
	//		{
	//			throw new ConfigurationErrorsException("The tax rate is not set up properly");
	//		}

	//		return output;
	//	}
	//}
}
