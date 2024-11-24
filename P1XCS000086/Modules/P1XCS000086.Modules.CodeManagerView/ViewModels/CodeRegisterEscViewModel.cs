using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;

using Prism.Navigation.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeRegisterEscViewModel : RegionViewModelBase, INotifyPropertyChanged/*, IRegionMemberLifetime*/
	{
		/*
		// Fields
		private IRegionManager _regionManager;
		private ICodeRegisterModel _model;
		private ISqlSelect _select;



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

		public ReactivePropertySlim<string> OpenDirHeaderContent { get; }
		public ReactivePropertySlim<string> OpenFileHeaderContent { get; }

		public ReactivePropertySlim<Visibility> ContentVisibility { get; }
		public ReactivePropertySlim<Visibility> ContentInitialVisibility { get; }

		// public ReactiveCollection<SelectedRowPropertyField> SelectedRowPropertyFieldItems { get; }

		#endregion
		*/


		public CodeRegisterEscViewModel(IRegionManager regionManager, ICodeRegisterModel model, ISqlSelect select)
			: base(regionManager)
		{
			/*
			// インジェクション
			_regionManager = regionManager;
			_model = model;
			_select = select;


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

			UiFramework = new ReactivePropertySlim<List<string>>(null).AddTo(_disposables);
			SelectedUiFramework = new ReactivePropertySlim<string>(string.Empty);
			SelectedIndexUiFramework = new ReactivePropertySlim<int>(0);

			Explanation = new ReactivePropertySlim<string>(string.Empty);
			Summary = new ReactivePropertySlim<string>(string.Empty);

			Table = new ReactivePropertySlim<DataTable>(null).AddTo(_disposables);

			OpenDirHeaderContent = new ReactivePropertySlim<string>(string.Empty);
			OpenFileHeaderContent = new ReactivePropertySlim<string>(string.Empty);
			ContentVisibility = new ReactivePropertySlim<Visibility>(Visibility.Collapsed);
			ContentInitialVisibility = new ReactivePropertySlim<Visibility>(Visibility.Visible);

			// SelectedRowPropertyFieldItems = new ReactiveCollection<SelectedRowPropertyField>().AddTo(_disposables);

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
			DataGridRowSelectionChanged = new ReactiveCommandSlim<DataRowView>();
			DataGridRowSelectionChanged.Subscribe(_ => OnDataGridRowSelectionChanged(_)).AddTo(_disposables);

			ContextMenuOpenParentFolder = new();
			ContextMenuOpenParentFolder.Subscribe(OnContextMenuOpenParentFolder).AddTo(_disposables);

			ContextMenuOpenProjectFolder = new();
			ContextMenuOpenProjectFolder.Subscribe(OnContextMenuOpenProjectFolder).AddTo(_disposables);

			ContextMenuOpenProject = new();
			ContextMenuOpenProject.Subscribe(OnContextMenuOpenProject).AddTo(_disposables);

			ContextMenuAwakeVS = new();
			ContextMenuAwakeVS.Subscribe(OnContextMenuAwakeVS).AddTo(_disposables);
			*/
		}



		/*
		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{
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

			Table.Value = _model.SetTable(SelectedLangType.Value, SelectedDevType.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim<DataRowView> DataGridRowSelectionChanged { get; }
		private void OnDataGridRowSelectionChanged(DataRowView param)
		{
			// コマンドパラメータから受け取った値をキャスト
			DataRowView dataRowView = param as DataRowView;


			if (dataRowView is not null)
			{
				var items = _model.GetSelectedRowPropertyFieldItem(dataRowView)
							  .Select(item => new SelectedRowPropertyField(_regionManager, item.columnNames, item.propertyText))
							  .ToList();
				SelectedRowPropertyField.AddRangeItem(items);
				SelectedRowPropertyFieldItems.AddRangeOnScheduler(items);


				Row = dataRowView.Row;
				(string developNumber, string _) = GetSelectedDevelopNumber(Row);
				if (string.IsNullOrEmpty(developNumber) is true)
				{
					ContentVisibility.Value = Visibility.Collapsed;
					ContentInitialVisibility.Value = Visibility.Visible;
				}
				else
				{
					OpenDirHeaderContent.Value = $"{developNumber}フォルダをエクスプローラーで開く";
					OpenFileHeaderContent.Value = $"{developNumber}プロジェクトを開く";

					ContentVisibility.Value = Visibility.Visible;
					ContentInitialVisibility.Value = Visibility.Collapsed;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim ContextMenuOpenParentFolder { get; }
		private void OnContextMenuOpenParentFolder()
		{
			_model.OpenProjectParentDirectry(SelectedLangType.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim ContextMenuOpenProjectFolder { get; }
		private void OnContextMenuOpenProjectFolder()
		{
			(string developNumber, string _) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectDirectry(developNumber, SelectedLangType.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommandSlim ContextMenuOpenProject { get; }
		private void OnContextMenuOpenProject()
		{
			(string developNumber, string dirFileName) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectFile(developNumber, dirFileName, SelectedLangType.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommand ContextMenuAwakeVS { get; }
		private void OnContextMenuAwakeVS()
		{
			_model.AwakeVS();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="selectedRow"></param>
		/// <returns></returns>
		private (string, string) GetSelectedDevelopNumber(DataRow selectedRow)
		{
			string devNumber = selectedRow["develop_number"].ToString();
			string dirFileName = selectedRow["dir_file_name"].ToString();

			return (devNumber, dirFileName);
		}
		*/
	}
}
