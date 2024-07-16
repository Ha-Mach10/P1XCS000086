using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
			List<(string, List<string>)> queryAndValues = new();

			List<List<string>> bRowItems = DatatableToList(beforeTable);
			List<List<string>> aRowItems = DatatableToList(afterTable);

			
			foreach (List<string> bRowItem in bRowItems)
			{

			}

			// var ssss = aRowItems.Except(bRowItems).ToList();
			// var exceptedRowItems = aRowItems.Except().ToList();

			// foreach (List<string> rowI)

			/*
            foreach (var columnName in columnNames)
            {
				List<string> bRow = new();
                foreach (var rowItem in beforeTable.AsEnumerable().ToList())
				{
					bRow.Add(rowItem.ToString());
				}

				List<string> aRow = new();
				foreach(var rowItem in afterTable.AsEnumerable().ToList())
				{
					aRow.Add(rowItem.ToString());
				}

				// 列の差分が1以上の場合、
				if (bRow.Except(aRow).Count() >= 1)
				{

				}
            }
			*/


            int a = 0;
		}

		private List<List<string>> DatatableToList(DataTable dt)
		{
			List<string> columnNames = new();
			foreach (var column in dt.Columns)
			{
				columnNames.Add(column.ToString());
			}

			List<List<string>> dtItems = new();

			foreach (var rowItem in  dt.AsEnumerable())
			{
				List<string> dtRowItems = new();

				foreach (var columnName in columnNames)
				{
					dtRowItems.Add(rowItem[columnName].ToString());
				}

				dtItems.Add(dtRowItems);
			}

			return dtItems;
		}
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
