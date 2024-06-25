using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.CodeManager
{
	public class MasterManagerModel : IMasterManagerModel
	{
		// Fields
		private CommonModel _common;
		private SqlSelect _select;



		// Properties
		public List<(string, string)> TableNames { get; }
		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Constructor
		public MasterManagerModel()
		{
			_common = new CommonModel();
			_select = new SqlSelect(_common.ConnStr);

			TableNames = _common.GetTranclateTableNames();
			UseAppMajor = _common.UseAppMajor;
			UseAppRange = _common.UseAppRange;
		}



		// Public Methods
		/// <summary>
		/// 任意のテーブルをDataTableで取得
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable SearchTable(string tableName)
		{
			string query = $"SELECT * FROM `{tableName}`";
			return _select.Select(query);
		}
	}
}
