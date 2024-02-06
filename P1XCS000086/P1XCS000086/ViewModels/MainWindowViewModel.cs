using System;
using System.Data;
using System.Windows;
using System.Diagnostics;
using System.Reactive;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Xps.Serialization;
using System.Security.Cryptography.Xml;
using System.CodeDom;
using System.Reflection.Emit;

using P1XCS000086.Services;
using P1XCS000086.Services.Models;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.Objects;

using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.ObjectExtensions;

using MaterialDesignThemes.Wpf;

using Org.BouncyCastle.Bcpg.OpenPgp;
using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.Domains;
using System.Linq;


namespace P1XCS000086.ViewModels
{
	public class MainWindowViewModel : BindableBase, IDestructible, INotifyPropertyChanged
	{
		private string _title = "Prism Application";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private IRegionManager _regionManager;
		private IMainWindowModel _mainWindowModel;
		private IJsonConnectionStrings _jsonConnectionStrings;
		private IJsonExtention _jsonExtention;
		private CompositeDisposable disposables = new CompositeDisposable();


		// 
		private ObservableCollection<string> _languageItemCollection;
		private ObservableCollection<string> _developmentItemCollections;
		private DataTable _dataGridItems;
		private int _rowsCount;
		private int _devItemSelectedIndex;
		private bool _isDevItemSelected = false;



        #region ReactiveProperties
		// 
		public ReactivePropertySlim<List<string>> LanguageClassificationsItemViewCollection { get; }
		public ReactivePropertySlim<List<string>> UseApplicationsItemViewCollection { get; }
		public ReactivePropertySlim<string> LangViewSelectedItem { get; }
		public ReactivePropertySlim<string> UseAppViewSelectedItem { get; }
		public ReactivePropertySlim<DataTable> ViewOnlyCodesDataTable { get; }


		// 
		public ReactivePropertySlim<bool> AddRadioIsChecked { get; }
		public ReactivePropertySlim<bool> EditRadioIsChecked { get; }
		public ReactivePropertySlim<bool> DeleteRadioIsChecked { get; }
		public ReactivePropertySlim<List<string>> DatabaseTables { get; }
		public ReactivePropertySlim<List<DBTableColumnFieldItem>> DBTableColumnsFieldItems { get; }
		public ReactivePropertySlim<string> DBTableSelectedValue { get; }
		public ReactivePropertySlim<DataTable> TableDataGridItem { get; }


		// 
		public ReactivePropertySlim<int> SelectedTabIndex { get; }
		public ReactivePropertySlim<string> SearchTextDevName { get; }
		public ReactivePropertySlim<string> SearchTextCodeName { get; }
		public ReactivePropertySlim<List<string>> SearchTextUseApplivation { get; }
		// 
		public ReactivePropertySlim<string> Server { get; }
		public ReactivePropertySlim<string> User { get; }
		public ReactivePropertySlim<string> Database { get; }
		public ReactivePropertySlim<string> Password { get; }
		public ReactivePropertySlim<bool> PersistSecurityInfo { get; }
		// 
		public ReactivePropertySlim<string> LanguageSelectedValue { get; }
		public ReactivePropertySlim<string> DevelopmentSelectedValue { get; }
		public ObservableCollection<string> LanguageItemCollection
		{
			get => _languageItemCollection;
			set => SetProperty(ref _languageItemCollection, value);
		}
		public ObservableCollection<string> DevelopmentItemCollection
		{
			get => _developmentItemCollections;
			set => SetProperty(ref _developmentItemCollections, value);
		}
		public DataTable DataGridItems
		{
			get => _dataGridItems;
			set => SetProperty(ref _dataGridItems, value);
		}
		public int RowsCount
		{
			get => _rowsCount;
			set => SetProperty(ref _rowsCount, value);
		}
		public int DevItemSelectedIndex
		{
			get => _devItemSelectedIndex;
			set => SetProperty(ref _devItemSelectedIndex, value);
		}

