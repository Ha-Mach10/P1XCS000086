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
using System.Data;
using P1XCS000086.Modules.CodeManagerView.Domains;
using unvell.ReoGrid;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class MasterManagerViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private IMasterManagerModel _model;


		// Properties

		public bool KeepAlive { get; private set; }
		public ReactivePropertySlim<List<string>> DatabaseNames { get; }
		public ReactivePropertySlim<string> SelectedDatabaseName { get; }
		public ReactivePropertySlim<List<TableNameListItem>> TableNames { get; }
		public ReactivePropertySlim<TableNameListItem> SelectedTableName { get; }

		public ReactivePropertySlim<DataTable> Table { get; }
		public ReactivePropertySlim<ReoGridControl> ReoGrid { get; }



		public MasterManagerViewModel(IRegionManager regionManager, IMasterManagerModel model)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;

			// このビューモデルの生存
			KeepAlive = true;

			// Properties
			DatabaseNames = new ReactivePropertySlim<List<string>>(GenerateDatabaseNames().ToList()).AddTo(_disposables);
			SelectedDatabaseName = new ReactivePropertySlim<string>(string.Empty);
			TableNames = new ReactivePropertySlim<List<TableNameListItem>>().AddTo(_disposables);
			SelectedTableName = new ReactivePropertySlim<TableNameListItem>().AddTo(_disposables);

			Table = new ReactivePropertySlim<DataTable>().AddTo(_disposables);
			ReoGrid = new ReactivePropertySlim<ReoGridControl>(ReoGridInitializedObject()).AddTo(_disposables);


			// Commands
			DatabaseNameSelectionChanged = new ReactiveCommandSlim();
			DatabaseNameSelectionChanged.Subscribe(OnDatabaseNameSelectionChanged).AddTo(_disposables);
			ListViewSelectionChanged = new ReactiveCommandSlim();
			ListViewSelectionChanged.Subscribe(OnListViewSelectionChanged).AddTo(_disposables);
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);
			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);
		}



		// Commands

		public ReactiveCommandSlim DatabaseNameSelectionChanged { get; }
		private void OnDatabaseNameSelectionChanged()
		{
			TableNames.Value = GenerateTableNameListItems().ToList();
		}
		public ReactiveCommandSlim ListViewSelectionChanged { get; }
		private void OnListViewSelectionChanged()
		{
			if (SelectedTableName.Value is null)
			{
				Table.Value = null;
				return;
			}
			Table.Value = _model.SearchTable(SelectedDatabaseName.Value, SelectedTableName.Value.TableName);

			// ReoGridの各種プロパティを再定義
			ResetReoGrid();
		}
		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{

		}
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{

		}



		// Private Methods
		
		private IEnumerable<string> GenerateDatabaseNames()
		{
			foreach(var item in _model.DatabaseNames)
			{
				yield return item;
			}
		}
		private IEnumerable<TableNameListItem> GenerateTableNameListItems()
		{
			foreach (var item in _model.GetTableNameSets(SelectedDatabaseName.Value))
			{
				yield return new TableNameListItem(item.Item1, item.Item2);
			}
		}
		private ReoGridControl ReoGridInitializedObject()
		{
			ReoGridControl reoGrid = new();

			var sheet = reoGrid.CurrentWorksheet;
			sheet.Resize(rows: 60, cols: 4);

			return reoGrid;
		}
		private void ResetReoGrid()
		{
			ReoGridControl reoGrid = ReoGrid.Value;
			DataTable dt = Table.Value;

			var sheet = reoGrid.CurrentWorksheet;

			// ReoGridの行列サイズを再定義
			sheet.Resize( rows: dt.Rows.Count, cols: dt.Columns.Count);

			// ReoGridのカラム名称を変更
			for(int i = 0; i <= dt.Columns.Count; i++)
			{
				sheet.RowHeaders[i].Text = dt.Columns[i].ColumnName;
			}

			// reoGrid.CurrentWorksheet = 
		}
		private string ConvertAlphabet(int index)
		{
			string result = string.Empty;

			List<int> indexes = new List<int>();

			int riminder = 0;
			int quotient = 0;
			while (index > 0)
			{
				
			}

			// indexが26より大きい場合
			do
			{
				// 余り
				int reminder = index % 26;
				// 商
				int quotient = index / 26;



				// 余りをアルファベットに変換
				result = ('A' + reminder - 1).ToString() + result;


			}
			while (index > 26)
		}
	}
}
