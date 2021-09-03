using System.Configuration;

namespace TRMDataManager.Library
{
	public class ConfigHelper
	{
		// TODO: Move this from config to the API
		public static decimal GetTaxRate()
		{
			string rateText = ConfigurationManager.AppSettings["taxRate"];

			bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);

			if (IsValidTaxRate is false)
			{
				throw new ConfigurationErrorsException("The tax rate is not set up properly");
			}

			return output;
		}
	}
}