		private bool _useApplicationIsChecked = false;
		public bool UseApplicationIsChecked
		{
			get => _useApplicationIsChecked;
			set => SetProperty(ref _useApplicationIsChecked, value);
		}
		public ReactivePropertySlim<string> DevelopName { get; }
		public ReactivePropertySlim<string> CodeName { get; }
		public ReactivePropertySlim<List<string>> UseApplication { get; }
		public ReactivePropertySlim<List<string>> UseApplicationSub { get; }
		public ReactivePropertySlim<string> UseApplicationSelectedValue { get; }
		public ReactivePropertySlim<string> UseApplicationSubSelectedValue { get; }
		public ReactivePropertySlim<string> UseApplicationManual { get; }
		public ReactivePropertySlim<string> ReferenceNumber { get; }
		public ReactivePropertySlim<string> OldNumber { get; }
		public ReactivePropertySlim<string> NewNumber { get; }
		public ReactivePropertySlim<string> InheritenceNumber { get; }
		public ReactivePropertySlim<string> Explanation { get; }
		public ReactivePropertySlim<string> Summary { get; }

		private Visibility _useApplicationComboBoxVisibility = Visibility.Visible;
		public Visibility UseApplicationComboBoxVisibility
		{
			get => _useApplicationComboBoxVisibility;
			set => SetProperty(ref _useApplicationComboBoxVisibility, value);
		}
		private Visibility _useApplicationTextBoxVisibility = Visibility.Collapsed;
		public Visibility UseApplicationTextBoxVisibility
		{
			get => _useApplicationTextBoxVisibility;
			set => SetProperty(ref _useApplicationTextBoxVisibility, value);
		}
		public ReactivePropertySlim<Visibility> RegistButtonVisibility { get; }


