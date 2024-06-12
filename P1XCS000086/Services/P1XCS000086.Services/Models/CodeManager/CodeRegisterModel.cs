using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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



		public CodeRegisterModel()
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
			LangTypes = select.SelectedColumnToList("languafe_type", "SELECT `language_type` FROM `manager_language_type`;");

		}



		public List<string> GetDatabaseField()
		{

		}
	}
}
