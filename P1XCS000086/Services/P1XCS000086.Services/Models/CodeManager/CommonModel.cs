using System;
using System.Collections.Generic;
using System.Text;

using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.Sql;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.ComponentModel;
using System.Linq;
using System.Data;

namespace P1XCS000086.Services.Models.CodeManager
{
	internal class CommonModel
	{
		// Fields
		private SqlShowTables _showTable;
		private SqlSelect _select;
		private SqlSelect _selectCommonManager;



		// Properties
		public string ConnStrManager { get; private set; }
		public string ConnStrCommonManager { get; private set; }

		public List<string> DatabaseNames { get; }

		public List<string> TableNames { get; }
		public List<string> LangTypes { get; }
		public List<string> DevTypes { get; }

		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Constructor
		public CommonModel()
		{
			List<string> databaseName = new List<string>
			{
				"manager",
				"common_manager"
			};
			foreach (var name in databaseName)
			{
				// Keyに"manager"が含まれているか判別
				if (SqlConnectionStrings.ConnectionStrings.TryGetValue(name, out string connStr) is false)
				{
					continue;
				}

				switch (name)
				{
					case "manager":
						ConnStrManager = connStr;
						break;
					case "common_manager":
						ConnStrCommonManager = connStr;
						break;
				}
			}


			// 
			_showTable = new SqlShowTables(ConnStrManager);
			// 
			_select = new SqlSelect(ConnStrManager);
			_selectCommonManager = new SqlSelect(ConnStrCommonManager);

			// 以下、共通処理

			// "manager_language_type"テーブルから"language_type"カラムを文字列のリストで取得
			DatabaseNames = _selectCommonManager.SelectedColumnToList("database_name", "SELECT `database_name` FROM `database_structure` WHERE `type` = 'database';");
			TableNames = _showTable.ShowTables();
			LangTypes = _select.SelectedColumnToList("language_type", "SELECT `language_type` FROM `manager_language_type`;");
			UseAppMajor = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 1;");
			UseAppRange = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 2;");
		}



		// 
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<(string, string)> GetTranclateTableNames(string databaseName)
		{
			List<(string tableNameJp, string tableName)> resultTappleList = new List<(string, string)>();

			List<string> columnNames = new List<string>
			{
				"column_name",
				"logical_name",
			};

			// クエリ文字列
			string query = $"SELECT `{columnNames[0]}`, `{columnNames[1]}` FROM `database_structure` WHERE `type` = 'Table' AND `database_name` = '{databaseName}';";
			
			_selectCommonManager.Select(query).AsEnumerable().Select(x =>
			{
				string tableName = x[columnNames[0]].ToString();
				string tableNameJp = x[columnNames[1]].ToString();

				resultTappleList.Add((tableNameJp, tableName));

				return x;
			}).ToArray();

			return resultTappleList;
		}
	}
}
