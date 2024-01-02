using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;

using P1XCS000086.Services;
using P1XCS000086.Services.Models;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Interfaces;

using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using P1XCS000086.Services.Objects;
using System.Data;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Xps.Serialization;
using System.Security.Cryptography.Xml;
using System.CodeDom;
using System.Reflection.Emit;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Windows;
using Reactive.Bindings.ObjectExtensions;
using System.Reactive;
using System.Configuration;
using System.Threading.Tasks;

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
		private CompositeDisposable disposables = new CompositeDisposable();


		// 
		private ObservableCollection<string> _languageItemCollection;
		private ObservableCollection<string> _developmentItemCollections;
		private DataTable _dataGridItems;
		private int _rowsCount;
		private int _devItemSelectedIndex;
		private bool _isDevItemSelected = false;

		private bool _useApplicationIsChecked;

		private Visibility _useApplicationComboBoxVisibility;
		private Visibility _useApplicationTextBoxVisibility;

		private SnackbarMessageQueue _snackBarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));
		private bool _snackIsActive = false;


        #region ReactiveProperties
        public ReactivePropertySlim<string> Server { get; }
		public ReactivePropertySlim<string> User { get; }
		public ReactivePropertySlim<string> Database { get; }
		public ReactivePropertySlim<string> Password { get; }
		public ReactivePropertySlim<bool> PersistSecurityInfo { get; }

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

		// public ReactivePropertySlim<bool> UseApplicationIsChecked { get; set; }
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


		public Visibility UseApplicationComboBoxVisibility
		{
			get => _useApplicationComboBoxVisibility;
			set => SetProperty(ref _useApplicationComboBoxVisibility, value);
		}
		public Visibility UseApplicationTextBoxVisibility
		{
			get => _useApplicationTextBoxVisibility;
			set => SetProperty(ref _useApplicationTextBoxVisibility, value);
		}


		public bool SnackIsActive
		{
			get => _snackIsActive;
			set => SetProperty(ref _snackIsActive, value);
		}
		public SnackbarMessageQueue SnackBarMessageQueue
		{
			get => _snackBarMessageQueue;
			set => SetProperty(ref _snackBarMessageQueue, value);
		}


		public ReactivePropertySlim<string> ConnectionDatabase { get; }
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="regionManager"></param>
		public MainWindowViewModel(IRegionManager regionManager)
		{
			_regionManager = regionManager;

			// MainWindowModelをインターフェース(IMainWindowModel)から生成
			IMainWindowModel mainWindowModel = new MainWindowModel();
			_mainWindowModel = mainWindowModel;


			// ----------------------------------------------------------------------------------
			// Properties
			// ----------------------------------------------------------------------------------

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


			// JSONファイルが存在していない場合
			IJsonExtention jsonExtention = new JsonExtention();
			IJsonConnectionStrings jsonConnString = mainWindowModel.JsonDeserialize();
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

				List<string> useApplicationItems = mainWindowModel.UseApplicationComboBoxItemSetting();
				List<string> useApplicationSubItems = mainWindowModel.UseApplicationSubComboBoxItemSetting();
				UseApplication = new ReactivePropertySlim<List<string>>(useApplicationItems).AddTo(disposables);
				UseApplicationSub = new ReactivePropertySlim<List<string>>(useApplicationSubItems).AddTo(disposables);
			}
			else if (jsonConnString is null)
			{
                Server = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                User = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                Database = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                Password = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
                PersistSecurityInfo = new ReactivePropertySlim<bool>(false).AddTo(disposables);
            }


			// Command Events
			LanguageTypeComboChange = new ReactiveCommand();
			LanguageTypeComboChange.Subscribe(() => OnLanguageTypeComboChange()).AddTo(disposables);
			DevelopmentTypeComboChange = new ReactiveCommand();
			DevelopmentTypeComboChange.Subscribe(() => OnDevelopmentTypeComboChange()).AddTo(disposables);

			SqlConnectionTest = new ReactiveCommand();
			SqlConnectionTest.Subscribe(() => OnSqlConnectionTest()).AddTo(disposables);
			RegistSqlConnectionString = new ReactiveCommand();
			RegistSqlConnectionString.Subscribe(() => OnRegistSqlConnectionString()).AddTo(disposables);

			CheckedStateChanged = new ReactiveCommand();
			CheckedStateChanged.Subscribe(() => OnCheckedStateChanged()).AddTo(disposables);

			CodeNumberRegist = new ReactiveCommand();
			CodeNumberRegist.Subscribe(() => OnCodeNumberRegist()).AddTo(disposables);
		}


		// Command Events
		public ReactiveCommand LanguageTypeComboChange { get; }
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

			RowsCount = dt.Rows.Count;

			DevItemSelectedIndex = -1;
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
			// 接続文字列をJSONファイルへシリアル化
			// 接続文字列入力フィールドへ入力されていない場合
			if (Server is null || User is null || Database is null || Password is null)
			{
				string emptyValue = string.Empty;
				_mainWindowModel.JsonSerialize(emptyValue, emptyValue, emptyValue, emptyValue, PersistSecurityInfo.Value);
			}
			// 入力されている場合
			else
			{
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

			if (DevelopName is not null) { developName = DevelopName.Value; }
			if (CodeName is not null) { codeName = CodeName.Value; }

			if (UseApplicationSelectedValue is not null || UseApplicationSubSelectedValue is not null)
			{
				useApplication = _mainWindowModel.RegistCodeNumberComboBoxItemSelect(UseApplicationSelectedValue.Value);
				useApplicationSub = _mainWindowModel.RegistCodeNumberComboBoxItemSelect(UseApplicationSubSelectedValue.Value);
			}
			else if (UseApplicationManual is not null)
			{
				useApplication = UseApplicationManual.Value;
			}

			if (ReferenceNumber is not null) { referenceNumber = ReferenceNumber.Value; }
			if (OldNumber is not null) { oldNumber = OldNumber.Value; }
			if (NewNumber is not null) { newNumber = NewNumber.Value; }
			if (InheritenceNumber is not null) { inheritenceNumber = InheritenceNumber.Value; }

			if (Explanation is not null) {  explanation = Explanation.Value; }
			if (Summary is not null) {  summary = Summary.Value; }
			
			int i = 0;
		}


		// 「IDestructible」の実装
		public void Destroy()
		{
			disposables.Dispose();
		}
	}
}
