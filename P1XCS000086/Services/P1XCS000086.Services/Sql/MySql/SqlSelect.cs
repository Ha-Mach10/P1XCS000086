using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces;

namespace P1XCS000086.Services.Sql.MySql
{
	internal class SqlSelect : ISqlSelect
	{
		private string _conStr;
		private string _command;


		public SqlSelect(string conStr, string command)
		{
			_conStr = conStr;
			_command = command;
		}


		public string GenerateQuery(string table, List<string> columns = null)
		{
			string query = string.Empty;

			
			// 
			if (columns == null || columns.Count == 0)
			{
				query = $"SELECT * FROM {table}";
				return query;
			}

			// 
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT");
			foreach (string column in columns)
			{
				if (columns.Last() == column)
				{
					sb.Append(column);
					break;
				}
				sb.Append(column);
				sb.Append(',');
			}
			sb.Append($"FROM {table}");

			return query;
		}


		public DataTable Select(string query)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlDataAdapter adapter = new MySqlDataAdapter(_command, _conStr))
				{
					adapter.Fill(dt);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return dt;
		}
	}
}
