using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
	public interface ISqlDelete
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 実行結果のメッセージ
		/// </summary>
		public string ResultMessage { get; }

		/// <summary>
		/// 例外発生時のメッセージ
		/// </summary>
		public string ExceptionMessage { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DELETEクエリを実行
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="columnNames">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns></returns>
		public bool Delete(string command, List<string> columnNames, List<string> values);

		/// <summary>
		/// DELETEクエリを実行
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="connStr">接続文字列</param>
		/// <param name="columnNames">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns></returns>
		public bool Delete(string command, string connStr, List<string> columnNames, List<string> values);

		/// <summary>
		/// 接続文字列を内部変数へセット
		/// </summary>
		/// <param name="sqlConnStr">接続文字列のオブジェクト</param>
		public void SetConnectionString(IMySqlConnectionString sqlConnStr);
	}
}
