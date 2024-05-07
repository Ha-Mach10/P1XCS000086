using P1XCS000086.Core.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.HouseholdExpenses.ViewModels
{
	public class HEHomeViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		private IRegionManager _regionManager;



		// ****************************************************************************
		// Properties
		// ****************************************************************************





		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public HEHomeViewModel(IRegionManager regionManager) : base(regionManager)
		{
			_regionManager = regionManager;
		}
	}
}
