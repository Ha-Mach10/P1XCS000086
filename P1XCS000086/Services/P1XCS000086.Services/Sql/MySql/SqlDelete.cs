using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlDelete : ISqlDelete
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private string _connStr;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 実行結果のメッセージ
		/// </summary>
		public string ResultMessage { get; private set; } = string.Empty;

		/// <summary>
		/// 例外発生時のメッセージ
		/// </summary>
		public string ExceptionMessage { get; private set; } = string.Empty;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public SqlDelete() { }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DELETEクエリを実行
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="columnNames">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns></returns>
		public bool Delete(string command, List<string> columnNames, List<string> values)
		{
			bool result = ExecuteDelete(columnNames, values, command, _connStr);
			return result;
		}

		/// <summary>
		/// DELETEクエリを実行
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="connStr">接続文字列</param>
		/// <param name="columnNames">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns></returns>
		public bool Delete(string command, string connStr, List<string> columnNames, List<string> values)
		{
			bool result = ExecuteDelete(columnNames, values, command, connStr);
			return result;
		}

		/// <summary>
		/// 接続文字列を内部変数へセット
		/// </summary>
		/// <param name="sqlConnStr">接続文字列のオブジェクト</param>
		public void SetConnectionString(IMySqlConnectionString sqlConnStr)
		{
			// 接続文字列が設定されている場合、内部変数へ値をセット
			if (sqlConnStr.IsGetConnectionString(out string connStr))
			{
				_connStr = connStr;
			}
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// DELETEクエリの実行
		/// </summary>
		/// <param name="columns">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <param name="command">クエリコマンド</param>
		/// <param name="connStr">接続文字列</param>
		/// <returns></returns>
		private bool ExecuteDelete(List<string> columns, List<string> values, string command, string connStr = "")
		{
			// 
			string connectionString = connStr;
			if (connStr == "")
			{
				connectionString = _connStr;
			}

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
							ResultMessage = "データ更新の失敗";

							// ロールバックする
							tran.Rollback();
							return false;
						}

						// コミットする
						tran.Commit();
					}
					// データベース操作で例外が発生した場合
					catch (MySqlException sqlEx)
					{
						ExceptionMessage = $"発生した例外：{sqlEx.Message}\n\n発生元：{sqlEx.Source}\n\nMySQLエラーコード：{sqlEx.Code}";
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionMessage = $"発生した例外：{ex.Message}\n\n発生元：{ex.Source}";
				return false;
			}


			// 削除に成功
			ResultMessage = "データ更新に成功";
			ExceptionMessage = string.Empty;
			return true;
		}
	}
}
