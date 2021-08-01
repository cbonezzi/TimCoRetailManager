using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
	public class UserData
	{
		private readonly IConfiguration _config;

		public UserData(IConfiguration config)
		{
			_config = config;
		}

		public List<UserModel> GetUserById(string Id)
		{
			SqlDataAccess sql = new SqlDataAccess(_config);

			//anonimous obj
			//this works on the same dll but not accross assemblies.
			var p = new { Id = Id };

			var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");

			return output;
		}
	}
}