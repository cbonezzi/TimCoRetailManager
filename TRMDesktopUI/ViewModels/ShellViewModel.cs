using System;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		private SalesViewModel _salesVM;
		private IEventAggregator _events;
		private ILoggedInUserModel _user;

		public ShellViewModel(
			//LoginViewModel loginVM,
			IEventAggregator events,
			SalesViewModel salesVM,
			ILoggedInUserModel user)
		{
			_events = events;
			_salesVM = salesVM;
			_user = user;

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
			_user.LogOffUser(); 
			ActivateItemAsync(IoC.Get<LoginViewModel>());
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(_salesVM);
			NotifyOfPropertyChange(() => IsLoggedIn);

			//wiping out the loginVM
			//_loginVM = _container.GetInstance<LoginViewModel>();
		}
	}
}
