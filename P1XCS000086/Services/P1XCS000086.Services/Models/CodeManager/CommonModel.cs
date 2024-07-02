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



		// Properties
		public string ConnStrManager { get; private set; }
		public string ConnStrCommonManager { get; private set; }


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
			_select = new SqlSelect(ConnStrManager);

			// "manager_language_type"テーブルから"language_type"カラムを文字列のリストで取得
			TableNames = _showTable.ShowTables();
			LangTypes = _select.SelectedColumnToList("language_type", "SELECT `language_type` FROM `manager_language_type`;");
			UseAppMajor = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 1;");
			UseAppRange = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 2;");
		}



		// 
		public List<(string, string)> GetTranclateTableNames()
		{
			List<(string tableNameJp, string tableName)> resultTappleList = new List<(string, string)>();

			// クエリ文字列
			string query = $"SELECT `column_name`, `japanese` FROM `table_translator` WHERE `type` = 'Table';";
			
			_select.Select(query).AsEnumerable().Select(x =>
			{
				string tableName = x["column_name"].ToString();
				string tableNameJp = x["japanese"].ToString();

				resultTappleList.Add((tableNameJp, tableName));

				return x;
			}).ToArray();

			return resultTappleList;
		}
	}
}
