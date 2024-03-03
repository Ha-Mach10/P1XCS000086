using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;

namespace P1XCS000086.Modules.CodeManageMaster.ViewModels
{
	public class CodeManageMasterHostViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposable;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<DataTable> MasterDataTable { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManageMasterHostViewModel()
		{
			MasterDataTable = new ReactivePropertySlim<DataTable>().AddTo(_disposable);
		}



		// ****************************************************************************
		// Reactive Command
		// ****************************************************************************





		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		// 破棄
		public void Destroy()
		{

		}
	}
}
