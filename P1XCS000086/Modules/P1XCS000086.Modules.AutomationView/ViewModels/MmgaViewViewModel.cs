using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Models.Automation;
using Prism.Regions;
using Reactive.Bindings.Disposables;
using System.ComponentModel;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Web.WebView2.Wpf;
using MahApps.Metro.Controls;
using System.Windows;
using Meowtrix.PixivApi;

namespace P1XCS000086.Modules.AutomationView.ViewModels
{
	public class MmgaVViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// *****************************************************************************
		// Fields
		// *****************************************************************************

		private IMmgaVModel _model;



		// *****************************************************************************
		// Properties
		// *****************************************************************************





		// *****************************************************************************
		// Reactive Properties
		// *****************************************************************************




		public MmgaVViewModel(IRegionManager regionManager, IMmgaVModel model)
			: base(regionManager)
		{
			_model = model;


			KeepAlive = true;




			// Commands

		}



		// *****************************************************************************
		// Reactive Properties
		// *****************************************************************************





		// *****************************************************************************
		// Events
		// *****************************************************************************


	}
}
