using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using P1XCS000086.Services.Interfaces.Data;
using P1XCS000086.Services.Sql;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Data
{
	public class DTConverter : IDTConveter
	{
		// 
		private string _databaseName;
		private string _tableName;



		// Properties
		public List<string> ExceptionMessages { get; private set; } = new();
		public List<string> ResultMessages { get; private set; } = new();



		public DTConverter()
		{

		}



		// Public Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnNames"></param>
		/// <param name="gridObjects"></param>
		/// <returns></returns>
		public DataTable Convert(List<string> columnNames, object[,] gridObjects)
		{
			DataTable dt = new();

			// カラム名のリストからdtのカラム名を設定
			columnNames.Select(x => dt.Columns.Add(x)).ToArray();

			// 行数を取得
			int secondDimension = gridObjects.Cast<object>().ToList().Where(x => x is not null).Count() / columnNames.Count;
			for (int i = 0; i < secondDimension; i++)
			{
				// 新しいDataRowを定義
				DataRow dr = dt.NewRow();

				int count = 0;
				foreach (var columnName in columnNames)
				{
					if (gridObjects[i, count] is null)
					{
						dr[columnName] = gridObjects[i, count];
					}
					else
					{
						dr[columnName] = gridObjects[i, count].ToString();
					}
					count++;
				}

				dt.Rows.Add(dr);
			}

			return dt;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="beforeTable"></param>
		/// <param name="afterTable"></param>
		/// <param name="databaseName"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public void DataTablesCompatation(DataTable beforeTable, DataTable afterTable, string databaseName, string tableName)
		{
			_databaseName = databaseName;
			_tableName = tableName;


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
				return;
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

			// 生成されたクエリ文のリストを取得
			var queryAndValuePair = GenQueryValues(columnNames, listPair, tableName);

			// 取得したクエリを実行
			ExecuteQuery(queryAndValuePair);
		}



		// Private Methods

		/// <summary>
		/// 与えられたbeforeItemsとafterItemsの比較を行い、クエリ種別とSQL文を返す
		/// </summary>
		/// <param name="columnNames">カラム名称一覧</param>
		/// <param name="tuples">beforeItemsとafterItemsのタプル</param>
		/// <returns>クエリ種別とSQL文</returns>
		private List<string> GenQueryValues (List<string> columnNames, List<(List<string> beforeItems, List<string> afterItems)> tuples, string tableName)
		{
			List<string> querys = new();
			// List<(QueryType, string)> queryAndValuePair = new();
			var valuePairs = tuples.Select((x, y) => x.beforeItems.Zip(x.afterItems, (before, after) => new { BeforeValue = before, AfterValue = after }).ToList()).ToList();

			foreach (var pairs in valuePairs)
			{
				// カラム名称とBeforeValueとAfterValueのペアを取得
				var items = pairs.Zip(columnNames, (valuePair, columnName) => new { ValuePair = valuePair, ColumnName = columnName })
								 .ToList();


				// BeforeValueが全てnullの場合（INSERTクエリ）
				if (items.Where(x => string.IsNullOrEmpty(x.ValuePair.BeforeValue)).Count() == columnNames.Count)
				{
					// INSERT用の配列を取得
					string[] insertValues = items.Select(x => $"'{x.ValuePair.AfterValue}'").ToArray();
					// 上記配列からINSERT用の文字列を取得
					string insertValue = string.Join(',', insertValues);

					// 
					querys.Add($"INSERT INTO `{tableName}` VALUES ({insertValue});");
				}
				// AfterValueがすべてnullの場合（DELETEクエリ）
				else if (items.Where(x => string.IsNullOrEmpty(x.ValuePair.AfterValue)).Count() == columnNames.Count)
				{
					// WHERE用の値を配列で取得
					string[] whereValues = items.Select(x => $"`{x.ColumnName}` = '{x.ValuePair.BeforeValue}'").ToArray();
					// 上記配列からWHERE用の文字列を取得
					string whereValue = string.Join("AND", whereValues);

					// 
					querys.Add($"DELETE FROM `{tableName}` WHERE {whereValue};");
				}
				// AfterValueとBeforeValueの値が複数合致している場合（UPDATEクエリ）
				else if (items.Where(x => x.ValuePair.BeforeValue == x.ValuePair.AfterValue).Count() < columnNames.Count)
				{
					// SET用の値をリストで取得
					var setValues = items.Where(x => x.ValuePair.BeforeValue != x.ValuePair.AfterValue)
										 .Select(x => $"`{x.ColumnName}` = '{x.ValuePair.AfterValue}'")
										 .ToList();
					// WHERE用の値をリストで取得
					var whereValues = items.Where(x => x.ValuePair.BeforeValue == x.ValuePair.AfterValue)
										   .Select(x => $"`{x.ColumnName}` = '{x.ValuePair.AfterValue}'")
										   .ToList();

					// SET用の文字列を取得
					string setValue = string.Join(',', setValues);
					// WHERE用の文字列を取得
					string whereValue = string.Join("AND", whereValues);

					// 
					querys.Add($"UPDATE `{tableName}` SET {setValue} WHERE {whereValue};");
				}
			}

			return querys;
		}

		private void ExecuteQuery(List<string> querys)
		{
			var connStrings = SqlConnectionStrings.ConnectionStrings;
			string connStr = connStrings[_databaseName];

			SqlExecute sqlExecute = new(connStr);

			foreach (string query in querys)
			{
				sqlExecute.Execute(query);

				ExceptionMessages.Add(sqlExecute.ExceptionMessage);
				ResultMessages.Add(sqlExecute.ResultMessage);
			}
		}
	}
}
