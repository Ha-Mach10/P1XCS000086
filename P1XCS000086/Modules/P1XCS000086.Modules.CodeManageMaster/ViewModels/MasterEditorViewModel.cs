using System;
using System.Reactive.Disposables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Regions;
using Prism.Commands;
using Prism.Mvvm;

using Reactive.Bindings.Extensions;
using Prism.Navigation;


namespace P1XCS000086.Modules.CodeManageMaster.ViewModels
{
	public class MasterEditorViewModel : BindableBase, IDestructible
	{
		private string _message;
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposable;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public MasterEditorViewModel()
		{
			Message = "View A from your Prism Module";
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		public void Destroy()
		{

		}
	}
}
