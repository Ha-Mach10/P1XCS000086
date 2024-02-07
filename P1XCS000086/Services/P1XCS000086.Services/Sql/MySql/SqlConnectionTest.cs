using MySql.Data.MySqlClient;

using P1XCS000086.Services.Interfaces.Sql;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlConnectionTest : ISqlConnectionTest
	{
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// SQLの接続テスト用メソッド
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		/// <returns>接続の成功/失敗</returns>
		public bool SqlConnection(string connStr)
		{
			if (connStr == string.Empty)
			{
				return false;
			}

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connStr))
				{
					conn.Open();
					conn.Close();

					return true;
				}
			}
			catch (MySqlException msex)
			{
				Debug.WriteLine(msex.Message);

				return false;
			}
		}
	}
}
