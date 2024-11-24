using P1XCS000086.Core.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace P1XCS000086.Modules.MovDirectryManager.ViewModels
{
	public class MovieDirectryManagerViewModel : RegionViewModelBase
	{
		IRegionManager _regionManager;
		private string _message;
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		public MovieDirectryManagerViewModel(IRegionManager regionManager)
			:base(regionManager)
		{
			_regionManager = regionManager;

			Message = "View A from your Prism Module";
		}
	}
}
