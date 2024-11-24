using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Core;
using P1XCS000086.Services.Interfaces.Models.Automation;
using Prism.Navigation.Regions;
using Reactive.Bindings.Disposables;
using System.ComponentModel;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Web.WebView2.Wpf;
using MahApps.Metro.Controls;
using P1XCS000086.Modules.AutomationView.Domains;

namespace P1XCS000086.Modules.AutomationView.ViewModels
{
	public class WebAutoMationBaseViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// *****************************************************************************
		// Fields
		// *****************************************************************************

		private IRegionManager _regionManager;
		private IWebAutoMationBaseModel _model;



		// *****************************************************************************
		// Properties
		// *****************************************************************************



		// *****************************************************************************
		// Reactive Properties
		// *****************************************************************************

		public ReactiveCollection<ServiceButtonItem> Buttons { get; }



		public WebAutoMationBaseViewModel(IRegionManager regionManager, IWebAutoMationBaseModel model)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;


			KeepAlive = true;


		}



		// *****************************************************************************
		// Reactive Commands
		// *****************************************************************************

		public ReactiveCommandSlim<string> TransitionView { get; }
		private void OnTransitionView(string commandProperty)
		{
			if (string.IsNullOrEmpty(commandProperty) is not true)
			{
				_regionManager.RequestNavigate(RegionNames.AutomationWebViewRegion, commandProperty);
			}
		}



		// *****************************************************************************
		// Reactive Commands
		// *****************************************************************************

		private IEnumerable<ServiceButtonItem> GenerateServiceButtons()
		{
			yield return null;
		}
	}
}
