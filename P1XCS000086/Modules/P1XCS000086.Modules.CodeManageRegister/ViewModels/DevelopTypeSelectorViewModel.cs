using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Reactive.Disposables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Interfaces.Models.CodeManageRegister;

namespace P1XCS000086.Modules.CodeManageRegister.ViewModels
{
	public class DevelopTypeSelectorViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************
		
		// 
		private CompositeDisposable _disposables;
		// 
		private IDevelopTypeSelectorModel _model;
		private IIntegrRegisterModel _integrModel;
		// 
		private IMySqlConnectionString _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;



		// ****************************************************************************
		// Reactive Properties
		// ****************************************************************************

		// Reactive Properties
		public ReactivePropertySlim<List<string>> LanguageItemCollection { get; }
		public ReactivePropertySlim<List<string>> DevelopmentItemCollection { get; }
		// 
		public ReactivePropertySlim<string> LanguageSelectedValue { get; }
		public ReactivePropertySlim<string> DevelopmentSelectedValue { get; }
		// 
		public ReactivePropertySlim<int> RecordCount { get; }
		// 
		public ReactivePropertySlim<Visibility> DevelopNumberContentControlVisibility { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public DevelopTypeSelectorViewModel(IDevelopTypeSelectorModel model, IMySqlConnectionString connStr, ISqlSelect select, ISqlInsert insert, IIntegrRegisterModel integrModel)
		{
			// インターフェースのインジェクション
			_model = model;
			_integrModel = integrModel;
			_connStr = connStr;
			_select = select;
			_insert = insert;

			_model.SetModelBuiltin(_select, _connStr);


			// ReactivePropertyの初期値設定
			LanguageItemCollection					= new ReactivePropertySlim<List<string>>().AddTo(_disposables);
			DevelopmentItemCollection				= new ReactivePropertySlim<List<string>>().AddTo(_disposables);
			LanguageSelectedValue					= new ReactivePropertySlim<string>(string.Empty).AddTo(_disposables);
			DevelopmentSelectedValue				= new ReactivePropertySlim<string>(string.Empty).AddTo(_disposables);
			RecordCount								= new ReactivePropertySlim<int>(0).AddTo(_disposables);
			DevelopNumberContentControlVisibility	= new ReactivePropertySlim<Visibility>(Visibility.Collapsed).AddTo(_disposables);


			// Commandの購読
			LanguageTypeComboChange = new ReactiveCommand();
			LanguageTypeComboChange.Subscribe(() => OnLanguageTypeComboChange()).AddTo(_disposables);
			DevelopmentTypeComboChange = new ReactiveCommand();
			DevelopmentTypeComboChange.Subscribe(() => OnDevelopmentTypeComboChange()).AddTo(_disposables);
		}



		// ****************************************************************************
		// Command Event
		// ****************************************************************************

		// 
		public ReactiveCommand LanguageTypeComboChange { get; }
		private void OnLanguageTypeComboChange()
		{
			// Language文字列ComboBoxから取得（Like検索用）
			string selectedValue = LanguageSelectedValue.Value;


			DevelopmentItemCollection.Value = _model.DevelopmentComboBoxItemSetting(selectedValue);

			// DataGrid用のDataTableを取得
			DataTable dt = _model.CodeManagerDataGridItemSetting(selectedValue);
			_integrModel.SetDataTable(dt);

			// プロジェクトのディレクトリを取得
			string directryPath = _mainWindowModel.GetProjectDirectry(selectedValue);
			ProjectDirectryText = $"対象ディレクトリ ： {directryPath}";

			RowsCount = dt.Rows.Count;

			DevItemSelectedIndex = -1;

			// GroupBoxのVisibilityを変更（非表示）
			GroupBoxVisibility = Visibility.Collapsed;
		}
		public ReactiveCommand DevelopmentTypeComboChange { get; }
		private void OnDevelopmentTypeComboChange()
		{
			// 初回起動時または言語種別ComboBox変更時
			if (DevItemSelectedIndex == -1 || _isDevItemSelected == false)
			{
				_isDevItemSelected = true;

				return;
			}
			DataTable dt = _mainWindowModel.CodeManagerDataGridItemSetting(DevelopmentSelectedValue.Value, LanguageSelectedValue.Value);
			DataGridItems = dt;

			RowsCount = dt.Rows.Count;

			// GroupBoxのVisibilityを変更（表示）
			GroupBoxVisibility = Visibility.Visible;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		// 破棄
		public void Destroy()
		{
			_disposables.Dispose();
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		private string GetProjectDirectry(string languageType)
		{
			string queryCommand = @$"SELECT language_directry
									 FROM project_language_directry
									 WHERE language_type=
									 (
										SELECT language_type_code
										FROM manager_language_type
										WHERE language_type='{languageType}'
									 );";
			string directryPath = GetSelectItem("language_directry", queryCommand);

			return directryPath;
		}
	}
}
