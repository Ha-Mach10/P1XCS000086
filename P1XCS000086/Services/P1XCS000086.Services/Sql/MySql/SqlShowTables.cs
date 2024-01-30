using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P1XCS000086.Services.Sql.MySql
{
    public class SqlShowTables : ISqlShowTables
	{
		private string _connStr;
		
		
		public SqlShowTables(string connStr)
		{
			_connStr = connStr;
		}


		public List<string> ShowTables()
		{
			string command = "SHOW TABLES FROM manager;";
			List<string> tables = GetShowTableItems(command);

			return tables;
		}
		public List<string> ShowTables(string databaseName)
		{
			string command = $"SHOW TABLES FROM {databaseName};";
			List<string> tables = GetShowTableItems(command);

			return tables;
		}

        private List<string> GetShowTableItems(string command)
		{
			List<string> tables = new List<string>();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connStr))
				{
					// コネクションを開く
					conn.Open();

					// アダプターを生成
					using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, _connStr))
					{
						DataTable dt = new DataTable();
						adapter.Fill(dt);

						tables = dt.AsEnumerable().Select(x => x.ToString()).ToList();
					}
				}
			}
			catch (Exception ex)
			{
				return new List<string>() { "no data" };
			}

			return tables;
		}
    }
}
