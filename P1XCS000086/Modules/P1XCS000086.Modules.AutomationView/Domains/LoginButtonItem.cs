using P1XCS000086.Core.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.AutomationView.Domains
{
	public class LoginButtonItem : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// ********************************************************
		// Properties
		// ********************************************************

		public bool KeepAlive { get; set; } = false;
		
		public string Name { get; private set; } = string.Empty;
		public string ImageKind { get; private set; } = string.Empty;
		public string ViewName { get; private set; } = string.Empty;



		// ********************************************************
		// Reactive Command Properties
		// ********************************************************

		public ReactiveCommandSlim<string> TransitionCommand { get; }



		public LoginButtonItem(IRegionManager regionManager, string name, string imageKind, string viewName, Action<string> transitionAction)
			: base(regionManager)
		{
			Name = name;
			ImageKind = imageKind;
			ViewName = viewName;

			TransitionCommand = new();
			TransitionCommand.Subscribe(transitionAction).AddTo(_disposables);
		}
	}
}
