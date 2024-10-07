using MaterialDesignThemes.Wpf;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Modules.CodeManagerView.Domains;
using P1XCS000086.Modules.CodeManagerView.logics;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;

using Prism.Regions;

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
using System.Xml.Serialization;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeRegisterViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private ICodeRegisterModel _model;



		#region Properties

		public ReactivePropertySlim<bool> IsPaneOc { get; }

		public ReactivePropertySlim<int> RecordCount { get; }

		public ReactivePropertySlim<List<string>> LangTypes { get; }
		public ReactivePropertySlim<List<string>> DevTypes { get; }
		public ReactivePropertySlim<string> SelectedLangType { get; }
		public ReactivePropertySlim<string> SelectedDevType { get; }
		public ReactivePropertySlim<int> SelectedIndexDevType { get; }

		[RegularExpression("^[0-9A-z_]{1,50}$", ErrorMessage = "規定の文字のみを含んだ文字列を入力してください")]
		public ReactiveProperty<string> DevelopName { get; }

		public ReactiveProperty<List<string>> UseAppMajor { get; }
		public ReactivePropertySlim<List<string>> UseAppRange { get; }
		public ReactivePropertySlim<string> SelectedUseAppMajor { get; }
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

		public ReactiveCollection<ContextMenuItem> ContextMenuItems { get; }

		public ReactiveCollection<SelectedRowPropertyField> SelectedRowPropertyFieldItems { get; }

		#endregion



		public CodeRegisterViewModel(IRegionManager regionManager, ICodeRegisterModel model)
			: base(regionManager)
		{
			// インジェクション
			_regionManager = regionManager;
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

			UseAppMajor = new ReactiveProperty<List<string>>(_model.UseAppMajor)
				.SetValidateAttribute(() => UseAppMajor)
				.AddTo(_disposables);
			UseAppRange = new ReactivePropertySlim<List<string>>(_model.UseAppRange).AddTo(_disposables);
			SelectedUseAppMajor = new ReactivePropertySlim<string>(string.Empty);
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

			List<ContextMenuItem> menuItems = new()
			{
				new ContextMenuItem(_regionManager, "親フォルダ―を開く", OnContextMenuOpenParentFolder, true),
				new ContextMenuItem(_regionManager, "Visual Studio 2019を起動", OnContextMenuAwakeVS2019, true),
				new ContextMenuItem(_regionManager, "Visual Studio 2022を起動", OnContextMenuAwakeVS2022, true)
			};
			ContextMenuItem.AddRangeItems(menuItems);
			ContextMenuItems = new ReactiveCollection<ContextMenuItem>().AddTo(_disposables);
			ContextMenuItems.AddRangeOnScheduler(ContextMenuItem.Items);

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
					UseAppMajor.ObserveHasErrors,
				}
				.CombineLatestValuesAreAllFalse()
				.ToReactiveCommand();
			RegistCodeNumber.Subscribe(OnRegistCodeNumber).AddTo(_disposables);

			// 
			DataGridRowSelectionChanged = new ReactiveCommandSlim();
			DataGridRowSelectionChanged.Subscribe(async param =>
			{
				// コマンドパラメータから受け取った値をキャスト
				DataRowView dataRowView = param as DataRowView;

				// nullでない場合
				if (dataRowView is not null)
				{
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
						ContextMenuItems.Clear();
						ContextMenuItem.ClearParticalItem();

					}
					else
					{
						// 「dirFileName」がnullまたはstring.Emptyである場合
						if (string.IsNullOrEmpty(dirFileName))
						{
							ContextMenuItems.Clear();

							ContextMenuItem.ClearParticalItem();
							ContextMenuItems.AddRangeOnScheduler(ContextMenuItem.Items);
							ContextMenuItems.AddOnScheduler(new ContextMenuItem(_regionManager, $"{developNumber}を作成する", OnContextMenuCreateProject));

							return;
						}

						List<ContextMenuItem> menuItems = new()
						{
							new ContextMenuItem(_regionManager, $"{developNumber}フォルダをエクスプローラーで開く", OnContextMenuOpenProjectFolder),
							new ContextMenuItem(_regionManager, $"{developNumber}プロジェクトを開く", OnContextMenuOpenProject)
						};
						ContextMenuItem.ClearParticalItem();
						ContextMenuItem.AddRangeItems(menuItems);

						ContextMenuItems.Clear();
						ContextMenuItems.AddRangeOnScheduler(ContextMenuItem.Items);
					}
				}
			}).AddTo(_disposables);
		}



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
		public ReactiveCommand RegistCodeNumber { get; }
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
		private void OnContextMenuOpenParentFolder()
		{
			_model.OpenProjectParentDirectry(SelectedLangType.Value);
		}
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
		private void OnContextMenuAwakeVS2019()
		{
			_model.AwakeVS2019();
		}
		/// <summary>
		/// Visual Studio 2022 を起動（コンテキストメニュー用のコマンド）
		/// </summary>
		private void OnContextMenuAwakeVS2022()
		{
			_model.AwakeVS2022();
		}
		/// <summary>
		/// 
		/// </summary>
		private async void OnContextMenuCreateProject()
		{
			// Visual Studio 2022のプロセス名とウィンドウタイトルを定数で宣言
			const string ProcessName = "devenv";
			const string WindowTitle = "Microsoft Visual Studio";

			const string CreateNewButtonName = "Button_1";


			// Visual Sutudioのメインウィンドウハンドルを取得
			var hWnd = await _model.FindProcessMainwindowHandle(10000);
			// AutomationElementを取得
			AutomationElement element = AutomationElement.FromHandle(hWnd);

			// Visual Studioの各種コントロールを操作
			PushButtonByName(element, "新しいプロジェクトの作成");
			await Task.Delay(2000);
			AA(element, "LanguageFilter");
			await Task.Delay(2000);
			PushButtonById(element, "button_Next");

			// メソッドを通っているかテストの為の表示。　いらんかも＾＾
			System.Windows.MessageBox.Show("現在作成中");
		}



		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);

			ContextMenuItem.ItemsClear();
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

		private void PushButtonById(AutomationElement element, string automationId)
		{
			// ボタンコントロールの取得
			InvokePattern button = FindElementById(element, automationId).GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			button.Invoke();
		}
		private void PushButtonByName(AutomationElement element, string name)
		{
			// ボタンコントロールの取得
			InvokePattern button = FindElementByName(element, name).First().GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			button.Invoke();
		}
		private void AA(AutomationElement element, string name)
		{
			// ***************************************************************************************************************************************************************
			// ***************************************************************************************************************************************************************

			List<(ScrollItemPattern, SelectionItemPattern, SynchronizedInputPattern, List<string>, string, AutomationElement)> listViewPatternItems = new();
			foreach (var item in FindElementByLocalizeControlType(element, "一覧項目"))
			{
				List<string> pattItems = item.GetSupportedPatterns().Select(x => x.ProgrammaticName).ToList();
				var aaItem = item.GetCurrentPattern(ScrollItemPattern.Pattern) as ScrollItemPattern;
				var aaaItem = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
				var aaaaItem = item.GetCurrentPattern(SynchronizedInputPattern.Pattern) as SynchronizedInputPattern;

				string aName = aaaItem.Current.SelectionContainer.Current.Name;

				aaaItem.Select();
				aaItem.ScrollIntoView();

				// UIElementAutomationPeer peer = UIElementAutomationPeer.FromElement(item);

				// listViewPatternItems.Add((aaItem, aaaItem, aaaaItem));
				listViewPatternItems.Add((aaItem, aaaItem, aaaaItem, pattItems, aName, item));
			}

			List<(AutomationElement, string, string)> allElementItems = new();
			foreach (var item in FindElementAll(element, "WPF"))
			{
				string itemName = item.Current.Name;
				string className = item.Current.ClassName;

				allElementItems.Add((item, itemName, className));
			}

			List<(AutomationElement, string)> listViewElementItems = new();
			foreach (var item in FindElementClassName(element, "ListBoxItem"))
			{
				string itemName = item.Current.Name;

				listViewElementItems.Add((item, itemName));
			}

			List<(Type type, string str, string prgName, int id)> valuesSsssss = new();
			foreach (var topItem in FindElementByName(element, "ListView"))
			{
				foreach (var item in topItem.GetSupportedPatterns())
				{
					Type type = item.GetType();
					string str = item.ToString();
					string prgName = item.ProgrammaticName;
					int id = item.Id;

					valuesSsssss.Add((type, str, prgName, id));
				}
			}

			// 有用
			List<(AutomationElement, List<string>, string, SelectionPattern, ExpandCollapsePattern, ItemContainerPattern, List<string>, List<AutomationElement>)> comboBoxLang = new();
			foreach (var item in FindElementByName(element, "LanguageFilter"))
			{
				List<string> pattItems = item.GetSupportedPatterns().Select(x => x.ProgrammaticName).ToList();
				var patternSelection = item.GetCurrentPattern(SelectionPattern.Pattern) as SelectionPattern;
				var patternExpandColl = item.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
				var patternItemContainer = item.GetCurrentPattern(ItemContainerPattern.Pattern) as ItemContainerPattern;
				string itemName = item.Current.Name;

				object obj = new();

				patternExpandColl.Expand();

				List<string> names = new();
				List<AutomationElement> aus = new();
				foreach (var itema in FindElementByLocalizeControlType(element, "テキスト"))
				{
					if (itema.Current.Name is "新しいプロジェクトの作成") break;

					names.Add(itema.Current.Name);
					aus.Add(itema);
				}

				comboBoxLang.Add((item, pattItems, itemName, patternSelection, patternExpandColl, patternItemContainer, names, aus));
			}

			List<(Type type, string str, string prgName, int id)> valuesAss = new();
			var ass = FindElementById(element, "ListViewTemplates").GetSupportedPatterns().ToList();
			foreach (var item in ass)
			{
				Type type = item.GetType();
				string str = item.ToString();
				string prgName = item.ProgrammaticName;
				int id = item.Id;

				AutomationPattern patternA = null;
				valuesAss.Add((type, str, prgName, id));

				try
				{
					/*
					switch (prgName)
					{
						case "SelectionPatternIdentifiers.Pattern":
							break;
						case "ScrollPatternIdentifiers.Pattern":
							break;
						case "SynchronizedInputPatternIdentifiers.Pattern":
							break;
						case "ItemContainerPatternIdentifiers.Pattern":
							break;
					}
					*/

					// var pattObj = FindElementById(element, "ListViewTemplates").GetCurrentPattern(item) as ;

					// valuesAss.Add((type, str, prgName, id, pattObj));
				}
				catch(Exception ex)
				{
					// a
				}
			}

			// ***************************************************************************************************************************************************************
			// ***************************************************************************************************************************************************************


			TreeNode treeNode = new();
			WalkControlElements(element, treeNode);


			Dictionary<string, string> keyValuePairs = new();

			var combolang = FindElementById(element, "ComboBox_1");
			AutomationPattern comboPatt = null;
			foreach (var pattern in element.GetSupportedPatterns())
			{
				var i = pattern;
				/*
				if (pattern.ProgrammaticName is "ExpandCollapsePatternIdentifiers.Pattern")
				{
					comboPatt = pattern;
				}
				*/
			}
			/*
			ExpandCollapsePattern comboExpandPatt = element.GetCurrentPattern(comboPatt) as ExpandCollapsePattern;
			comboExpandPatt.Expand();
			comboExpandPatt.Collapse();

			AutomationElement listItem = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "ComboBox_1"));
			foreach (var pattern in listItem.GetSupportedPatterns())
			{
				if (pattern.ProgrammaticName is "SelectionItemPatternIdentifiers.Pattern")
				{
					comboPatt = pattern;
				}
			}

			SelectionItemPattern selectionItemPattern = listItem.GetCurrentPattern(comboPatt) as SelectionItemPattern;
			selectionItemPattern.Select();
			*/

			int a = 0;
		}
		/// <summary>
		/// 指定されたautomationIdに一致するAutomationElementを取得
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="automationId"></param>
		/// <returns></returns>
		private AutomationElement FindElementById(AutomationElement rootElement, string automationId)
		{
			return rootElement.FindFirst(TreeScope.Element | TreeScope.Descendants | TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
		}
		/// <summary>
		/// 指定された名前に一致するAutomationElementのコレクションをIEnumerableで返す
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private IEnumerable<AutomationElement> FindElementByName(AutomationElement rootElement, string name)
		{
			return rootElement.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name)).Cast<AutomationElement>();
		}

		private IEnumerable<AutomationElement> FindElementByLocalizeControlType(AutomationElement rootElement, string localizedControlType)
		{
			return rootElement.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, localizedControlType)).Cast<AutomationElement>();
		}

		private IEnumerable<AutomationElement> FindElementAll(AutomationElement rootElement, string a)
		{
			return rootElement.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.FrameworkIdProperty, a)).Cast<AutomationElement>();
		}
		private IEnumerable<AutomationElement> FindElementClassName(AutomationElement rootElement, string className)
		{
			return rootElement.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, className)).Cast<AutomationElement>();
		}

		private void WalkControlElements(AutomationElement rootElement, TreeNode treeNode)
		{
			AutomationElement elementNode = TreeWalker.ContentViewWalker.GetFirstChild(rootElement);
			
			while (elementNode is not null)
			{
				TreeNode childTreeNode = treeNode.Nodes.Add(elementNode.Current.ControlType.LocalizedControlType);
				WalkControlElements(elementNode, childTreeNode);
				elementNode = TreeWalker.ContentViewWalker.GetNextSibling(elementNode);
			}
		}
	}
}
