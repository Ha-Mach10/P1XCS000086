﻿using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace P1XCS000086.Services.Sql.MySql
{
	public class MySqlQueryParser
	{
		// *****************************************************************************
		// Enumerators
		// *****************************************************************************

		private enum QueryType
		{
			SELECT,
			INSERT,
			UPDATE,
			DELETE,
			NONE,
		}



		// *****************************************************************************
		// Properties
		// *****************************************************************************

		public string QueryTypeName { get; private set; } = string.Empty;
		public List<string> ColumnNames { get; private set; } = new();
		public string TableName { get; private set; } = string.Empty;
		public Dictionary<string, string> ColumnValues { get; private set; } = new();
		public bool IsParse { get; private set; } = false;



		// *****************************************************************************
		// Constructor
		// *****************************************************************************

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



		// *****************************************************************************
		// Private Methods
		// *****************************************************************************

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
			// 
			const string from = "FROM";

			// 
			string selectColumns = GetPhrase(QueryType.SELECT.ToString(), query, from).Replace(" ", "");

			// カラム名指示に「`（バッククォート）」が使用されているか判別
			// 使用されていた場合は除去
			if (selectColumns.Contains('`'))
			{
				selectColumns = selectColumns.Replace("`", "");
			}

			// カラム名を区切るカンマを基点に文字列を分割し、カラム名リストのプロパティへ格納
			ColumnNames = selectColumns.Split(',').ToList();
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
			// 
			const string orderBy = "ORDER BY";
			const string limit = "LIMIT";

			string whereColumnValue = string.Empty;

			// WHEREが含まれていない場合は処理を抜ける
			if (query.Contains(where) is false) return;
			else if (query.Contains(orderBy))
			{
				whereColumnValue = GetPhrase(where, query, orderBy);
			}
			else if (query.Contains(limit))
			{
				whereColumnValue = GetPhrase(where, query, limit);
			}

			// カラム名のバッククォート(`)を空文字に置換
			whereColumnValue.Replace("`", "");
			// カラム名のシングルクォート(')を空文字に置換
			whereColumnValue.Replace("'", "");
			// 空白文字列を空文字列に置換
			whereColumnValue.Replace(" ", "");
			/*
			// 「WHERE」以降の文字列のインデックスを取得
			int whereValuePosition = query.IndexOf("WHERE") + where.Length;
			// 「WHERE」以降の文字列を取得
			string whereKeywordAfter = query.Substring(whereValuePosition);
			*/

			// 正規表現
			Regex regexValue = new Regex("`([A-z0-9_]+)` *= *(.+)");

			// "AND"を基点に切り取る
			var columnAndValues = whereColumnValue.Split("AND")
				.Where(x => regexValue.IsMatch(x))
				.Select(x => regexValue.Replace(x, "$1,$2"));

			// カラム名と値をディクショナリへ格納
			foreach (var columnAndValue in columnAndValues)
			{
				var columnValue = columnAndValue.Split(',');

				ColumnValues.Add(columnValue[0], columnValue[1]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="query"></param>
		/// <param name="nextKeyword"></param>
		/// <returns></returns>
		private string GetPhrase(string keyword, string query, string nextKeyword)
		{
			// キーワード以後の文字列のインデックスを取得
			int startIndex = query.IndexOf(keyword) + keyword.Length;

			// 次に出現するキーワードまでの文字列の長さを取得
			int length = query.IndexOf(nextKeyword) - keyword.Length;

			int startIndexDeff = startIndex;

			// キーワードの前後にバッククォートが付属しているかチェックする
			if (Regex.IsMatch(query, $"`{keyword}`"))
			{
				startIndex = startIndex + 2;

				startIndexDeff = Math.Abs(startIndexDeff - startIndex);
			}
			if (Regex.IsMatch(query, $"`{nextKeyword}`") && startIndexDeff > 0)
			{
				length = query.IndexOf(nextKeyword) - (startIndexDeff + keyword.Length);
			}

			// 取得したインデックス位置と文字長さからテーブルやカラム名等を取得する
			string resultString = query.Substring(startIndex, length).Replace("`", "").Replace(" ", "");

			return resultString;
		}


	}
}
