using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Org.BouncyCastle.Bcpg.OpenPgp;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Sql;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.CodeManager
{
	public class CodeRegisterModel : ICodeRegisterModel
	{
		private string _connStr;

		private SqlSelect select;



		public List<string> LangTypes { get; private set; }
		public List<string> DevTypes { get; private set; }
		public DataTable Table { get; private set; }



		public CodeRegisterModel()
		{
			RefreshValue();
		}



		public void RefreshValue()
		{
			// Keyに"manager"が含まれているか判別
			if (SqlConnectionStrings.ConnectionStrings.TryGetValue("manager", out string connStr) is false)
			{
				return;
			}

			// 接続文字列を取得
			_connStr = connStr;

			// MySQLのSELECT用クラスのインスタンスを生成し、初期化
			select = new SqlSelect(connStr);

			// "manager_language_type"テーブルから"language_type"カラムを文字列のリストで取得
			LangTypes = select.SelectedColumnToList("language_type", "SELECT `language_type` FROM `manager_language_type`;");
			DevTypes = select.SelectedColumnToList("develop_type", "SELECT `develop_type` FROM `manager_develop_type`;");

			int a = 0;
		}
		public List<string> SetDevTpe(string selectedValue)
		{
			string subQuery = "SELECT `script_type` FROM `manager_language_type` WHERE `language_type` = @language_type;";
			List<string> columnNames = new List<string>() { "language_type" };
			List<string> values = new List<string>() { selectedValue };

			// "script_type" を取得
			string subResult = select.GetJustOneSelectedItem("script_type", subQuery, columnNames, values);
			
			// "subResult"から"develop_type"を取得
			string query = $"SELECT `develop_type` FROM `manager_develop_type` WHERE `script_type` = '{subResult}';";
			DevTypes = select.SelectedColumnToList("develop_type", query);
			return DevTypes;
		}
		public DataTable SetTable(string selectedLangType, string selectedDevType)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(@"SELECT CONCAT(`develop_type_code`, `language_type_code`)");
			sb.AppendLine("FROM `manager_language_type` AS `lang`");
			sb.AppendLine("INNER JOIN `manager_develop_type` AS `dev`");
			sb.AppendLine("ON `lang`.`script_type` = `dev`.`script_type`");
			sb.AppendLine("WHERE `language_type` = @language_type");
			sb.AppendLine("AND `develop_type` = @develop_type;");

			// 
			string subQuery = sb.ToString();
			List<string> columnNames = new List<string>() { "language_type", "develop_type" };
			List<string> values = new List<string>() { selectedLangType, selectedDevType };
			// 
			string tmpColumn = "CONCAT(`develop_type_code`, `language_type_code`)";
			// 
			string subResult = select.GetJustOneSelectedItem(tmpColumn, subQuery, columnNames, values);

			// 
			string query = $"SELECT * FROM `manager_register_code` WHERE `develop_number` LIKE '{subResult}%';";

			// 
			Table = select.Select(query);

			return Table;
		}
	}
}
