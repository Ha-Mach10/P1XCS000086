using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlInsert : ISqlInsert
	{
		private string _connStr;
		private string _command;


		public string ResultMessage { get; private set; }
		public string ExceptionMessage {  get; private set; }

		
		public SqlInsert(string connStr)
		{
			_connStr = connStr;
		}
		public SqlInsert(string connStr, string command) : this(connStr)
		{
			_command = command;
		}


		public bool Insert(string command, List<string> columns, List<string> values)
		{
			ResultMessage = string.Empty;

			try
			{
				// コネクションを生成し、コマンドを生成
				using (MySqlConnection conn = new MySqlConnection(_connStr))
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
			catch(MySqlException ex)
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
