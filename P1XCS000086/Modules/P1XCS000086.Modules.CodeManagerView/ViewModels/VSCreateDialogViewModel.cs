using MaterialDesignThemes.Wpf;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Modules.CodeManagerView.Domains;
using P1XCS000086.Modules.CodeManagerView.InnerModels;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.ObjectExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class VSCreateDialogViewModel : RegionViewModelBase, IDialogAware, INotifyPropertyChanged
	{
		// ---------------------------------------------------------------
		// Reference Sources
		// --------------------------------------------------------------- 
		// 
		// Title : [C#][Prism][備忘録]PrismのIDialogServiceを使用してダイアログを表示する
		// Url   : https://qiita.com/minoura_a/items/a97a31776f536b0630ac
		// 
		// Title : WPF Prism 8.0 の新機能「複数のダイアログウィンドウのサポートが追加」について
		// Url   : https://houwa-js.co.jp/2021/04/prism80/
		// --------------------------------------------------------------- 





		// ---------------------------------------------------------------
		// Static Fields
		// --------------------------------------------------------------- 

		private static bool _isEnterd = true;



		// ---------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------- 

		private IRegionManager _regionManager;
		private IDialogService _dialogService;
		private IVSCreateDialogModel _model;



		// ---------------------------------------------------------------
		// Reactive Properties
		// --------------------------------------------------------------- 

		public ReactivePropertySlim<Visibility> ListViewVisibility { get; }
		public ReactivePropertySlim<Visibility> ProgressVisibility { get; }

		public ReactiveCollection<string> LanguageFilterItems { get; }
		public ReactiveCollection<string> PlatformFilterItems { get; }
		public ReactiveCollection<string> ProjectTypeFilterItems { get; }
		public ReactivePropertySlim<string> SelectedLanguageItem { get; }
		public ReactivePropertySlim<string> SelectedPlatformItem { get; }
		public ReactivePropertySlim<string> SelectedProjectTypeItem { get; }

		public ReactiveCollection<ProjectTypeItem> ProjectTypes { get; }
		public ReactivePropertySlim<ProjectTypeItem> SelectedProjectType { get; }



		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		public VSCreateDialogViewModel(IRegionManager regionManager, IDialogService dialogService, IVSCreateDialogModel model)
			: base(regionManager)
		{
			// インジェクション
			_regionManager = regionManager;
			_dialogService = dialogService;
			_model = model;

			if (_isEnterd)
			{
				 Task.Run(SetUiAutomationVisualStudioContent);
				_isEnterd = false;
			}

			// Properties
			ListViewVisibility = new ReactivePropertySlim<Visibility>(Visibility.Collapsed);
			ProgressVisibility = new ReactivePropertySlim<Visibility>(Visibility.Visible);

			LanguageFilterItems = new ReactiveCollection<string>().AddTo(_disposables);
			PlatformFilterItems = new ReactiveCollection<string>().AddTo(_disposables);
			ProjectTypeFilterItems = new ReactiveCollection<string>().AddTo(_disposables);
			SelectedLanguageItem = new ReactivePropertySlim<string>(string.Empty);
			SelectedPlatformItem = new ReactivePropertySlim<string>(string.Empty);
			SelectedProjectTypeItem = new ReactivePropertySlim<string>(string.Empty);

			ProjectTypes = new ReactiveCollection<ProjectTypeItem>().AddTo(_disposables);
			SelectedProjectType = new ReactivePropertySlim<ProjectTypeItem>().AddTo(_disposables);


			// Commands
			AcceptCreate = new ReactiveCommandSlim();
			AcceptCreate.Subscribe(OnAcceptCreate).AddTo(_disposables);
			Cancel = new ReactiveCommandSlim();
			Cancel.Subscribe(OnCancel).AddTo(_disposables);
		}



		// ---------------------------------------------------------------
		// Implementation Interface
		// ---------------------------------------------------------------

		#region IDialogAware

		private string _title = "Test";
		public string Title
		{
			get => _title;
			set => _title = value;
		}
		/// <summary>
		/// Closeリクエスト要求
		/// </summary>
		public event Action<IDialogResult> RequestClose;

		public bool CanCloseDialog() => true;
		public void OnDialogClosed()
		{

		}
		public void OnDialogOpened(IDialogParameters parameters)
		{
			var param = parameters.GetValue<string>("test");

			// 初期化等
		}

		#endregion



		// ---------------------------------------------------------------
		// Events
		// --------------------------------------------------------------- 





		// ---------------------------------------------------------------
		// Reactive Commands
		// --------------------------------------------------------------- 

		public ReactiveCommandSlim AcceptCreate { get; }
		private void OnAcceptCreate()
		{
			var result = new DialogResult(ButtonResult.OK);

			result.Parameters.Add("resultKey", "");

			// ダイアログを閉じる
			RequestClose?.Invoke(result);
		}
		public ReactiveCommandSlim Cancel { get; }
		private void OnCancel()
		{
			var result = new DialogResult(ButtonResult.Cancel);

			result.Parameters.Add("resultKey", "");

			// ダイアログを閉じる
			RequestClose?.Invoke(result);
		}



		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 





		// ---------------------------------------------------------------
		// Private Methods
		// --------------------------------------------------------------- 

		private async void SetUiAutomationVisualStudioContent()
		{
			// Initial Process

			// Visual Sutudioのメインウィンドウハンドルを取得
			var hWnd = await _model.FindProcessMainwindowHandle(5000);
			// AutomationElementを取得
			AutomationElement mainWindow = AutomationElement.FromHandle(hWnd);
			// ウィンドウのAutomationElementを取得する
			// AutomationElement mainWindow = UiAutomationInnerModel.GetMainWindowElemnt(_model, 5000);
			// ウィンドウのステータスを変更
			UiAutomationInnerModel.MainWindowChangeScreen(mainWindow, WindowVisualState.Maximized);
			// Visual Studioの各種コントロールを操作
			UiAutomationInnerModel.PushButtonByName(mainWindow, "新しいプロジェクトの作成", 2000);
			await Task.Delay(2000);
			// UiAutomationInnerModel.PushButtonByName(mainWindow, "すべてクリア(_C)");

			foreach (string comboName in new List<string>() { "LanguageFilter", "PlatformFilter", "ProjectTypeFilter" })
			{
				UiAutomationInnerModel.TryGetScrollableComboBoxElement(mainWindow, comboName, out List<string> comboTtems);

				switch (comboName)
				{
					case "LanguageFilter":
						LanguageFilterItems.AddRangeOnScheduler(comboTtems);
						break;
					case "PlatformFilter":
						PlatformFilterItems.AddRangeOnScheduler(comboTtems);
						break;
					case "ProjectTypeFilter":
						ProjectTypeFilterItems.AddRangeOnScheduler(comboTtems);
						break;
				}
			}


			if (UiAutomationInnerModel.TryGetScrollableListViewElement(mainWindow, "一覧項目", out ScrollPattern scrollPattern))
			{
				UiAutomationInnerModel.ScrollableElementScrolling(scrollPattern);
			}
			var items = UiAutomationInnerModel.GetListViewContents(mainWindow, "一覧項目", "Windows デスクトップ アプリケーション is unpinned", "区切り線");

			WindowPattern windowPattern = mainWindow.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
			windowPattern.Close();

			int a = 0;

			ProgressVisibility.Value = Visibility.Collapsed;
			ListViewVisibility.Value = Visibility.Visible;
		}
	}
}
