using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using P1XCS000086.Core.Mvvm;
using System.ComponentModel;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeManagerViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// Fields
		private IRegionManager _regionManager;


		// Properties




		public CodeManagerViewModel(IRegionManager regionManager) : base(regionManager)
		{
			_regionManager = regionManager;
		}
	}
}
