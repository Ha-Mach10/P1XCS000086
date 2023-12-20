using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
			_command = command;
		}


		public DataTable Select()
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_conStr))
				{
					// コネクションを開く
					conn.Open();

					// アダプターを生成
					using (MySqlDataAdapter adapter = new MySqlDataAdapter(_command, _conStr))
					{
						adapter.Fill(dt);
					}
				}
			}
			catch(MySqlException ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return dt;
		}
		public DataTable Select(string whereColumn, string whereValue)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_conStr))
				{
					conn.Open();

					// 
					using (MySqlCommand command = conn.CreateCommand())
					{
						command.CommandText = _command;

						// パラメータクエリのパラメータを取得する
						// 「@[A-z]{1,100}」は@から始まり、A~Z,a~zまでのいずれかの文字が1~100回繰り返されることを示す
						string whereValueStr = Regex.Match(_command, @"@[A-z]{1,100}").ToString();

						command.Parameters.Add(new MySqlParameter(whereValueStr, whereValue));

						// アダプターを生成
						using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
						{
							// SELECTクエリで取得したテーブルを格納
							adapter.Fill(dt);
						}
					}
				}
			}
			catch (MySqlException ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return dt;
		}
		public DataTable Select(List<string> whereColumns, List<string> whereValues)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_conStr))
				{
					conn.Open();

					// 
					using (MySqlCommand command = conn.CreateCommand())
					{
						command.CommandText = _command;

						// パラメータクエリのパラメータを取得する
						// 「@[A-z]{1,100}」は@から始まり、A~Z,a~zまでのいずれかの文字が1~100回繰り返されることを示す
						var matches = Regex.Matches(_command, @"@[A-z]{1,100}");
						int count = 0;
						foreach (Match match in matches)
						{
							command.Parameters.Add(new MySqlParameter(match.ToString(), whereValues[count]));
							count++;
						}

						// アダプターを生成
						using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
						{
							// SELECTクエリで取得したテーブルを格納
							adapter.Fill(dt);
						}
					}
				}
			}
			catch (MySqlException ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return dt;
		}
	}
}
