using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Core.Mvvm
{
    public class RegionViewModelDialogBase : RegionViewModelBase, IDialogAware, INotifyPropertyChanged
    {
		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		protected IDialogAware DialogAware { get; private set; }



		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		public RegionViewModelDialogBase(IRegionManager regionManager, IDialogAware dialogAware)
            : base(regionManager)
        {
			DialogAware = dialogAware;
        }



		#region Implementation Interface

		// ---------------------------------------------------------------
		// IDialogAware
		// ---------------------------------------------------------------

		public virtual string Title => "defaultDialog";

		public event Action<IDialogResult> RequestClose;

		public virtual bool CanCloseDialog() => true;

		public virtual void OnDialogClosed() { }

		public virtual void OnDialogOpened(IDialogParameters parameters) { }

		#endregion
	}
}
