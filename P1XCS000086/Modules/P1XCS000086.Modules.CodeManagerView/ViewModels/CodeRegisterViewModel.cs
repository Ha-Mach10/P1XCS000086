using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using P1XCS000086.Core.Mvvm;
using System.ComponentModel;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using System.Net.Http;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeRegisterViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private ICodeRegisterModel _model;


		#region Properties

		public bool KeepAlive { get; private set; }

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
		
		#endregion



		public CodeRegisterViewModel(IRegionManager regionManager, ICodeRegisterModel model)
			: base(regionManager)
		{
			// インジェクション
			_regionManager = regionManager;
			_model = model;



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
			DataGridRowSelectionChanged.Subscribe(param =>
			{
				// コマンドパラメータから受け取った値をキャスト
				DataRowView dataRowView = param as DataRowView;				
				
				if (dataRowView is not null)
				{
					Row = dataRowView.Row;
					(string developNumber, string _) = GetSelectedDevelopNumber(Row);
					OpenDirHeaderContent.Value = $"{developNumber}フォルダをエクスプローラーで開く";
					OpenFileHeaderContent.Value = $"{developNumber}プロジェクトを開く";
				}				
			});
			// DataGridRowSelectionChanged.Subscribe(OnDataGridRowSelectionChanged).AddTo(_disposables);
			// 
			ContextMenuOpenParentFolder = new ReactiveCommandSlim();
			ContextMenuOpenParentFolder.Subscribe(OnContextMenuOpenParentFolder).AddTo(_disposables);
			ContextMenuOpenProjectFolder = new ReactiveCommandSlim();
			ContextMenuOpenProjectFolder.Subscribe(OnContextMenuOpenProjectFolder).AddTo(_disposables);
			ContextMenuOpenProject = new ReactiveCommandSlim();
			ContextMenuOpenProject.Subscribe(OnContextMenuOpenProject).AddTo(_disposables);
		}



		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{
			// 開発種別を選択するコンボボックス用Listに格納
			DevTypes.Value = _model.SetDevType(SelectedLangType.Value);
			SelectedIndexDevType.Value = -1;

			UiFramework.Value = _model.SetFrameworkName(SelectedLangType.Value);
			SelectedIndexUiFramework.Value = -1;
		}
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{
			Table.Value = _model.SetTable(SelectedLangType.Value, SelectedDevType.Value);
			RecordCount.Value = Table.Value.Rows.Count;
		}
		public ReactiveCommandSlim UseAppMajorSelectionChanged { get; }
		private void OnUseAppMajorSelectionChanged()
		{

		}
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
		public ReactiveCommandSlim DataGridRowSelectionChanged { get; }
		private void OnDataGridRowSelectionChanged()
		{
			/*
			if (Row.Value is not null)
			{
				(string developNumber, string _) = GetSelectedDevelopNumber(Row.Value);
				OpenDirHeaderContent.Value = $"{developNumber}フォルダをエクスプローラーで開く";
				OpenFileHeaderContent.Value = $"{developNumber}プロジェクトを開く";
			}
			*/

		}
		public ReactiveCommandSlim ContextMenuOpenParentFolder { get; }
		private void OnContextMenuOpenParentFolder()
		{
			_model.OpenProjectParentDirectry(SelectedLangType.Value);
		}
		public ReactiveCommandSlim ContextMenuOpenProjectFolder { get; }
		private void OnContextMenuOpenProjectFolder()
		{
			(string developNumber, string _) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectDirectry(developNumber, SelectedLangType.Value);
		}
		public ReactiveCommandSlim ContextMenuOpenProject { get; }
		private void OnContextMenuOpenProject()
		{
			(string developNumber, string dirFileName) = GetSelectedDevelopNumber(Row);
			_model.OpenProjectFile(developNumber, dirFileName, SelectedLangType.Value);
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
	}
}
