using MaterialDesignThemes.Wpf;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Modules.CodeManagerView.Domains;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;

using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeRegisterViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private ICodeRegisterModel _model;



		#region Properties

		public bool KeepAlive { get; private set; } = true;

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
			KeepAlive = true;

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
		/// 
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
		/// 
		/// </summary>
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{
			Table.Value = _model.SetTable(SelectedLangType.Value, SelectedDevType.Value);
			RecordCount.Value = Table.Value.Rows.Count;
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim UseAppMajorSelectionChanged { get; }
		private void OnUseAppMajorSelectionChanged()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommand RegistCodeNumber { get; }
		private void OnRegistCodeNumber()
		{
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

			_model.InsertCodeManager(developNumber, developName, uiFramework, createdOn, useApplication, explanation, summary);

			SnackbarIsActive.Value = true;
			ResultMessage.Value = _model.ResultMessage;

			Table.Value = _model.SetTable(SelectedLangType.Value, SelectedDevType.Value);
		}

		public ReactiveCommandSlim CloseSnackbar { get; }
		private void OnCloseSnackbar()
		{
			SnackbarIsActive.Value = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim DataGridRowSelectionChanged { get; }

		/// <summary>
		/// 現在指定しているプロジェクト番号のプロジェクトが存在する親フォルダをエクスプローラーで開く
		/// </summary>
		private void OnContextMenuOpenParentFolder()
		{
			_model.OpenProjectParentDirectry(SelectedLangType.Value);
		}
		/// <summary>
		/// 現在指定しているプロジェクト番号のプロジェクトフォルダをエクスプローラーで開く
		/// </summary>
		private void OnContextMenuOpenProjectFolder()
		{
			(string developNumber, string _) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectDirectry(developNumber, SelectedLangType.Value);
		}
		/// <summary>
		/// 現在指定しているプロジェクト番号のプロジェクトファイルを開く
		/// </summary>
		private void OnContextMenuOpenProject()
		{
			(string developNumber, string dirFileName) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectFile(developNumber, dirFileName, SelectedLangType.Value);
		}
		/// <summary>
		/// Visual Studio 2019 を起動
		/// </summary>
		private void OnContextMenuAwakeVS2019()
		{
			_model.AwakeVS2019();
		}
		/// <summary>
		/// Visual Studio 2022 を起動
		/// </summary>
		private void OnContextMenuAwakeVS2022()
		{
			_model.AwakeVS2022();
		}
		private void OnContextMenuCreateProject()
		{
			_model.A();

			System.Windows.MessageBox.Show("現在作成中");
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
