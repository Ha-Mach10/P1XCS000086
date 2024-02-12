using Prism.Commands;
using Prism.Mvvm;

using Reactive.Bindings.Extensions;
using Reactive.Bindings;

using P1XCS000086.Services.Interfaces.Models.CodeManageRegister;

using System;
using System.Reactive.Disposables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using System.Data;
using Prism.Regions;
using Reactive.Bindings.ObjectExtensions;

namespace P1XCS000086.Modules.CodeManageRegister.ViewModels
{
	public class CodeManagerRegisterViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		// オブジェクト破棄用
		private CompositeDisposable _disposables;


		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public DataTable GridDataTable
		{
			get => IIntegrRegisterModel.GridDataTable;
		}



		// ****************************************************************************
		// Reactive Properties
		// ****************************************************************************

		public ReactivePropertySlim<DataTable> DataItem { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManagerRegisterViewModel()
		{
			// 
			DataItem = new ReactivePropertySlim<DataTable>(GridDataTable).AddTo(_disposables);
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 破棄
		/// </summary>
		public void Destroy()
		{
			_disposables?.Dispose();
		}
	}
}