		private SnackbarMessageQueue _snackBarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));
		public SnackbarMessageQueue SnackBarMessageQueue
		{
			get => _snackBarMessageQueue;
			set => SetProperty(ref _snackBarMessageQueue, value);
		}
		private Visibility _groupBoxVisibility = Visibility.Collapsed;
		public Visibility GroupBoxVisibility
		{
			get => _groupBoxVisibility;
			set => SetProperty(ref _groupBoxVisibility, value);
		}

		private string _projectDirectryText;
		public string ProjectDirectryText
		{
			get => _projectDirectryText;
			set => SetProperty(ref _projectDirectryText, value);
		}


		public ReactivePropertySlim<string> ConnectionDatabase { get; }
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="regionManager"></param>
		public MainWindowViewModel(IRegionManager regionManager, IMainWindowModel model, IJsonExtention jsonExtention, IJsonConnectionStrings jsonConnString)
		{
			_regionManager = regionManager;

			// MainWindowModelをインターフェース(IMainWindowModel)から生成
			_mainWindowModel = model;
			_jsonConnectionStrings = jsonConnString;
			_jsonExtention = jsonExtention;


			// -----------------------------------------------------------------------------------------------------
			// Properties Initialize
			// -----------------------------------------------------------------------------------------------------

			// タブの初期選択インデックスを設定
			SelectedTabIndex = new ReactivePropertySlim<int>(0).AddTo(disposables);

			// View用ComboBox選択要素取得用プロパティ初期設定
			LanguageClassificationsItemViewCollection	= new ReactivePropertySlim<List<string>>().AddTo(disposables);
			UseApplicationsItemViewCollection			= new ReactivePropertySlim<List<string>>().AddTo(disposables);
			LangViewSelectedItem						= new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			UseAppViewSelectedItem						= new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			ViewOnlyCodesDataTable						= new ReactivePropertySlim<DataTable>().AddTo(disposables);

			// 
			SearchTextDevName			= new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			SearchTextCodeName			= new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);

			// 
			LanguageSelectedValue = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			DevelopmentSelectedValue = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);

			// 開発番号登録フィールド用プロパティ群
			DevelopName				 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			CodeName				 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			UseApplicationManual	 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			ReferenceNumber			 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			OldNumber				 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			NewNumber				 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			InheritenceNumber		 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			Explanation				 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			Summary					 = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			
			UseApplicationIsChecked = true;
			UseApplicationComboBoxVisibility = Visibility.Visible;
			UseApplicationTextBoxVisibility = Visibility.Collapsed;

			RegistButtonVisibility = new ReactivePropertySlim<Visibility>(Visibility.Collapsed).AddTo(disposables);


			// 
			AddRadioIsChecked		= new ReactivePropertySlim<bool>(true).AddTo(disposables);
			EditRadioIsChecked		= new ReactivePropertySlim<bool>(false).AddTo(disposables);
			DeleteRadioIsChecked	= new ReactivePropertySlim<bool>(false).AddTo(disposables);
			DBTableSelectedValue	= new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			TableDataGridItem		= new ReactivePropertySlim<DataTable>().AddTo(disposables);



			// JSONファイルが存在していない場合
			// IJsonExtention jsonExtention = new JsonExtention();
			// IJsonConnectionStrings jsonConnString = mainWindowModel.JsonDeserialize();
            if (jsonExtention.PathCheckAndGenerate() && jsonConnString is not null)
			{
				// Properties SQL Connection
				Server				= new ReactivePropertySlim<string>(jsonConnString.Server).AddTo(disposables);
				User				= new ReactivePropertySlim<string>(jsonConnString.User).AddTo(disposables);
				Database			= new ReactivePropertySlim<string>(jsonConnString.DatabaseName).AddTo(disposables);
				Password			= new ReactivePropertySlim<string>(jsonConnString.Password).AddTo(disposables);
				PersistSecurityInfo = new ReactivePropertySlim<bool>(jsonConnString.PersistSecurityInfo).AddTo(disposables);

				// ComboBoxItem Property
				ObservableCollection<string> languageItems = new ObservableCollection<string>();
				foreach (string item in mainWindowModel.LanguageComboBoxItemSetting())
				{
					languageItems.Add(item);
				}
				LanguageItemCollection = languageItems;

				// 
				List<string> languageItemList = mainWindowModel.LanguageComboBoxItemSetting();
				List<string> developmentItemList = mainWindowModel.ViewUseApplicationComboBoxItemSetting();
				LanguageClassificationsItemViewCollection = new ReactivePropertySlim<List<string>>(languageItemList).AddTo(disposables);
				UseApplicationsItemViewCollection = new ReactivePropertySlim<List<string>>(developmentItemList).AddTo(disposables);


				// 
				List<string> useApplicationItems = mainWindowModel.UseApplicationComboBoxItemSetting();
				List<string> useApplicationSubItems = mainWindowModel.UseApplicationSubComboBoxItemSetting();
				UseApplication = new ReactivePropertySlim<List<string>>(useApplicationItems).AddTo(disposables);
				UseApplicationSub = new ReactivePropertySlim<List<string>>(useApplicationSubItems).AddTo(disposables);

				// 
				// List<string> searchTextUseApplicationItems = mainWindowModel.SearchTextUseApplicationComboBoxItemSetting();
				SearchTextUseApplivation = new ReactivePropertySlim<List<string>>(new List<string>()).AddTo(disposables);


				// 
				DatabaseTables = new ReactivePropertySlim<List<string>>(mainWindowModel.ShowTableItems()).AddTo(disposables);
				DBTableColumnsFieldItems = new ReactivePropertySlim<List<DBTableColumnFieldItem>>().AddTo(disposables);

			}
			else if (jsonConnString is null)
			{
                Server = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                User = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                Database = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                Password = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                PersistSecurityInfo = new ReactivePropertySlim<bool>(false).AddTo(disposables);
            }


			// -----------------------------------------------------------------------------------------------------
			// Command Events
			// -----------------------------------------------------------------------------------------------------

			// 
			SearchTextClaar = new ReactiveCommand();
			SearchTextClaar.Subscribe(() => OnSearchTextClaar()).AddTo(disposables);
			// 
			LanguageTypeComboChange = new ReactiveCommand();
			LanguageTypeComboChange.Subscribe(() => OnLanguageTypeComboChange()).AddTo(disposables);
			DevelopmentTypeComboChange = new ReactiveCommand();
			DevelopmentTypeComboChange.Subscribe(() => OnDevelopmentTypeComboChange()).AddTo(disposables);
			// 
			SqlConnectionTest = new ReactiveCommand();
			SqlConnectionTest.Subscribe(() => OnSqlConnectionTest()).AddTo(disposables);
			RegistSqlConnectionString = new ReactiveCommand();
			RegistSqlConnectionString.Subscribe(() => OnRegistSqlConnectionString()).AddTo(disposables);
			// 
			CheckedStateChanged = new ReactiveCommand();
			CheckedStateChanged.Subscribe(() => OnCheckedStateChanged()).AddTo(disposables);
			// 
			CodeNumberRegist = new ReactiveCommand();
			CodeNumberRegist.Subscribe(() => OnCodeNumberRegist()).AddTo(disposables);
			// 
			TextChanged = new ReactiveCommand();
			TextChanged.Subscribe(() => OnChangedValue()).AddTo(disposables);
			// 
			ComboChanged = new ReactiveCommand();
			ComboChanged.Subscribe(() => OnChangedValue()).AddTo(disposables);
			
			// 
			DataGridSelectComboChanged = new ReactiveCommand();
			DataGridSelectComboChanged.Subscribe(() => OnDataGridSelectComboChanged()).AddTo(disposables);

			// 
			TableSelectionChange = new ReactiveCommand();
			TableSelectionChange.Subscribe(() => OnTableSelectionChange()).AddTo(disposables);
		}


		// Command Events
		public ReactiveCommand DataGridSelectComboChanged { get; }
		private void OnDataGridSelectComboChanged()
		{
			string langValue = LangViewSelectedItem.Value;
			string useAppValue = UseAppViewSelectedItem.Value;

			if (langValue is null) { langValue = string.Empty; }
			if (useAppValue is null) { useAppValue = string.Empty; }
			
			ViewOnlyCodesDataTable.Value = _mainWindowModel.GetViewDataTable(langValue, useAppValue);
		}

		public ReactiveCommand LanguageTypeComboChange { get; }
		/// <summary>
		/// ****************************************************************************************************************
		/// 不要
		/// </summary>
		private void OnLanguageTypeComboChange()
		{
			// Language文字列ComboBoxから取得（Like検索用）
			string selectedValue = LanguageSelectedValue.Value;
			ObservableCollection<string> developmentItems = new ObservableCollection<string>();
			foreach (string item in _mainWindowModel.DevelopmentComboBoxItemSetting(selectedValue))
			{
				developmentItems.Add(item);
			}
			DevelopmentItemCollection = developmentItems;

			// DataGrid用のDataTableを取得
			DataTable dt = _mainWindowModel.CodeManagerDataGridItemSetting(selectedValue);
			DataGridItems = dt;

			// プロジェクトのディレクトリを取得
			string directryPath = _mainWindowModel.GetProjectDirectry(selectedValue);
			ProjectDirectryText = $"対象ディレクトリ ： {directryPath}";

			RowsCount = dt.Rows.Count;

			DevItemSelectedIndex = -1;

			// GroupBoxのVisibilityを変更（非表示）
			GroupBoxVisibility = Visibility.Collapsed;
		}
		public ReactiveCommand DevelopmentTypeComboChange { get; }
		/// <summary>
		/// ****************************************************************************************************************
		/// 不要
		/// </summary>
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

		public ReactiveCommand SqlConnectionTest { get; }
		private void OnSqlConnectionTest()
		{
			if (_mainWindowModel.SqlConnection())
			{
                SnackBarMessageQueue.Enqueue("接続成功");

				return;
			}
			else if (!_mainWindowModel.SqlConnection())
			{
				_mainWindowModel.Server = Server.Value;
				_mainWindowModel.User = User.Value;
				_mainWindowModel.Database = Database.Value;
				_mainWindowModel.Password = Password.Value;
				_mainWindowModel.PersistSecurityInfo = PersistSecurityInfo.Value;

                SnackBarMessageQueue.Enqueue("接続成功");

				return;
            }

            SnackBarMessageQueue.Enqueue("接続失敗");
        }

		public ReactiveCommand RegistSqlConnectionString { get; }
		private void OnRegistSqlConnectionString()
		{
			// 接続文字列入力フィールドへ入力されていない場合
			if (Server is null || User is null || Database is null || Password is null)
			{
				// 空文字列を生成
				string emptyValue = string.Empty;

				// 接続文字列をJSONファイルへシリアル化
				_mainWindowModel.JsonSerialize(emptyValue, emptyValue, emptyValue, emptyValue, PersistSecurityInfo.Value);
			}
			else
			{
				// 接続文字列をJSONファイルへシリアル化
				_mainWindowModel.JsonSerialize(Server.Value, User.Value, Database.Value, Password.Value, PersistSecurityInfo.Value);
			}

			// 「language」ComboBox設定用Listを取得
			ObservableCollection<string> languageItems = new ObservableCollection<string>();
			foreach (string languageItem in _mainWindowModel.LanguageComboBoxItemSetting())
			{
				languageItems.Add(languageItem);
			}
			// 
			LanguageItemCollection = languageItems;
		}

		public ReactiveCommand CheckedStateChanged { get; }
		private void OnCheckedStateChanged()
		{
			if (UseApplicationIsChecked == false)
			{
				UseApplicationComboBoxVisibility = Visibility.Visible;
				UseApplicationTextBoxVisibility = Visibility.Collapsed;
			}
			else if (UseApplicationIsChecked == true)
			{
				UseApplicationComboBoxVisibility = Visibility.Collapsed;
				UseApplicationTextBoxVisibility = Visibility.Visible;
			}
		}

		public ReactiveCommand CodeNumberRegist { get; }
		private void OnCodeNumberRegist()
		{
			// 一時的な
			string developName = string.Empty;
			string codeName = string.Empty;
			string useApplication = string.Empty;
			string useApplicationSub = string.Empty;
			string referenceNumber = string.Empty;
			string oldNumber = string.Empty;
			string newNumber = string.Empty;
			string inheritenceNumber = string.Empty;
			string explanation = string.Empty;
			string summary = string.Empty;

			// 開発番号の生成
			string codeNumberClassificationString =
				_mainWindowModel.CodeNumberClassification(DevelopmentSelectedValue.Value, LanguageSelectedValue.Value);
			string codeNumber = $"{codeNumberClassificationString}{(RowsCount + 1).ToString("000000")}";

			if (DevelopName is not null) { developName = DevelopName.Value; }
			if (CodeName is not null) { codeName = CodeName.Value; }

			if (UseApplicationSelectedValue is not null || UseApplicationSubSelectedValue is not null)
			{
				useApplication = _mainWindowModel.RegistCodeNumberComboBoxItemSelect(UseApplicationSelectedValue.Value);
				useApplicationSub = _mainWindowModel.RegistCodeNumberComboBoxItemSelect(UseApplicationSubSelectedValue.Value);
				
				useApplication = $"{useApplication}_{useApplicationSub}";
			}
			else if (UseApplicationManual is not null)
			{
				useApplication = UseApplicationManual.Value;
			}

			if (ReferenceNumber is not null) { referenceNumber = ReferenceNumber.Value; }
			if (OldNumber is not null) { oldNumber = OldNumber.Value; }
			if (NewNumber is not null) { newNumber = NewNumber.Value; }
			if (InheritenceNumber is not null) { inheritenceNumber = InheritenceNumber.Value; }

			if (Explanation is not null) { explanation = Explanation.Value; }
			if (Summary is not null) { summary = Summary.Value; }

			List<string> values = new List<string>()
			{
				codeNumberClassificationString,
				developName,
				codeName,
				DateTime.Now.ToString(),
				useApplication,
				referenceNumber, oldNumber, newNumber, inheritenceNumber,
				explanation, summary
			};

			// 
			if (!_mainWindowModel.RegistExecute(values))
			{
				string message = $"{_mainWindowModel.ResultMessage}\n{_mainWindowModel.ExceptionMessage}";
				SnackBarMessageQueue.Enqueue(message);
			}

			int a = 0;
		}
		public ReactiveCommand TextChanged { get; }
		public ReactiveCommand ComboChanged { get; }
		private void OnChangedValue()
		{
			if (DevelopName is not null || UseApplicationManual is not null)
			{
				RegistButtonVisibility.Value = Visibility.Visible;
				return;
			}

			RegistButtonVisibility.Value = Visibility.Collapsed;
		}
		public ReactiveCommand SearchTextClaar { get; }
		private void OnSearchTextClaar()
		{
			SearchTextDevName.Value = string.Empty;
			SearchTextCodeName.Value = string.Empty;

			int i = 0;
		}
		public ReactiveCommand TableSelectionChange { get; }
		private void OnTableSelectionChange()
		{
			// ComboBoxの中身を取得する
			string selectedValue = DBTableSelectedValue.Value;

			// 
			var items = _mainWindowModel.GetInPutFieldColumns(selectedValue).Select(x => new DBTableColumnFieldItem(x)).ToList();
			DBTableColumnsFieldItems.Value = items;

			// DataGridへ出力
			TableDataGridItem.Value = _mainWindowModel.MasterTableData(selectedValue);
		}


		// 「IDestructible」の実装
		public void Destroy()
		{
			disposables.Dispose();
		}
	}
}
