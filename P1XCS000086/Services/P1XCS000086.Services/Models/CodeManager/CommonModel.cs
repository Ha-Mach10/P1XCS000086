using System;
using System.Collections.Generic;
using System.Text;

using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.Sql;

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

	}
}
