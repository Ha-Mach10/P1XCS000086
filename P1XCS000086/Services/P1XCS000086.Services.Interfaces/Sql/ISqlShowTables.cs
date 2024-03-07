using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
    public interface ISqlShowTables
    {
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// テーブル一覧を取得する
		/// </summary>
		/// <returns>テーブル一覧のリスト</returns>
		public List<string> ShowTables();
		/// <summary>
		/// テーブル一覧を取得する
		/// </summary>
		/// <param name="databaseName">データベース名称</param>
		/// <returns>テーブル一覧のリスト</returns>
		public List<string> ShowTables(string databaseName);

		/// <summary>
		/// 接続文字列のセット
		/// </summary>
		/// <param name="connStr"></param>
		public void SetConnectionString(string connStr);

	}
}
