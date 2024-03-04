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
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;

namespace P1XCS000086.Modules.CodeManageMaster.ViewModels
{
	public class CodeManageMasterHostViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposable;

		private ICodeManagerMasterHostModel _model;
		private IIntegrMasterModel _integrModel;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<DataTable> MasterDataTable { get; }
		public ReactivePropertySlim<string> SelectedMasterTable { get; }
		public ReactivePropertySlim<DataRow> SelectedRow { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManageMasterHostViewModel(ICodeManagerMasterHostModel model, IIntegrMasterModel integrModel)
		{
			_model = model;
			_integrModel = integrModel;

			// DIされたモデルを注入
			_model.InjectModels(_integrModel);

			// Properties
			MasterDataTable = new ReactivePropertySlim<DataTable>().AddTo(_disposable);
			SelectedMasterTable = new ReactivePropertySlim<string>(string.Empty).AddTo(_disposable);
			SelectedRow = new ReactivePropertySlim<DataRow>().AddTo(_disposable);

			// Reactive Commands
			DataGridSelected = new ReactiveCommandSlim();
			DataGridSelected.Subscribe(() => OnDataGridSelected()).AddTo(_disposable);
		}



		// ****************************************************************************
		// Reactive Commands
		// ****************************************************************************

		public ReactiveCommandSlim DataGridSelected { get; }
		private void OnDataGridSelected()
		{
			// 取得したDetaRowをセット
			_integrModel.SetSelectedRow(SelectedRow.Value);
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		// 破棄
		public void Destroy()
		{
			_disposable?.Dispose();
		}
	}
}
