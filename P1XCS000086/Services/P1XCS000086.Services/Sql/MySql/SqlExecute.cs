using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Mysqlx.Prepare;
using System.Collections;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlExecute
	{
		private string _connStr;



		public string ExceptionMessage { get; private set; }
		public string ResultMessage { get; private set; }



		public SqlExecute(string connStr)
		{
			_connStr = connStr;
		}



		public bool Execute(string query)
		{
			if (string.IsNullOrEmpty(_connStr))
			{
				ResultMessage = "接続文字列が設定されていません。";
				return false;
			}
			try
			{
				using (MySqlConnection conn = new(_connStr))
				using (MySqlCommand command = new(query, conn))
				{
					// トランザクションを宣言
					MySqlTransaction tran = null;

					// 接続をオープン
					conn.Open();

					// トランザクションの開始
					tran = conn.BeginTransaction();
					// コマンドの実行
					var result = command.ExecuteNonQuery();

					// コマンドの失敗
					if (result <= 0)
					{
						ResultMessage = "クエリの実行に失敗しました。";

						// ロールバック処理
						tran.Rollback();
						return false;
					}

					// トランザクションをコミット
					tran.Commit();

					ResultMessage = "クエリのトランザクションは正常にコミットされました。";
				}
			}
			catch (MySqlException msex)
			{
				ExceptionMessage = msex.Message;
				ResultMessage = "SQL実行時に例外が発生しました。";

				return false;
			}

			return true;
		}
	}
}
