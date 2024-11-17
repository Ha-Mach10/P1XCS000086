using MaterialDesignThemes.Wpf;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Modules.CodeManagerView.Domains;
using P1XCS000086.Modules.CodeManagerView.InnerModels;
using P1XCS000086.Modules.CodeManagerView.Views;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;

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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeRegisterViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// ---------------------------------------------------------------
		// Static Fields
		// --------------------------------------------------------------- 

		private static bool s_isEntried = true;
		private static string s_codeDir = string.Empty;



		// ---------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------- 

		private IRegionManager _regionManager;
		private IDialogService _dialogService;
		private ICodeRegisterModel _model;



		// ---------------------------------------------------------------
		// Reactive Properties
		// --------------------------------------------------------------- 

		public ReactivePropertySlim<bool> IsPaneOc { get; }

		public ReactivePropertySlim<int> RecordCount { get; }

		public ReactivePropertySlim<List<string>> LangTypes { get; }
		public ReactivePropertySlim<List<string>> DevTypes { get; }
		public ReactivePropertySlim<string> SelectedLangType { get; }
		public ReactivePropertySlim<string> SelectedDevType { get; }
		public ReactivePropertySlim<int> SelectedIndexDevType { get; }

		[RegularExpression("^[0-9A-z_]{1,50}$", ErrorMessage = "規定の文字のみを含んだ文字列を入力してください")]
		[MinLength(1, ErrorMessage ="１文字以上の入力が必要です")]
		public ReactiveProperty<string> DevelopName { get; }

		public ReactivePropertySlim<List<string>> UseAppMajor { get; }
		public ReactivePropertySlim<List<string>> UseAppRange { get; }
		[Required(ErrorMessage = "必須の選択項目です")]
		public ReactiveProperty<string> SelectedUseAppMajor { get; }
		public ReactivePropertySlim<string> SelectedUseAppRange { get; }
		public ReactivePropertySlim<int> SelectedIndexUseAppRange { get; }

		public ReactivePropertySlim<List<string>> UiFramework { get; }
		public ReactivePropertySlim<string> SelectedUiFramework { get; }
		public ReactivePropertySlim<int> SelectedIndexUiFramework { get; }

		public ReactivePropertySlim<string> Explanation { get; }
		public ReactivePropertySlim<string> Summary { get; }


		public ReactivePropertySlim<DataTable> Table { get; }
		public DataRow Row { get; private set; }

		public ReactivePropertySlim<bool> SnackbarIsActive { get; }
		public ReactivePropertySlim<string> ResultMessage { get; }

		public ReactivePropertySlim<string> DialogResultMessage { get; }

		public ReactiveCollection<ContextMenuItem> ContextMenuItems { get; }

		public ReactiveCollection<SelectedRowPropertyField> SelectedRowPropertyFieldItems { get; }



		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		public CodeRegisterViewModel(IRegionManager regionManager, IDialogService dialogService, ICodeRegisterModel model)
			: base(regionManager)
		{
			// インジェクション
			_regionManager = regionManager;
			_dialogService = dialogService;
			_model = model;


			// このビューモデルの生存
			KeepAlive = false;

			#region Properties

			IsPaneOc = new ReactivePropertySlim<bool>(true);

			RecordCount = new ReactivePropertySlim<int>(0);

			LangTypes = new ReactivePropertySlim<List<string>>(_model.LangTypes).AddTo(_disposables);
			DevTypes = new ReactivePropertySlim<List<string>>(null).AddTo(_disposables);

			SelectedLangType = new ReactivePropertySlim<string>(string.Empty);
			SelectedDevType = new ReactivePropertySlim<string>(string.Empty);

			SelectedIndexDevType = new ReactivePropertySlim<int>(0);

			DevelopName = new ReactiveProperty<string>(string.Empty)
				.SetValidateAttribute(() => DevelopName)
				.AddTo(_disposables);

			UseAppMajor = new ReactivePropertySlim<List<string>>(_model.UseAppMajor);
			UseAppRange = new ReactivePropertySlim<List<string>>(_model.UseAppRange).AddTo(_disposables);
			SelectedUseAppMajor = new ReactiveProperty<string>(string.Empty)
				.SetValidateAttribute(() => SelectedUseAppMajor)
				.AddTo(_disposables);
			SelectedUseAppRange = new ReactivePropertySlim<string>(string.Empty);
			SelectedIndexUseAppRange = new ReactivePropertySlim<int>(-1);

			UiFramework = new ReactivePropertySlim<List<string>>(new()).AddTo(_disposables);
			SelectedUiFramework = new ReactivePropertySlim<string>(string.Empty);
			SelectedIndexUiFramework = new ReactivePropertySlim<int>(0);

			Explanation = new ReactivePropertySlim<string>(string.Empty);
			Summary = new ReactivePropertySlim<string>(string.Empty);

			Table = new ReactivePropertySlim<DataTable>(null).AddTo(_disposables);

			SnackbarIsActive = new ReactivePropertySlim<bool>(false);
			ResultMessage = new ReactivePropertySlim<string>(string.Empty);

			DialogResultMessage = new ReactivePropertySlim<string>(string.Empty);

			// ContextMenuItemクラスのItemsプロパティへ追加
			ContextMenuItem.AddRangeItems(new List<ContextMenuItem>()
			{
				new ContextMenuItem(_regionManager, "親フォルダ―を開く", OnContextMenuOpenParentFolder, true),
				new ContextMenuItem(_regionManager, "Visual Studio 2019を起動", OnContextMenuAwakeVS2019, true),
				new ContextMenuItem(_regionManager, "Visual Studio 2022を起動", OnContextMenuAwakeVS2022, true)
			});
			ContextMenuItems = new ReactiveCollection<ContextMenuItem>().AddTo(_disposables);

			SelectedRowPropertyFieldItems = new ReactiveCollection<SelectedRowPropertyField>().AddTo(_disposables);

		#endregion

		// 

		// Commands
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);

			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);

			UseAppMajorSelectionChanged = new ReactiveCommandSlim();
			UseAppMajorSelectionChanged.Subscribe(OnUseAppMajorSelectionChanged).AddTo(_disposables);
			// 
			RegistCodeNumber =
				new[]
				{
					DevelopName.ObserveHasErrors,
					SelectedUseAppMajor.ObserveHasErrors,
				}
				.CombineLatestValuesAreAllFalse()
				.ToReactiveCommandSlim()
				.AddTo(_disposables);
			RegistCodeNumber.Subscribe(OnRegistCodeNumber).AddTo(_disposables);
			RegistCodeNumberAndCreateProject =
				new[]
				{
					DevelopName.ObserveHasErrors,
					SelectedUseAppMajor.ObserveHasErrors,
				}
				.CombineLatestValuesAreAllFalse()
				.ToReactiveCommandSlim()
				.AddTo(_disposables);

			// 
			DataGridRowSelectionChanged = new ReactiveCommandSlim();
			DataGridRowSelectionChanged.Subscribe(param =>
			{
				if (param is DataRowView dataRowView)
				{
					// nullでない場合
					if (dataRowView is null) return;

					var items = _model.GetSelectedRowPropertyFieldItem(dataRowView)
									  .Select(item => new SelectedRowPropertyField(_regionManager, item.columnNames, item.propertyText))
									  .ToList();
					SelectedRowPropertyFieldItems.Clear();
					SelectedRowPropertyFieldItems.AddRangeOnScheduler(items);


					Row = dataRowView.Row;
					(string developNumber, string dirFileName) = GetSelectedDevelopNumber(Row);

					// 「developNumber」がnullまたはstring.Emptyになっている
					if (string.IsNullOrEmpty(developNumber) is true)
					{
						ContextMenuItem.ClearParticalItem();
						ContextMenuItems.AddRangeOnScheduler(ContextMenuItem.Items);
					}
					else
					{
						// 「dirFileName」がnullまたはstring.Emptyである場合
						if (string.IsNullOrEmpty(dirFileName))
						{
							// デフォルトの値以外を削除
							ContextMenuItem.ClearParticalItem();
							// メニュー用アイテムを追加
							ContextMenuItem.AddItem(new ContextMenuItem(_regionManager, $"{developNumber}を作成する", OnContextMenuCreateProject));

							ContextMenuItems.Clear();
							// 別スレッドでアイテムのリストを追加
							ContextMenuItems.AddRangeOnScheduler(ContextMenuItem.Items);

							return;
						}

						// デフォルトの値以外を削除
						ContextMenuItem.ClearParticalItem();
						// メニュー用アイテムを追加
						ContextMenuItem.AddRangeItems(new List<ContextMenuItem>()
						{
							new ContextMenuItem(_regionManager, $"{developNumber}フォルダをエクスプローラーで開く", OnContextMenuOpenProjectFolder),
							new ContextMenuItem(_regionManager, $"{developNumber}プロジェクトを開く", OnContextMenuOpenProject)
						});

						// 
						ContextMenuItems.Clear();
						// 別スレッドでアイテムのリストを追加
						ContextMenuItems.AddRangeOnScheduler(ContextMenuItem.Items);
					}
				}
			}).AddTo(_disposables);
		}



		// ---------------------------------------------------------------
		// Override Methods
		// --------------------------------------------------------------- 

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			ContextMenuItem.ItemsClear();
		}



		// ---------------------------------------------------------------
		// Reactive Commands
		// --------------------------------------------------------------- 

		/// <summary>
		/// 言語種別のコンボボックスの項目選択時に発火
		/// コンテキストメニューをリセットし、開発種別コンボボックス及びUIフレームワーク種別選択コンボボックスの選択項目へ値を再セットする
		/// </summary>
		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{
			// コレクションをクリア（例外回避）
			SelectedRowPropertyFieldItems.Clear();
			ContextMenuItems.Clear();


			// 開発種別を選択するコンボボックス用Listに格納
			DevTypes.Value = _model.SetDevType(SelectedLangType.Value);
			SelectedIndexDevType.Value = -1;

			UiFramework.Value = _model.SetFrameworkName(SelectedLangType.Value);
			SelectedIndexUiFramework.Value = -1;

			// 取得した言語種別からディレクトリをDataBaseから取得
			s_codeDir = _model.GetProjectDirTableToDirPath(SelectedLangType.Value);
		}

		/// <summary>
		/// 開発種別のコンボボックスの項目選択時に発火
		/// DataGridのデータを更新し、レコード数を表示する
		/// </summary>
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{
			Table.Value = _model.SetTable(SelectedLangType.Value, SelectedDevType.Value);
			RecordCount.Value = Table.Value.Rows.Count;
		}

		/// <summary>
		/// ？？？？？？使用しない？？？？？？
		/// </summary>
		public ReactiveCommandSlim UseAppMajorSelectionChanged { get; }
		private void OnUseAppMajorSelectionChanged()
		{
			// 使わんかも....
		}

		/// <summary>
		/// 新しい開発コードを登録する
		/// </summary>
		public ReactiveCommandSlim RegistCodeNumber { get; }
		private void OnRegistCodeNumber()
		{
			// 各種プロパティを取得し、データベースへ挿入するためのフォーマットに整形
			string developNumber = $"{_model.CodeDevType}{(RecordCount.Value + 1).ToString("000000")}";
			string developName = DevelopName.Value;
			string uiFramework = SelectedUiFramework.Value;
			string createdOn = DateTime.Today.ToString("yyyy-MM-dd");
			string useApplication = _model.SetUseApplication(SelectedUseAppMajor.Value);
			if (string.IsNullOrEmpty(SelectedUseAppRange.Value) is false)
			{
				string useAppSub = _model.SetUseApplication(SelectedUseAppRange.Value);
				useApplication = $"{useApplication}_{useAppSub}";
			}
			string explanation = Explanation.Value;
			string summary = Summary.Value;

			// データベースへのインサートを実行
			_model.InsertCodeManager(developNumber, developName, uiFramework, createdOn, useApplication, explanation, summary);

			// スナックバーへ上記インサートの結果を表示する
			// ※なぜかうまく表示できていないので、改善の必要あり
			SnackbarIsActive.Value = true;
			ResultMessage.Value = _model.ResultMessage;

			// DataGridの表示を更新する
			Table.Value = _model.SetTable(SelectedLangType.Value, SelectedDevType.Value);
		}
		public ReactiveCommandSlim RegistCodeNumberAndCreateProject { get; }
		private void OnRegistCodeNumberAndCreateProject()
		{

		}

		/// <summary>
		/// スナックバーを非表示にする
		/// </summary>
		public ReactiveCommandSlim CloseSnackbar { get; }
		private void OnCloseSnackbar()
		{
			// スナックバーを非活性にする
			SnackbarIsActive.Value = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim DataGridRowSelectionChanged { get; }

		/// <summary>
		/// 現在指定しているプロジェクト番号のプロジェクトが存在する親フォルダをエクスプローラーで開く（コンテキストメニュー用のコマンド）
		/// </summary>
		private void OnContextMenuOpenParentFolder() => _model.OpenProjectParentDirectry(SelectedLangType.Value);
		/// <summary>
		/// 現在指定しているプロジェクト番号のプロジェクトフォルダをエクスプローラーで開く（コンテキストメニュー用のコマンド）
		/// </summary>
		private void OnContextMenuOpenProjectFolder()
		{
			(string developNumber, string _) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectDirectry(developNumber, SelectedLangType.Value);
		}
		/// <summary>
		/// 現在指定しているプロジェクト番号のプロジェクトファイルを開く（コンテキストメニュー用のコマンド）
		/// </summary>
		private void OnContextMenuOpenProject()
		{
			(string developNumber, string dirFileName) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectFile(developNumber, dirFileName, SelectedLangType.Value);
		}
		/// <summary>
		/// Visual Studio 2019 を起動（コンテキストメニュー用のコマンド）
		/// </summary>
		private void OnContextMenuAwakeVS2019() => _model.AwakeVS2019();
		/// <summary>
		/// Visual Studio 2022 を起動（コンテキストメニュー用のコマンド）
		/// </summary>
		private void OnContextMenuAwakeVS2022() => _model.AwakeVS2022();
		/// <summary>
		/// 
		/// </summary>
		private void OnContextMenuCreateProject()
		{
			SS();
			/*
			IDialogParameters param = new DialogParameters()
			{
				{ "test", "テストダイアログ" },
			};
			_dialogService.ShowDialog("VSCreateDialog", param, dialogResult =>
			{
				if (dialogResult.Result == ButtonResult.OK)
				{
					// var result = dialogResult.Parameters.GetValues<string>("resultParam");
				}
				else if (dialogResult.Result == ButtonResult.Cancel)
				{
					// var result = dialogResult.Parameters.GetValues<string>("resultParam");
				}
			});
			*/
		}



		// ---------------------------------------------------------------
		// Private Methods
		// --------------------------------------------------------------- 

		private async void SS()
		{
			// Initial Process

			// 
			await Task.Run(() =>
			{
				// Visual Sutudioのメインウィンドウハンドルを取得
				var hWnd = _model.FindProcessMainwindowHandle(5000);
				// AutomationElementを取得
				AutomationElement mainWindow = AutomationElement.FromHandle(hWnd.Result);
				// ウィンドウのステータスを変更
				UiAutomationInnerModel.MainWindowChangeScreen(mainWindow, WindowVisualState.Maximized);
				// Visual Studioの各種コントロールを操作
				UiAutomationInnerModel.PushButtonByName(mainWindow, "新しいプロジェクトの作成", 2000);

				while (UiAutomationInnerModel.TryFindTriggerKeyword(mainWindow, "新しいプロジェクトを構成します") is not true)
				{
					string developNumber = SelectedRowPropertyFieldItems
					.Where(x => x.TextBlockValue is "開発番号")
					.Select(x => x.TextBoxValue)
					.First();

					UiAutomationInnerModel.InputToTextBox(mainWindow, "projectNameText", developNumber);
					UiAutomationInnerModel.InputToTextBox(mainWindow, "PART_EditableTextBox", s_codeDir);
				}

				int a = 0;
			});
		}
		/// <summary>
		/// DataGridの現在指定している行から開発番号と開発ファイル名をタプルで取得するメソッド
		/// </summary>
		/// <param name="selectedRow">選択行</param>
		/// <returns>開発番号および開発ファイル名</returns>
		private (string, string) GetSelectedDevelopNumber(DataRow selectedRow)
		{
			string devNumber = selectedRow["develop_number"].ToString();
			string dirFileName = selectedRow["dir_file_name"].ToString();

			if (string.IsNullOrEmpty(dirFileName))
			{
				return (devNumber, "");
			}

			return (devNumber, dirFileName);
		}
	}
}
