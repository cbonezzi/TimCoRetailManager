﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AutoMapper;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;
using TRMDesktopUI.ViewModels;

namespace TRMDesktopUI
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly SimpleContainer _container = new SimpleContainer();

		public Bootstrapper()
		{
			Initialize();

			ConventionManager.AddElementConvention<PasswordBox>(
				PasswordBoxHelper.BoundPasswordProperty,
				"Password",
				"PasswordChanged");
		}

		private IMapper ConfigureAutomapper()
		{
			// this will allow us to 
			// this uses reflection at the beginning and done!
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ProductModel, ProductDisplayModel>();
				cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
			});

			var output = config.CreateMapper();

			return output;

		}

		protected override void Configure()
		{
			// this will put the mapper into the container
			// this is a singleton in essence
			_container.Instance(ConfigureAutomapper());

			_container.Instance(_container)
				.PerRequest<IProductEndpoint, ProductEndpoint>()
				.PerRequest<ISaleEndpoint, SaleEndpoint>()
				.PerRequest<IUserEndpoint, UserEndpoint>();

			//central location for both windowmanager and eventaggregator
			//this will keep a central location for these important info
			//know it all for this for this instance.
			_container
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<ILoggedInUserModel, LoggedInUserModel>()
				.Singleton<IApiHelper, ApiHelper>();

			//using reflection at this point
			//this is fine for now, but for the future, instead of wiring directly to class, we can
			//do the interface.
			GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => _container.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			//launch shellviewmodel as our startup
			DisplayRootViewFor<ShellViewModel>();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}
	}
}
