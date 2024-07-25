using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using P1XCS000086.Services.Interfaces.Data;
using Reactive.Bindings.Notifiers;

namespace P1XCS000086.Services.Data
{
	public class DTConverter : IDTConveter
	{
		// Enum
		public enum QueryType
		{
			None = 0,
			Update = 1,
			Insert = 2,
			Delete = 3,
		}


		// 
		private DataTable _dt1;
		private DataTable _dt2;



		public List<(List<string> BeforeDtRowItems, List<string> AfterDtRowItems)> Lists { get; private set; }



		public DTConverter(DataTable dt1, DataTable dt2)
		{
			DataTablesCompatation(dt1, dt2);
		}



		// Public Methods
		public DataTable Convert(List<string> columnNames, object[,] gridObjects)
		{
			DataTable dt = new();

			columnNames.Select(x => dt.Columns.Add(x)).ToArray();

			int secondDimension = gridObjects.Cast<object>().ToList().Where(x => x is not null).Count() / columnNames.Count;
			for (int i = 0; i < secondDimension; i++)
			{
				DataRow dr = dt.NewRow();

				int count = 0;
				foreach (var columnName in columnNames)
				{
					dr[columnName] = gridObjects[i, count].ToString();
					count++;
				}

				dt.Rows.Add(dr);
			}

			return dt;
		}
		public List<(QueryType queryType, string query, List<string>)> QueryValues()
		{
			return null;
		}



		// Private Methods
		private bool DataTablesCompatation(DataTable beforeTable, DataTable afterTable)
		{
			// ---------------------------------------------------------------------
			// 2テーブルの入力が適切か判定（カラム比較）
			// ---------------------------------------------------------------------

			// 「beforeTable」「afterTable」両方のカラム文字列を比較し、合致している個数を返す
			var colDnu = beforeTable.Columns
								   .Cast<DataColumn>()
								   .Select(x => x.ColumnName)
								   .Zip(
										afterTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName),
										(f, s) => new { Result = f == s }
								   )
								   .Where(x => x.Result == true)
								   .Count();

			// colDnuの値と各テーブルのカラム数が合わない場合falseを返す
			if (colDnu != beforeTable.Columns.Count || colDnu != afterTable.Columns.Count)
			{
				return false;
			}

			List<string> columnNames = beforeTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();


			// ---------------------------------------------------------------------
			// 行数の比較によるテーブルの整形（同数の列に変更する）
			// ---------------------------------------------------------------------

			int fRowsCount = beforeTable.Rows.Count;
			int sRowsCount = afterTable.Rows.Count;
			// int size = fRowsCount > sRowsCount ? fRowsCount : sRowsCount;

			// データテーブルを参照する（参照のため、参照元のテーブルにも反映される）
			// （今回は複製を行わない："Clone()" or "Copy()"）
			DataTable alterTable = fRowsCount < sRowsCount ? beforeTable : afterTable;

			// 各テーブルの行数の差分の絶対値を試行回数とする
			for (int i = 0; i < Math.Abs(fRowsCount - sRowsCount); i++)
			{
				// 新しい行を定義
				DataRow dr = alterTable.NewRow();
				foreach (var columnName in columnNames)
				{
					// 空文字列を格納
					dr[columnName] = DBNull.Value;
				}

				// テーブルに追加
				alterTable.Rows.Add(dr);
			}


			List<(List<string>, List<string>)> listPair = new();

			// 2テーブルのDataRowのタプルリストを返す
			var dnu = beforeTable.AsEnumerable()
								.Zip(
									afterTable.AsEnumerable(),
									(first, second) => new { First = first, Second = second }
								)
								.ToList();

			// 
			foreach (var item in dnu)
			{
				// 行の配列を要素ごとに比較し、
				var dnu2 = item.First
							   .ItemArray
							   .Zip(
									item.Second.ItemArray,
									(f, s) => new { Same = f.ToString() == s.ToString(), F = f.ToString(), S = s.ToString() }
								)
							   .ToList();

				int count = dnu2.Where(x => x.Same is false).Count();
				if (count != 0)
				{
					listPair.Add((dnu2.Select(x => x.F).ToList(), dnu2.Select(x => x.S).ToList()));
				}
			}

			GenQueryValues(columnNames, listPair);

			int a = 0;
			// if (beforeTable)

			return false;
		}

		private List<(QueryType, string)> GenQueryValues
			(List<string> columnNames, List<(List<string> beforeItems, List<string> afterItems)> tuples)
		{
			List<(QueryType, List<string>, List<string>)> querySameValuesTaple = new();
			var valuePairs = tuples.Select((x, y) => x.beforeItems.Zip(x.afterItems, (before, after) => new { BeforeValue = before, AfterValue = after }).ToList()).ToList();

			foreach (var pairs in valuePairs)
			{
				// カラム名称とBeforeValueとAfterValueのペアを取得
				var items = pairs.Zip(columnNames, (valuePair, columnName) => new { ValuePair = valuePair, ColumnName = columnName })
								 .ToList();

				QueryType queryType = QueryType.None;

				// BeforeValueが全てnullの場合（INSERTクエリ）
				if (items.Where(x => string.IsNullOrEmpty(x.ValuePair.BeforeValue)).Count() == columnNames.Count)
				{
					queryType = QueryType.Insert;
					string itemValue = $"INSERT INTO `*table*` VALUES ({string.Join(',', items.Select(x => $"'{x.ValuePair.AfterValue}'").ToArray())});";

					int c = 0;
				}
				// AfterValueとBeforeValueの値が複数合致している場合（UPDATEクエリ）
				else if (items.Where(x => x.ValuePair.BeforeValue == x.ValuePair.AfterValue).Count() < columnNames.Count)
				{
					var setValues = items.Where(x => x.ValuePair.BeforeValue != x.ValuePair.AfterValue)
										 .Select(x => $"`{x.ColumnName}` = '{x.ValuePair.AfterValue}'")
										 .ToList();
					var whereValues = items.Where(x => x.ValuePair.BeforeValue == x.ValuePair.AfterValue)
										   .Select(x => $"`{x.ColumnName}` = '{x.ValuePair.AfterValue}'")
										   .ToList();


					string setValue = $"SET {string.Join(',', setValues)}";
					string whereValue = $"";

					queryType = QueryType.Update;
					string itemValue = $"UPDATE `*table*` SET { string.Join(',', setValues) } WHERE { string.Join("AND", whereValues) };";

					int d = 0;
				}
				// AfterValueがすべてnullの場合（DELETEクエリ）
				else if (items.Where(x => string.IsNullOrEmpty(x.ValuePair.AfterValue)).Count() == columnNames.Count)
				{
					queryType = QueryType.Delete;
					string itemValue = $"DELETE FROM `*table*` WHERE {string.Join(',', string.Join("AND", items.Select(x => $"`{x.ColumnName}` = '{x.ValuePair.AfterValue}'")))}";
				}


				int b = 0;
			}


			int a = 0;

			return null;
		}
	}
}
