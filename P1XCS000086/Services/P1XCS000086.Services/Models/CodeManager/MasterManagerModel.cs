using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using P1XCS000086.Services.Data;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.CodeManager
{
	public class MasterManagerModel : IMasterManagerModel
	{
		// Enum
		public enum QueryType
		{
			None	= 0,
			Update	= 1,
			Insert	= 2,
			Delete	= 3,
		}

		// Fields
		private CommonModel _common;
		private SqlSelect _selectManager;
		private SqlSelect _selectCommonManager;



		// Properties
		public List<string> DatabaseNames { get; }
		public List<(string, string)> TableNames { get; }
		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Constructor
		public MasterManagerModel()
		{
			_common = new CommonModel();

			_selectManager = new SqlSelect(_common.ConnStrManager);
			_selectCommonManager = new SqlSelect(_common.ConnStrCommonManager);

			DatabaseNames = _common.DatabaseNames;
			UseAppMajor = _common.UseAppMajor;
			UseAppRange = _common.UseAppRange;
		}



		// Public Methods

		/// <summary>
		/// 取得したテーブルからカラム名と論理名のタプルリストを取得
		/// </summary>
		/// <param name="selectedDatabaseName"></param>
		/// <returns></returns>
		public List<(string columnName, string logicalName)> GetTableNameSets(string selectedDatabaseName)
		{
			return _common.GetTranclateTableNames(selectedDatabaseName);
		}
		/// <summary>
		/// 任意のテーブルをDataTableで取得
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable SearchTable(string databaseName, string tableName)
		{
			string query = $"SELECT * FROM `{tableName}`";

			switch (databaseName)
			{
				case "manager":
					return _selectManager.Select(query);
				case "common_manager":
					return _selectCommonManager.Select(query);
			}

			return null;
		}

		/// <summary>
		/// 2つのDataTableを比較し、差分を抽出してデータベースを更新
		/// </summary>
		/// <param name="beforeTable">編集前のDataTable</param>
		/// <param name="afterTable">編集後のDataTable</param>
		public void TableUpDate(DataTable beforeTable, DataTable afterTable)
		{
			DTConverter dTConverter = new DTConverter(beforeTable, afterTable);

            int a = 0;
		}

		/// <summary>
		/// 2種のDataTableから
		/// </summary>
		/// <param name="dt1"></param>
		/// <param name="dt2"></param>
		/// <returns></returns>
		private List<(List<string> list1, List<string> list2)> DatatableToList(DataTable dt1, DataTable dt2)
		{
			List<(List<string>, List<string>)> rowsValuePairs = new();

			int rowsCount = dt1.Rows.Count > dt2.Rows.Count ? dt1.Rows.Count : dt2.Rows.Count;

			for (int i = 0; i > rowsCount; i++)
			{
				DataRow dt1Row = dt1.Rows[i], dt2Row = dt2.Rows[i];

				List<string> list1 = new(), list2 = new();

				foreach (DataColumn columnName in dt1.Columns)
				{
					list1.Add(dt1Row[columnName].ToString());
					list2.Add(dt2Row[columnName].ToString());
				}

				rowsValuePairs.Add((list1, list2));
			}

			return rowsValuePairs;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="beforeDtRowItems"></param>
		/// <param name="afterDtRowItems"></param>
		/// <returns></returns>
		private (QueryType, List<string>) RowItemExcept(List<string> beforeDtRowItems, List<string> afterDtRowItems)
		{
			if (beforeDtRowItems is not null && afterDtRowItems is null)
			{
				// 編集後テーブルがnullのとき
				return (QueryType.Delete, beforeDtRowItems);
			}
			else if (afterDtRowItems is not null && beforeDtRowItems is null)
			{
				// 編集前テーブルがnullのとき
				return (QueryType.Insert, afterDtRowItems);
			}
			else if (beforeDtRowItems is null && afterDtRowItems is null)
			{
				// 
				return (QueryType.None, null);
			}

			return (QueryType.Update, afterDtRowItems);
		}
	}
}
