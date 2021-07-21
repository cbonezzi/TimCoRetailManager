using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
	public class UserController : ApiController
	{
		public UserModel GetById()
		{
			string userId = RequestContext.Principal.Identity.GetUserId();

			//this is a dependency think about using DI
			UserData data = new UserData();

			return data.GetUserById(userId).First();
		}
	}
}
