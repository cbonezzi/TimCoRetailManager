using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class UserDisplayViewModel : Screen
	{
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;
		private readonly IUserEndpoint _userEndpoint;

		private BindingList<UserModel> _users;

		public BindingList<UserModel> Users
		{
			get
			{
				return _users;
			}
			set
			{
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		private UserModel _selectedUser;

		public UserModel SelectedUser
		{
			get { return _selectedUser; }
			set
			{
				_selectedUser = value;
				SelectedUserName = value.Email;
				SelectedUserRoles.Clear();

				//creates a new binding list of strings which will hold the role name
				SelectedUserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
				_ = LoadRoles();

				NotifyOfPropertyChange(() => SelectedUser);
			}
		}

		private string _selectedUserName;

		public string SelectedUserName
		{
			get
			{
				return _selectedUserName;
			}
			set
			{
				_selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}

		private BindingList<string> _selectedUserRoles = new BindingList<string>();

		public BindingList<string> SelectedUserRoles
		{
			get
			{
				return _selectedUserRoles;
			}
			set
			{
				_selectedUserRoles = value;
				NotifyOfPropertyChange(() => SelectedUserRoles);
			}
		}

		private BindingList<string> _availableRoles = new BindingList<string>();

		public BindingList<string> AvailableRoles
		{
			get
			{
				return _availableRoles;
			}
			set
			{
				_availableRoles = value;
				NotifyOfPropertyChange(() => AvailableRoles);
			}
		}

		private string _selectedUserRole;

		public string SelectedUserRole
		{
			get { return _selectedUserRole; }
			set
			{
				_selectedUserRole = value;
				NotifyOfPropertyChange(() => SelectedUserRole);
			}
		}

		private string _selectedAvailableRole;

		public string SelectedAvailableRole
		{
			get { return _selectedAvailableRole; }
			set
			{
				_selectedAvailableRole = value;
				NotifyOfPropertyChange(() => SelectedAvailableRole);
			}
		}



		public UserDisplayViewModel(
			StatusInfoViewModel status,
			IWindowManager window,
			IUserEndpoint userEndpoint)
		{
			_status = status;
			_window = window;
			_userEndpoint = userEndpoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			try
			{
				await LoadUsers();
			}
			catch (Exception ex)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;
				settings.Title = "System Error";

				if (ex.Message == "Unauthorized")
				{
					_status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales Form");
					await _window.ShowDialogAsync(_status, null, settings);
				}
				else
				{
					_status.UpdateMessage("Fatal Exception", ex.Message);
					await _window.ShowDialogAsync(_status, null, settings);
				}

				//may not need to be awaited
				await TryCloseAsync();
			}
		}

		private async Task LoadUsers()
		{
			var userList = await _userEndpoint.GetAll();
			Users = new BindingList<UserModel>(userList);
		}

		private async Task LoadRoles()
		{
			var roleList = await _userEndpoint.GetAllRoles();
			foreach (var role in roleList)
			{
				if (SelectedUserRoles.IndexOf(role.Value) < 0)
				{
					AvailableRoles.Add(role.Value);
				}
			}
		}

		public async void AddSelectedRole()
		{
			try
			{
				await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);
				SelectedUserRoles.Add(SelectedAvailableRole);
				AvailableRoles.Remove(SelectedAvailableRole);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async void RemoveSelectedRole()
		{
			try
			{
				await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);
				AvailableRoles.Add(SelectedUserRole);
				SelectedUserRoles.Remove(SelectedUserRole);
			}
			catch (Exception)
			{

			}
		}
	}
}
