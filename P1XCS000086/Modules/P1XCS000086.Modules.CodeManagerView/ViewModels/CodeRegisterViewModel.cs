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

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class CodeRegisterViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private ICodeRegisterModel _model;


		// Properties

		public bool KeepAlive { get; private set; }

		public ReactivePropertySlim<bool> IsPaneOc { get; }

		public ReactivePropertySlim<int> RecordCount { get; }

		public ReactivePropertySlim<List<string>> LangTypes { get; }
		public ReactivePropertySlim<List<string>> DevTypes { get; }
		public ReactivePropertySlim<string> SelectedLangType { get; }
		public ReactivePropertySlim<string> SelectedDevType { get; }
		public ReactivePropertySlim<int> SelectedIndexDevType { get; }

		[Required(ErrorMessage = "必須項目\n開発名称を入力してください")]
		[RegularExpression("^[0-9a-Z_]+$", ErrorMessage = "規定の文字のみを含んだ文字列を入力してください")]
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



		public CodeRegisterViewModel(IRegionManager regionManager, ICodeRegisterModel model)
			: base(regionManager)
		{
			// インジェクション
			_regionManager = regionManager;
			_model = model;



			// Properties
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


			// 

			// Commands
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);
			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);
			UseAppMajorSelectionChanged = new ReactiveCommandSlim();
			UseAppMajorSelectionChanged.Subscribe(OnUseAppMajorSelectionChanged).AddTo(_disposables);
			RegistCodeNumber =
				new[]
				{
					DevelopName.ObserveHasErrors,
					UseAppMajor.ObserveHasErrors
				}
				.CombineLatestValuesAreAllFalse()
				.ToReactiveCommand();
			RegistCodeNumber.Subscribe(OnRegistCodeNumber).AddTo(_disposables);
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
			string developNumber = $"{_model.CodeDevType}{RecordCount.Value.ToString("000000")}";
			string developName = DevelopName.Value;
			string uiFramework = SelectedUiFramework.Value;
			string createdOn = DateTime.Now.ToString();
			string useApplication = SelectedUseAppMajor.Value;
			if (string.IsNullOrEmpty(SelectedUseAppRange.Value) is false)
			{
				useApplication = $"{useApplication}_{SelectedUseAppRange.Value}";
			}
			string explanation = Explanation.Value;
			string summary = Summary.Value;

			int a = 0;
		}
	}
}
