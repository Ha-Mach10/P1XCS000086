using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace P1XCS000086.Services.Sql.MySql
{
	public class MySqlQueryParser
	{
		private enum QueryType
		{
			SELECT,
			INSERT,
			UPDATE,
			DELETE,
			NONE,
		}



		public string QueryTypeName { get; private set; } = string.Empty;
		public List<string> ColumnNames { get; private set; } = new();
		public string TableName { get; private set; } = string.Empty;
		public Dictionary<string, string> ColumnValues { get; private set; } = new();
		public bool IsParse { get; private set; } = false;



		public MySqlQueryParser(string query)
		{
			// 空白文字列またはnullまたはstring.Emptyか判別。
			if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query)) return;

			// 
			QueryType queryType = QueryTypeCheck(query);
			// 
			GetColumnNames(query, queryType);
			// 
			switch (queryType)
			{
				case QueryType.SELECT or QueryType.UPDATE or QueryType.DELETE:
					GetWhereColumnValue(query);
					break;

				case QueryType.INSERT:

					break;
			}
		}



		/// <summary>
		/// クエリのタイプを判別する
		/// </summary>
		/// <param name="query">クエリ文</param>
		/// <returns>いずれかのクエリタイプを返す。どれにも合致しない場合、値"NONE"を返す</returns>
		private QueryType QueryTypeCheck(string query)
		{
			// 全て大文字へ変換
			query = query.ToUpper();

			QueryType queryType = QueryType.SELECT;

			foreach (QueryType type in Enum.GetValues(typeof(QueryType)).OfType<QueryType>())
			{
				string queryTypeStr = type.ToString();
				Regex regex = new Regex(queryTypeStr);
				if (regex.IsMatch(query) && query.IndexOf(queryTypeStr) == 0)
				{
					QueryTypeName = type.ToString();
					return type;
				}
			}

			QueryTypeName = QueryType.NONE.ToString();
			return QueryType.NONE;
		}

		/// <summary>
		/// クエリとそのクエリのタイプから、カラム名を取得する
		/// </summary>
		/// <param name="query"></param>
		/// <param name="queryType"></param>
		/// <returns></returns>
		private void GetColumnNames(string query, QueryType queryType)
		{
			query = query.ToUpper();

			// 引数のクエリタイプから取得カラムの位置のインデックスを取得
			int startIndex = query.IndexOf(queryType.ToString());
			int endIndex = 0;

			// クエリ毎の次回予約語（FROM, INTO,等）の頭までのインデックスを取得
			switch (queryType)
			{
				case QueryType.SELECT:
					endIndex = query.IndexOf("FROM");
					break;

				case QueryType.INSERT:	// 不要かも
					endIndex = query.IndexOf("INTO");
					break;

				case QueryType.UPDATE:
					endIndex = query.IndexOf("SET");
					break;

				case QueryType.DELETE:
					endIndex = query.IndexOf("FROM");
					break;
			}

			// 文字列切り取り時の長さを取得
			int length = endIndex - startIndex;

			// 文字列の開始位置と長さでカラム名を切り取る
			string columnsStr = query.Substring(startIndex, length).Replace(" ", "");

			// カラム名指示に「`（バッククォート）」が使用されているか判別
			// 使用されていた場合は除去
			if (columnsStr.Contains('`'))
			{
				columnsStr = columnsStr.Replace("`", "");
			}

			// カラム名を区切るカンマを基点に文字列を分割し、カラム名リストのプロパティへ格納
			ColumnNames = columnsStr.Split(',').ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		private void GetWhereColumnValue(string query)
		{
			// 
			const string where = "WHERE";
			const string and = "AND";

			// WHEREが含まれていない場合は処理を抜ける
			if (query.Contains(where) is false) return;

			// 「WHERE」以降の文字列のインデックスを取得
			int whereValuePosition = query.IndexOf("WHERE") + where.Length;
			// 「WHERE」以降の文字列を取得
			string whereKeywordAfter = query.Substring(whereValuePosition);

			// 正規表現
			Regex regexColumn = new Regex("`([A-z]+)`");
			Regex regexValue = new Regex("`([A-z0-9_]+)` *= *(.+)");

			var columnAndValues = whereKeywordAfter.Split("AND")
				.Where(x => regexValue.IsMatch(x))
				.Select(x => regexValue.Replace(x, "$1,$2"));

			// カラム名と値をディクショナリへ格納
			foreach (var columnAndValue in columnAndValues)
			{
				var columnValue = columnAndValue.Split(',');

				ColumnValues.Add(columnValue[0], columnValue[1]);
			}
		}
	}
}
