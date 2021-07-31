﻿using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
	public class UserController : ApiController
	{
		[HttpGet]
		public UserModel GetById()
		{
			string userId = RequestContext.Principal.Identity.GetUserId();

			//this is a dependency think about using DI
			UserData data = new UserData();

			return data.GetUserById(userId).First();
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		[Route("api/User/Admin/GetAllUsers")]
		public List<ApplicationUserModel> GetAllUsers()
		{
			List<ApplicationUserModel> output = new List<ApplicationUserModel>();

			using (var context = new ApplicationDbContext())
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);

				var users = userManager.Users.ToList();
				var roles = context.Roles.ToList();

				foreach (var user in users)
				{
					ApplicationUserModel u = new ApplicationUserModel()
					{
						Id = user.Id,
						Email = user.Email
					};

					foreach (var r in user.Roles)
					{
						u.Roles.Add(r.RoleId, roles.Where(x=>x.Id == r.RoleId).First().Name);
					}

					output.Add(u);
				}
			}

			return output;
		}
	}
}
