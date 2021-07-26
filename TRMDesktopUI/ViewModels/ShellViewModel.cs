using System;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		private SalesViewModel _salesVM;
		private IEventAggregator _events;
		private ILoggedInUserModel _user;
		private IApiHelper _apiHelper;

		public ShellViewModel(
			IEventAggregator events,
			SalesViewModel salesVM,
			ILoggedInUserModel user,
			IApiHelper apiHelper)
		{
			_events = events;
			_salesVM = salesVM;
			_user = user;
			_apiHelper = apiHelper;

			_events.SubscribeOnUIThread(this);
			
			ActivateItemAsync(IoC.Get<LoginViewModel>());
		}

		public bool IsLoggedIn
		{
			get
			{
				bool output = false;

				if (string.IsNullOrWhiteSpace(_user.Token) == false)
				{
					output = true;
				}

				return output;
			}
		}

		public void ExitApplication()
		{
			TryCloseAsync();
		}

		public void LogOut()
		{
			_user.ResetUser(); 
			_apiHelper.LogOffUser();
			ActivateItemAsync(IoC.Get<LoginViewModel>());
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(_salesVM);
			NotifyOfPropertyChange(() => IsLoggedIn);
		}
	}
}
