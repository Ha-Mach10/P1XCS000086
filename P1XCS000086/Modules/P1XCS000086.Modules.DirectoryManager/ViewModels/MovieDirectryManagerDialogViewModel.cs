using P1XCS000086.Core.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.DirectoryManager.ViewModels
{
	public class MovieDirectryManagerDialogViewModel : RegionViewModelDialogBase
	{
		// ---------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------- 

		private IRegionManager _regionManager;
		private IDialogAware _dialogAware;




		private string _message;
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		public MovieDirectryManagerDialogViewModel(IRegionManager regionManager, IDialogAware dialogAware)
			:base(regionManager, dialogAware)
		{
			// 
			_regionManager = regionManager;
			_dialogAware = dialogAware;


			Message = "View A from your Prism Module";
		}
	}
}
