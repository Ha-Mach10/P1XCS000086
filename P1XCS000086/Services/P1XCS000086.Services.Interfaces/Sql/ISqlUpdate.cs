using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
	public interface ISqlUpdate
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 実行結果のメッセージ
		/// </summary>
		public string ResultMessage { get; }

		/// <summary>
		/// 例外のメッセージ
		/// </summary>
		public string ExceptionMessage { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// UPDATEクエリを実行する
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="connStr">接続文字列</param>
		/// <param name="columnNames">パラメータクエリに使用するカラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns>クエリの成否</returns>
		public bool Update(string command, string connStr, List<string> columnNames, List<string> values);
	}
}
