using System;
using System.Reactive.Disposables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Regions;
using Prism.Commands;
using Prism.Mvvm;

using Reactive.Bindings.Extensions;
using Prism.Navigation;
using Reactive.Bindings;
using System.Data;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using System.Windows.Xps.Serialization;


namespace P1XCS000086.Modules.CodeManageMaster.ViewModels
{
	public class MasterEditorViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposable;

		private IMasterEditorModel _model;
		private IIntegrMasterModel _integrModel;
		private IJsonConnectionStrings _jsonConnStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;
		private ISqlUpdate _update;
		private ISqlDelete _delete;
		private ISqlShowTables _showTables;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<bool> IsEditChecked { get; }
		public ReactivePropertySlim<bool> IsAddChecked { get; }
		public ReactivePropertySlim<bool> IsDeleteChecked { get; }
		public ReactivePropertySlim<List<string>> TableItems { get; }
		/// <summary>
		/// 選択されたテーブル名
		/// </summary>
		public ReactivePropertySlim<string> SelectedTableName { get; }
		/// <summary>
		///テーブル編集用の入力フィールド
		/// </summary>
		public ReactivePropertySlim<ITableField> Fields { get; }


		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public MasterEditorViewModel
			(IMasterEditorModel model, IIntegrMasterModel integrModel, IJsonConnectionStrings jsonConnStr, ISqlSelect select, ISqlInsert insert, ISqlUpdate update, ISqlDelete dlete, ISqlShowTables showTables)
		{
			_model = model;
			_integrModel = integrModel;
			_jsonConnStr = jsonConnStr;
			_select = select;
			_insert = insert;
			_update = update;
			_delete = dlete;
			_showTables = showTables;

			// DIされたモデル群をこのViewModelのModelへ注入
			_model.InjectModels(_integrModel, _jsonConnStr, _select, _insert, _update, _delete, _showTables);

			// Reactive Properties
			IsEditChecked		= new ReactivePropertySlim<bool>(true).AddTo(_disposable);
			IsAddChecked		= new ReactivePropertySlim<bool>(false).AddTo(_disposable);
			IsDeleteChecked		= new ReactivePropertySlim<bool>(false).AddTo(_disposable);
			TableItems			= new ReactivePropertySlim<List<string>>(_model.TableNames).AddTo(_disposable);
			SelectedTableName	= new ReactivePropertySlim<string>(string.Empty).AddTo(_disposable);
			Fields				= new ReactivePropertySlim<ITableField>().AddTo(_disposable);


			// Commands
			RadioCheckedChanged = new ReactiveCommandSlim();
			RadioCheckedChanged.Subscribe(() => OnRadioCheckedChanged()).AddTo(_disposable);

			ListViewSelectionChanged = new ReactiveCommandSlim();
			ListViewSelectionChanged.Subscribe(() => OnListViewSelectionChanged()).AddTo(_disposable);

			ApplyButtonClick = new ReactiveCommandSlim();
			ApplyButtonClick.Subscribe(() => OnApplyButtonClick()).AddTo(_disposable);
		}



		// ****************************************************************************
		// Reactive Command
		// ****************************************************************************

		public ReactiveCommandSlim RadioCheckedChanged { get; }
		private void OnRadioCheckedChanged()
		{
			if (IsEditChecked.Value)
			{

			}
			else if (IsAddChecked.Value)
			{

			}
			else if (IsDeleteChecked.Value)
			{

			}
		}

		public ReactiveCommandSlim ListViewSelectionChanged { get; }
		private void OnListViewSelectionChanged()
		{
			_integrModel.SetSelectedTableName(SelectedTableName.Value);
		}

		public ReactiveCommandSlim ApplyButtonClick { get; }
		private void OnApplyButtonClick()
		{

		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		// 破棄
		public void Destroy()
		{

		}
	}
}
