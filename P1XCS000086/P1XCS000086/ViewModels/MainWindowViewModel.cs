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


		// ReactiveProperties
		public ReactivePropertySlim<string> Server { get; set; }
		public ReactivePropertySlim<string> User { get; set; }
		public ReactivePropertySlim<string> Database { get; set; }
		public ReactivePropertySlim<string> Password { get; set; }
		public ReactivePropertySlim<bool> PersistSecurityInfo { get; set; }

		public ReactivePropertySlim<string> LanguageSelectedValue { get; set; }
		public ReactivePropertySlim<string> DevelopmentSelectedValue { get; set; }
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

		public ReactivePropertySlim<bool> SnackIsActive { get; }
		public ReactivePropertySlim<SnackbarMessageQueue> SnackBarMessageQueue { get; private set; }

		public ReactivePropertySlim<string> ConnectionDatabase { get; }


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


			// Properties
			LanguageSelectedValue = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			DevelopmentSelectedValue = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);			

			// JSONファイルが存在していない場合
			IJsonExtention jsonExtention = new JsonExtention();
			IJsonConnectionStrings jsonConnString = mainWindowModel.JsonDeserialize();
			if (jsonExtention.PathCheckAndGenerate())
			{
				// Properties SQL Connection
				Server = new ReactivePropertySlim<string>(jsonConnString.Server).AddTo(disposables);
				User = new ReactivePropertySlim<string>(jsonConnString.User).AddTo(disposables);
				Database = new ReactivePropertySlim<string>(jsonConnString.DatabaseName).AddTo(disposables);
				Password = new ReactivePropertySlim<string>(jsonConnString.Password).AddTo(disposables);
				PersistSecurityInfo = new ReactivePropertySlim<bool>(jsonConnString.PersistSecurityInfo).AddTo(disposables);

				// ComboBoxItem Property
				ObservableCollection<string> languageItems = new ObservableCollection<string>();
				foreach (string item in mainWindowModel.LanguageComboBoxItemSetting())
				{
					languageItems.Add(item);
				}
				LanguageItemCollection = languageItems;

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
			if (DevItemSelectedIndex == -1)
			{
				return;
			}
			DataTable dt = _mainWindowModel.CodeManagerDataGridItemSetting(DevelopmentSelectedValue.Value, LanguageSelectedValue.Value);
			DataGridItems = dt;

			RowsCount = dt.Rows.Count;
		}

		public ReactiveCommand SqlConnectionTest { get; }
		private void OnSqlConnectionTest()
		{
			SnackbarMessageQueue messageQueue = new SnackbarMessageQueue(new TimeSpan(2));

			if (_mainWindowModel.SqlConnection())
			{
				messageQueue.Enqueue("接続成功");
				SnackBarMessageQueue = new ReactivePropertySlim<SnackbarMessageQueue>(messageQueue).AddTo(disposables);

				return;
			}

			messageQueue.Enqueue("接続失敗");
			SnackBarMessageQueue = new ReactivePropertySlim<SnackbarMessageQueue>(messageQueue).AddTo(disposables);
		}

		public ReactiveCommand RegistSqlConnectionString { get; }
		private void OnRegistSqlConnectionString()
		{
			// 接続文字列をJSONファイルへシリアル化
			_mainWindowModel.JsonSerialize(Server.Value, User.Value, Database.Value, Password.Value, PersistSecurityInfo.Value);

			// 「language」ComboBox設定用Listを取得
			ObservableCollection<string> languageItems = new ObservableCollection<string>();
			foreach (string languageItem in _mainWindowModel.LanguageComboBoxItemSetting())
			{
				languageItems.Add(languageItem);
			}
			// 
			LanguageItemCollection = languageItems;
		}


		// 「IDestructible」の実装
		public void Destroy()
		{
			disposables.Dispose();
		}
	}
}
