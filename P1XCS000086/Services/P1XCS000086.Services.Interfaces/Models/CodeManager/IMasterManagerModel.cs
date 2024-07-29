using P1XCS000086.Services.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManager
{
	public interface IMasterManagerModel
	{
		// Properties
		public List<string> DatabaseNames { get; }
		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Public Methods

		/// <summary>
		/// 取得したテーブルからカラム名と論理名のタプルリストを取得
		/// </summary>
		/// <param name="selectedDatabaseName"></param>
		/// <returns></returns>
		public List<(string columnName, string logicalName)> GetTableNameSets(string selectedDatabaseName);
		/// <summary>
		/// 任意のテーブルをDataTableで取得
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable SearchTable(string databaseName, string tableName);
		/// <summary>
		/// 2つのDataTableを比較し、差分を抽出してデータベースを更新
		/// </summary>
		/// <param name="beforeTable">編集前のDataTable</param>
		/// <param name="afterTable">編集後のDataTable</param>
		public void TableUpDate(IDTConveter dtConverter, DataTable beforeTable, DataTable afterTable, string databaseName, string tableName);
	}
}
