using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Sql.MySql
{
    public class SqlInsert : ISqlInsert
	{
		private string _connStr;
		private string _command;


		public string ResultMessage { get; private set; }
		public string ExceptionMessage {  get; private set; }

		
		public SqlInsert() { }
		public SqlInsert(string connStr) : this()
		{
			_connStr = connStr;
		}
		public SqlInsert(string connStr, string command) : this(connStr)
		{
			_command = command;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="connStr"></param>
		/// <param name="command"></param>
		/// <param name="columns"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool Insert(string connStr, string command, List<string> columns, List<string> values)
		{
			bool result = ExecuteInsert(columns, values, command, connStr);
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="columns"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool Insert(string command, List<string> columns, List<string> values)
		{
			bool result = ExecuteInsert(columns, values, command, _connStr);
			return result;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="columns"></param>
		/// <param name="values"></param>
		/// <param name="command"></param>
		/// <param name="connStr"></param>
		/// <returns></returns>
		private bool ExecuteInsert(List<string> columns, List<string> values, string command, string connStr = "")
		{
			// 
			string connectionString = connStr;
			if (connStr == "") { connectionString = _connStr; }

			// 
			ResultMessage = string.Empty;

			try
			{
				// コネクションを生成し、コマンドを生成
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				using (MySqlCommand cmd = new MySqlCommand(command, conn))
				{
					MySqlTransaction tran = null;

					try
					{
						// コネクションを開く
						conn.Open();
						// トランザクションを開始
						tran = conn.BeginTransaction();

						// コマンドパラメータを設定（SQLインジェクション対策）
						for (int i = 0; i > columns.Count; i++)
						{
							cmd.Parameters.Add(new MySqlParameter(columns[i], values[i]));
						}

						// コマンドを実行
						var result = cmd.ExecuteNonQuery();

						// 実行された結果が1行未満のとき
						if (result != 1)
						{
							ResultMessage = "データを更新できませんでした。";

							// ロールバックする
							tran.Rollback();
							return false;
						}

						// コミットする
						tran.Commit();
					}
					// データベース操作で例外が発生した場合
					catch (MySqlException ex)
					{
						ExceptionMessage = $"Error: {ex.Message}";
						return false;
					}
				}
			}
			// コネクションおよびコマンド生成時に例外が発生した場合
			catch (MySqlException ex)
			{
				ExceptionMessage = ex.Message;
				return false;
			}



			// 成功した場合
			ResultMessage = "success";
			ExceptionMessage = string.Empty;
			return true;
		}
	}
}
