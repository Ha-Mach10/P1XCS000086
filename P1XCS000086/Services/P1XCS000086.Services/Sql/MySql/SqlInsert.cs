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
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private string _connStr;
		private string _command;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string ResultMessage { get; private set; } = "";
		public string ExceptionMessage {  get; private set; } = string.Empty;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public SqlInsert() { }
		public SqlInsert(string connStr) : this()
		{
			_connStr = connStr;
		}
		public SqlInsert(string connStr, string command) : this(connStr)
		{
			_command = command;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// INSERTクエリを実行
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="columns">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns>クエリの成否</returns>
		public bool Insert(string connStr, string command, List<string> columns, List<string> values)
		{
			bool result = ExecuteInsert(columns, values, command, connStr);
			return result;
		}
		/// <summary>
		/// INSERTクエリを実行
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="columns">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns>クエリの成否</returns>
		public bool Insert(string command, List<string> columns, List<string> values)
		{
			bool result = ExecuteInsert(columns, values, command, _connStr);
			return result;
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// INSERTクエリの実行
		/// </summary>
		/// <param name="columns">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <param name="command">クエリコマンド</param>
		/// <param name="connStr">接続文字列</param>
		/// <returns></returns>
		private bool ExecuteInsert(List<string> columns, List<string> values, string command, string connStr = "")
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
						/*
						for (int i = 0; i > columns.Count; i++)
						{
							cmd.Parameters.Add(new MySqlParameter(columns[i], values[i]));
						}
						*/
						var paramaters = columns.Zip(values, (column, value) => new MySqlParameter(column, value)).ToArray();
						cmd.Parameters.AddRange(paramaters);

						// コマンドを実行
						var result = cmd.ExecuteNonQuery();
						// タイムアウト設定の変更
						cmd.CommandTimeout = 10000;

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
			// コネクションおよびコマンド生成時に例外が発生した場合
			catch (MySqlException ex)
			{
				ExceptionMessage = $"発生した例外：{ex.Message}\n\n発生元：{ex.Source}";
				return false;
			}



			// 挿入に成功
			ResultMessage = "データ更新に成功";
			ExceptionMessage = string.Empty;
			return true;
		}
	}
}
