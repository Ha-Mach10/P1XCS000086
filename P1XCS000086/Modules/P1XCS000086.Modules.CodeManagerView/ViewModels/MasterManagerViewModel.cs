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
using unvell.ReoGrid.Data;
using System.Windows;
using P1XCS000086.Services.Interfaces.Data;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class MasterManagerViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private IMasterManagerModel _model;
		private IDTConveter _conveter;


		// Properties

		public bool KeepAlive { get; private set; }
		public ReactivePropertySlim<List<string>> DatabaseNames { get; }
		public ReactivePropertySlim<string> SelectedDatabaseName { get; }
		public ReactivePropertySlim<List<TableNameListItem>> TableNames { get; }
		public ReactivePropertySlim<TableNameListItem> SelectedTableName { get; }

		public ReactivePropertySlim<DataTable> Table { get; }
		public ReactivePropertySlim<ReoGridControl> ReoGrid { get; }

		public ReactivePropertySlim<Visibility> EditTableButtonVisibility { get; }
		public ReactivePropertySlim<Visibility> FixTableButtonVisibility { get; }



		public MasterManagerViewModel(IRegionManager regionManager, IMasterManagerModel model, IDTConveter conveter)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;
			_conveter = conveter;

			// このビューモデルの生存
			KeepAlive = true;

			// Properties
			DatabaseNames = new ReactivePropertySlim<List<string>>(GenerateDatabaseNames().ToList()).AddTo(_disposables);
			SelectedDatabaseName = new ReactivePropertySlim<string>(string.Empty);
			TableNames = new ReactivePropertySlim<List<TableNameListItem>>().AddTo(_disposables);
			SelectedTableName = new ReactivePropertySlim<TableNameListItem>().AddTo(_disposables);

			Table = new ReactivePropertySlim<DataTable>().AddTo(_disposables);
			ReoGrid = new ReactivePropertySlim<ReoGridControl>(new()).AddTo(_disposables);

			EditTableButtonVisibility = new ReactivePropertySlim<Visibility>(Visibility.Collapsed);
			FixTableButtonVisibility = new ReactivePropertySlim<Visibility>(Visibility.Collapsed);


			// Commands
			DatabaseNameSelectionChanged = new ReactiveCommandSlim();
			DatabaseNameSelectionChanged.Subscribe(OnDatabaseNameSelectionChanged).AddTo(_disposables);
			ListViewSelectionChanged = new ReactiveCommandSlim();
			ListViewSelectionChanged.Subscribe(OnListViewSelectionChanged).AddTo(_disposables);
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);
			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);

			EditTableButtonClick = new ReactiveCommandSlim();
			EditTableButtonClick.Subscribe(OnEditTableButtonClick).AddTo(_disposables);
			FixTableButtonClick = new ReactiveCommandSlim();
			FixTableButtonClick.Subscribe(OnFixTableButtonClick).AddTo(_disposables);
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

			EditTableButtonVisibility.Value = Visibility.Visible;

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
		public ReactiveCommandSlim EditTableButtonClick { get; }
		private void OnEditTableButtonClick()
		{
			// ReoGrid及びTableを取得
			ReoGridControl reoGrid = ReoGrid.Value;
			DataTable dt = Table.Value;

			// シートを定義
			var sheet = reoGrid.CurrentWorksheet;

			// ReoGridの行列サイズを再定義
			// (編集可能なように、行サイズを限界近くまで拡張)
			sheet.Resize(rows: 1040000, cols: dt.Columns.Count);

			// ボタンの可視性変更
			FixTableButtonVisibility.Value = Visibility.Visible;
			EditTableButtonVisibility.Value = Visibility.Collapsed;
		}
		public ReactiveCommandSlim FixTableButtonClick { get; }
		private void OnFixTableButtonClick()
		{
			// シートを追加
			var sheet = ReoGrid.Value.CurrentWorksheet;

			// カラム名を取得
			List<string> columnNames
				= new List<string>(sheet.ColumnHeaders.Select(x => x.Text));

			// 指定した範囲のセルを取得
			RangePosition rp = new RangePosition()
			{
				Cols = sheet.UsedRange.Cols,
				Rows = sheet.UsedRange.Rows
			};

			// DataTableを定義
			DataTable beforeTable = Table.Value;
			DataTable afterTable = _conveter.Convert(columnNames, sheet.GetRangeData(rp));

			// 
			_model.TableUpDate(beforeTable, afterTable);

			// サイズの再定義
			sheet.Resize(rp.Rows, rp.Cols);

			// ボタンの可視性変更
			FixTableButtonVisibility.Value = Visibility.Collapsed;
			EditTableButtonVisibility.Value = Visibility.Visible;
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
		/// <summary>
		/// ReoGridへのデータ格納及び各種パラメータを調整
		/// </summary>
		private void ResetReoGrid()
		{
			ReoGrid.Value = new ReoGridControl();

			ReoGridControl reoGrid = ReoGrid.Value;
			DataTable dt = Table.Value;

			// 現在のワークシートを取得
			var sheet = reoGrid.CurrentWorksheet;

			string cellRange = $"A1:{ConvertAlphabet(dt.Columns.Count)}{dt.Rows.Count}";
			// A1番地を基点にデータを格納
			sheet.SetRangeData(cellRange, dt);
			
			// ReoGridの行列サイズを再定義
			sheet.Resize(rows: dt.Rows.Count, cols: dt.Columns.Count);

			// ReoGridのカラム名称を変更
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				// カラム名称変更
				sheet.ColumnHeaders[i].Text = dt.Columns[i].ColumnName;

				// 列幅調整
				sheet.AutoFitColumnWidth(i);
			}
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
