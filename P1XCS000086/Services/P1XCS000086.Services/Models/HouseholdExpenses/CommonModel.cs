using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using P1XCS000086.Services.Sql;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.HouseholdExpenses
{
	internal class CommonModel
	{
		// Fields
		
		private SqlSelect _select;
		private SqlInsert _insert;
		private SqlUpdate _update;
		private SqlDelete _delete;



		// Properties

		#region 接続文字列
		public string ConnStrManager { get; private set; }
		public string ConnStrCommonManager { get; private set; }
		public string ConnStrHouseholdExpensesManager { get; private set; }
		#endregion

		



		// Constructor
		public CommonModel()
		{
			// データベース名のリストを定義
			List<string> databaseName = new List<string>
			{
				"manager",
				"common_manager",
				"household_expenses_manager"
			};

			// 
			foreach (var name in databaseName)
			{
				// Keyに"manager"が含まれているか判別
				if (SqlConnectionStrings.ConnectionStrings.TryGetValue(name, out string connStr) is false)
				{
					// 取得できない場合
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
					case "household_expenses_manager":
						ConnStrHouseholdExpensesManager = connStr;
						_select = new SqlSelect(connStr);
						break;
				}
			}
		}



		
	}
}
