using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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

			int secondDimension = gridObjects.Length / columnNames.Count;
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



			List<(string, string)> lists = new();
			List<List<string>> dList = new();
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
				

				/*
				List<string> tmpList = new();
				foreach (var item2 in dnu2)
				{
					if (item2.Same is false)
					{
						tmpList.Add(item2.S);
					}
				}
				*/
				if (count >= 1)
				{
					listPair.Add((dnu2.Select(x => x.F).ToList(), dnu2.Select(x => x.S).ToList()));
				}
			}

			int a = 0;
			// if (beforeTable)

			return false;
		}
	}
}
