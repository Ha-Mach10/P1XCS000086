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
		public string ConnStr { get; }
		public List<string> TableNames { get; }
		public List<string> LangTypes { get; }
		public List<string> DevTypes { get; }

		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Constructor
		public CommonModel()
		{
			// Keyに"manager"が含まれているか判別
			if (SqlConnectionStrings.ConnectionStrings.TryGetValue("manager", out string connStr) is false)
			{
				return;
			}

			ConnStr = connStr;

			// 
			_showTable = new SqlShowTables();
			_select = new SqlSelect();

			// "manager_language_type"テーブルから"language_type"カラムを文字列のリストで取得
			TableNames = _showTable.ShowTables();
			LangTypes = _select.SelectedColumnToList("language_type", "SELECT `language_type` FROM `manager_language_type`;");
			UseAppMajor = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 1;");
			UseAppRange = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 2;");
		}



		// 
		public List<(string, string)> GetTranclateTableName()
		{
			List<(string, string)> resultTappleList = new List<(string, string)>();

			// クエリ文字列
			string query = $"SELECT `column_name`, `japanese` FROM `table_translator` WHERE `type` = 'Table';";
			
			_select.Select(query).AsEnumerable().Select(x =>
			{
				string columnName = x["column_name"].ToString();
				string japanese = x["japanese"].ToString();

				resultTappleList.Add((columnName, japanese));

				return x;
			}).ToArray();

			return resultTappleList;
		}
		public List<string> GetSingleColumnFromTable(string tableName, string columnName, List<string> columns = null, List<string> values = null)
		{
			string query = string.Empty;
			List<string> result = new List<string>();

			if ((columns is not null && values is not null)
				&& columns.Count == values.Count)
			{
				string param = 
				query = $"SELECT `{columnName}` FROM `{tableName}`";
			}
			else if (columns is null || values is null)
			{
				query = $"SELECT `{columnName}` FROM `{tableName}`;";
				result = _select.SelectedColumnToList(columnName, query);
			}
		}
	}
}
