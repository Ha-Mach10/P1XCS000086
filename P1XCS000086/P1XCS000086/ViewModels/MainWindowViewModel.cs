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
		private List<string> _developmentItemCollections;
		private DataTable _dataGridItems;


		// ReactiveProperties
		public ReactivePropertySlim<string> Server { get; set; }
		public ReactivePropertySlim<string> User { get; set; }
		public ReactivePropertySlim<string> Database { get; set; }
		public ReactivePropertySlim<string> Password { get; set; }
		public ReactivePropertySlim<bool> PersistSecurityInfo { get; set; }

		public ReactivePropertySlim<string> LanguageSelectedValue { get; set; }
		public ReactivePropertySlim<string> DevelopmentSelectedValue { get; set; }
		public ReactivePropertySlim<List<string>> LanguageItemCollections { get; private set; }
		// public ReactivePropertySlim<List<string>> DevelopmentItemCollections { get; private set; }
		// public ReactivePropertySlim<DataTable> DataGridItems { get; private set; }
		public List<string> DevelopmentItemCollections
		{
			get => _developmentItemCollections;
			set => SetProperty(ref _developmentItemCollections, value);
		}
		public DataTable DataGridItems
		{
			get => _dataGridItems;
			set => SetProperty(ref _dataGridItems, value);
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
				List<string> languageItems = mainWindowModel.LanguageComboBoxItemSetting();
				LanguageItemCollections = new ReactivePropertySlim<List<string>>(languageItems).AddTo(disposables);
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
			List<string> developmentItems = _mainWindowModel.DevelopmentComboBoxItemSetting(selectedValue);
			DevelopmentItemCollections = developmentItems;
			// DevelopmentItemCollections = new ReactivePropertySlim<List<string>>(developmentItems).AddTo(disposables);

			// DataGrid用のDataTableを取得
			DataTable dt = _mainWindowModel.CodeManagerDataGridItemSetting(selectedValue);
			// DataGridItems = new ReactivePropertySlim<DataTable>(dt).AddTo(disposables);
			DataGridItems = _mainWindowModel.CodeManagerDataGridItemSetting(selectedValue);
			// DataGridItems = dt;
			int i = 0;
		}
		public ReactiveCommand DevelopmentTypeComboChange { get; }
		private void OnDevelopmentTypeComboChange()
		{
			// Language文字列ComboBoxから取得（Like検索用）
			string selectedValue = DevelopmentSelectedValue.Value;
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
			List<string> languageItems = _mainWindowModel.LanguageComboBoxItemSetting();
			// 
			LanguageItemCollections = new ReactivePropertySlim<List<string>>(languageItems).AddTo(disposables);
		}


		// 「IDestructible」の実装
		public void Destroy()
		{
			disposables.Dispose();
		}
	}
}
