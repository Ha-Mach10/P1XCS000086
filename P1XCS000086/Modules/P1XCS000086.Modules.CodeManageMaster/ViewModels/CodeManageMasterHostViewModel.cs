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
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using MaterialDesignThemes.Wpf;
using System.Windows;

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

		public ReactivePropertySlim<string> SelectedTableName { get; }
		public ReactivePropertySlim<DataTable> MasterDataTable { get; }
		public ReactivePropertySlim<DataRow> SelectedRow { get; }

		// MasterEditorViewModelより移植
		public ReactivePropertySlim<bool> IsEditChecked { get; }
		public ReactivePropertySlim<bool> IsAddChecked { get; }
		public ReactivePropertySlim<bool> IsDeleteChecked { get; }
		public ReactivePropertySlim<List<string>> TableItems { get; }
		/// <summary>
		/// 選択されたテーブル名
		/// </summary>
		// public ReactivePropertySlim<string> SelectedTableName { get; }
		/// <summary>
		/// 
		/// </summary>
		public ReactivePropertySlim<DataRow> Rows { get; }
		/// <summary>
		///テーブル編集用の入力フィールド
		/// </summary>
		public ReactivePropertySlim<List<ITableField>> DatabaseFields { get; }

		// CodeManagerFieldViewModelより移植
		public ReactivePropertySlim<string> Server { get; }
		public ReactivePropertySlim<string> User { get; }
		public ReactivePropertySlim<string> Database { get; }
		public ReactivePropertySlim<string> Password { get; }
		public ReactivePropertySlim<bool> PersistSecurityInfo { get; }
		public ReactivePropertySlim<SnackbarMessageQueue> ConnSnackbarMessageQueue { get; }
		public ReactivePropertySlim<Visibility> RegisterButtonVisibility { get; }



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
			SelectedTableName = new ReactivePropertySlim<string>(_integrModel.SelectedTableName).AddTo(_disposable);
			MasterDataTable = new ReactivePropertySlim<DataTable>().AddTo(_disposable);
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


		public ReactiveCommandSlim RegistConnString { get; }

		/// <summary>
		/// 接続文字列の登録
		/// </summary>
		private void OnRegistConnString()
		{
			_model.RegistConnectionString(Server.Value, User.Value, Database.Value, Password.Value, PersistSecurityInfo.Value);
		}

		// MasterEditorViewModelより移植

		public ReactiveCommandSlim RadioCheckedChanged { get; }
		private void OnRadioCheckedChanged()
		{

		}

		public ReactiveCommandSlim ListViewSelectionChanged { get; }
		private void OnListViewSelectionChanged()
		{
			if (SelectedTableName.Value is not null)
			{
				_integrModel.SetSelectedTableName(SelectedTableName.Value);
				DatabaseFields.Value = _model.GetTableFields(SelectedTableName.Value);
			}
		}

		public ReactiveCommandSlim ApplyButtonClick { get; }
		private void OnApplyButtonClick()
		{
			if (IsEditChecked.Value)
			{

			}
			else if (IsAddChecked.Value)
			{

			}
			else if (IsDeleteChecked.Value)
			{

			}
		}

		// CodeManagerFieldViewModelより移植

		public ReactiveCommandSlim ConnectionTest { get; }

		/// <summary>
		/// 接続テスト
		/// </summary>
		private void OnConnectionTest()
		{
			// 接続テスト
			string message = _model.TestDatabaseConnection(out bool result);

			// 接続の成否によって登録ボタンのVisibilityを変更する
			if (result)
			{
				// 成功した場合
				RegisterButtonVisibility.Value = Visibility.Visible;
			}
			else
			{
				// 失敗した場合
				RegisterButtonVisibility.Value = Visibility.Collapsed;
			}

			// Snackbarを表示
			ConnSnackbarMessageQueue.Value.Enqueue(message);
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
