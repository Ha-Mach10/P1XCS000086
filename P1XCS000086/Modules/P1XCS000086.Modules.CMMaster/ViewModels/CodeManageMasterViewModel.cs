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
using System.Windows;
using System.Reactive.Disposables;

using MaterialDesignThemes.Wpf;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Interfaces.IO;

namespace P1XCS000086.Modules.CMMaster.ViewModels
{
	public class CodeManageMasterViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposable;

		private ICodeManagerMasterModel _model;

		private IJsonConnectionStrings _jsonConnStr;
		private ISqlConnectionTest _connectionTest;
		private IJsonExtention _jsonExtention;
		private IJsonConnectionItem _jsonConnItem;
		private ISqlSelect _select;
		private ISqlInsert _insert;
		private ISqlUpdate _update;
		private ISqlDelete _delete;
		private ISqlShowTables _showTables;



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
		internal ReactivePropertySlim<List<ITableField>> DatabaseFields { get; }

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

		public CodeManageMasterViewModel(ICodeManagerMasterModel model)
		{
			_model = model;

			// DIされたモデルを注入
			_model.InjectModels(_connectionTest, _jsonExtention, _jsonConnItem, _jsonConnStr, _select, _insert, _update, _delete, _showTables);

			// Properties
			SelectedTableName = new ReactivePropertySlim<string>().AddTo(_disposable);
			MasterDataTable = new ReactivePropertySlim<DataTable>().AddTo(_disposable);
			SelectedRow = new ReactivePropertySlim<DataRow>().AddTo(_disposable);

			// Reactive Commands
			RegistConnString = new ReactiveCommandSlim();
			RegistConnString.Subscribe(() => OnRegistConnString()).AddTo(_disposable);
			DataGridSelected = new ReactiveCommandSlim();
			DataGridSelected.Subscribe(() => OnDataGridSelected()).AddTo(_disposable);

		}



		// ****************************************************************************
		// Reactive Commands
		// ****************************************************************************

		public ReactiveCommandSlim DataGridSelected { get; }
		private void OnDataGridSelected()
		{
			
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
		/// <summary>
		/// おそらく不要
		/// </summary>
		private void OnRadioCheckedChanged()
		{

		}

		public ReactiveCommandSlim ListViewSelectionChanged { get; }
		private void OnListViewSelectionChanged()
		{
			if (SelectedTableName.Value is not null)
			{
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
