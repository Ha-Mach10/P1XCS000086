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
using unvell.ReoGrid.DataFormat;

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
		/// <summary>
		/// ReoGridの各種パラメータを調整
		/// </summary>
		private void ResetReoGrid()
		{
			ReoGridControl reoGrid = ReoGrid.Value;
			DataTable dt = Table.Value;

			var sheet = reoGrid.CurrentWorksheet;

			// ReoGridの行列サイズを再定義
			sheet.Resize( rows: dt.Rows.Count, cols: dt.Columns.Count);

			// カラムの番地を取得
			string sheetHeaderRight = ConvertAlphabet(dt.Columns.Count);
			// 列加算用
			int count = 1;

			dt.AsEnumerable().Select(x =>
			{
				string address = $"A{count}:{sheetHeaderRight}{count}";
				// 一列ずつxのDataRowの配列を格納
				sheet[address] = x.ItemArray;
				sheet.SetRangeDataFormat(address, CellDataFormatFlag.Text);

				count++;
				return x;

			}).ToList();

			// ReoGridのカラム名称を変更
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				// カラム名称変更
				sheet.ColumnHeaders[i].Text = dt.Columns[i].ColumnName;

				// 列幅調整
				sheet.AutoFitColumnWidth(i);
				sheet.ColumnHeaders[i].IsAutoWidth = true;
			}

			sheet.ZoomReset();
		}
		/// <summary>
		/// 数値からアルファベットへの変換（Excelシート等のカラムを指定するためのメソッド）
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private string ConvertAlphabet(int index)
		{
			// 出力用のキャラクタを格納するリスト
			List<char> chars = new List<char>();

			// 演算結果の格納用
			// divide：商、riminder：余り
			int divide = index, riminder;

			// 商が
			while (divide != 0)
			{
				// 26で割り、商と余りを出力
				(divide, riminder) = Math.DivRem(divide, 26);

				// 余りが0の場合（余りが0→26以下）
				if (riminder == 0)
				{
					(divide, riminder) = (divide - 1, 26);
				}

				// charに値を加えて"chars"に格納
				chars.Add((char)('A' + riminder - 1));
			}

			// 格納された逆の順序で配列に変換し、連結して文字列に変換
			return new string(chars.AsEnumerable().Reverse().ToArray());
		}
	}
}
