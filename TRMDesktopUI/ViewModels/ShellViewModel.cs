using System;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
	{
		//private LoginViewModel _loginVM;
		private SalesViewModel _salesVM;
		private IEventAggregator _events;
		private SimpleContainer _container;

		public ShellViewModel(
			//LoginViewModel loginVM,
			IEventAggregator events,
			SalesViewModel salesVM,
			SimpleContainer container)
		{
			_events = events;
			//_loginVM = loginVM;
			_salesVM = salesVM;
			_container = container;

			_events.SubscribeOnUIThread(this);
			
			//ActivateItemAsync(_loginVM);
			ActivateItemAsync(_container.GetInstance<LoginViewModel>());
		}

		public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(_salesVM);

			//wiping out the loginVM
			//_loginVM = _container.GetInstance<LoginViewModel>();
		}
	}
}
