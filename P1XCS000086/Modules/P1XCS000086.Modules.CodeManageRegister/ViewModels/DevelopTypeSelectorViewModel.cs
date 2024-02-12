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
		// 
		private string _projectDirectry = string.Empty;



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


			// 
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


			// 接続文字列が登録されている時、ステートメント内に入る
			if (_connStr.IsGetConnectionString(out string sqlConnStr))
			{

			}
		}



		// ****************************************************************************
		// Command Event
		// ****************************************************************************

		// 
		public ReactiveCommand LanguageTypeComboChange { get; }
		/// <summary>
		/// 
		/// </summary>
		private void OnLanguageTypeComboChange()
		{
			// Language文字列ComboBoxから取得（Like検索用）
			string selectedValue = LanguageSelectedValue.Value;

			// 
			DevelopmentItemCollection.Value = _model.DevelopmentComboBoxItemSetting(selectedValue);

			// DataGrid用のDataTableを取得
			DataTable dt = _model.CodeManagerDataGridItemSetting(selectedValue);
			IIntegrRegisterModel.GridDataTable = dt;

			// プロジェクトのディレクトリを取得
			string directryPath = _model.GetProjectDirectry(selectedValue);
			_projectDirectry = directryPath;
			IIntegrRegisterModel.ProjectDirectryText = $"対象ディレクトリ ： {directryPath}";

			// レコード数を取得
			IIntegrRegisterModel.RecordCount = dt.Rows.Count;

			// 
			IIntegrRegisterModel.DevItemSelectedIndex = -1;

			// GroupBoxのVisibilityを変更（非表示）
			_integrModel.ChangeVisibility((int)Visibility.Collapsed);
		}

		public ReactiveCommand DevelopmentTypeComboChange { get; }
		/// <summary>
		/// 
		/// </summary>
		private void OnDevelopmentTypeComboChange()
		{
			// 初回起動時または言語種別ComboBox変更時
			if (IIntegrRegisterModel.DevItemSelectedIndex == -1 || IIntegrRegisterModel.IsDevItemSelected == false)
			{
				// 
				IIntegrRegisterModel.IsDevItemSelected = true;

				return;
			}
			DataTable dt = _model.CodeManagerDataGridItemSetting(DevelopmentSelectedValue.Value, LanguageSelectedValue.Value);
			IIntegrRegisterModel.GridDataTable = dt;

			IIntegrRegisterModel.RecordCount = dt.Rows.Count;

			// GroupBoxのVisibilityを変更（表示）
			_integrModel.ChangeVisibility((int)Visibility.Visible);
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 破棄
		/// </summary>
		public void Destroy()
		{
			_disposables.Dispose();
		}
	}
}
