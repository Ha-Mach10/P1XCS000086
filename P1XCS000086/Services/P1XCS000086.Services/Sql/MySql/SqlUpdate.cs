using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlUpdate : ISqlUpdate
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private string _connStr;
		private string _command;
		private List<string> _columnNames;
		private List<string> _values;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 実行結果のメッセージ
		/// </summary>
		public string ResultMessage { get; private set; } = string.Empty;

		/// <summary>
		/// 例外のメッセージ
		/// </summary>
		public string ExceptionMessage { get; private set; } = string.Empty;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public SqlUpdate()
		{

		}
		public SqlUpdate(string connStr)
			:this()
		{
			_connStr = connStr;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="columnNames"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public bool Update(string command, List<string> columnNames, List<string> values)
		{
			return Update(_connStr, command, columnNames, values);
		}
		/// <summary>
		/// UPDATEクエリを実行
		/// </summary>
		/// <param name="command">実行するクエリ文</param>
		/// <param name="connStr">接続文字列</param>
		/// <param name="columnNames">パラメータクエリに使用するカラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <returns>クエリの成否</returns>
		public bool Update(string connStr, string command, List<string> columnNames, List<string> values)
		{
			bool result = ExecuteUpdate(command, columnNames, values, connStr);
			return result;
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// UPDATEクエリの実行
		/// </summary>
		/// <param name="command">クエリ</param>
		/// <param name="columnNames">パラメータクエリ用カラム名のリスト</param>
		/// <param name="values">パラメータクエリ用の値リスト</param>
		/// <param name="connStr">接続文字列</param>
		/// <returns></returns>
		private bool ExecuteUpdate(string command, List<string> columns, List<string> values, string connStr = "")
		{
			// 
			string connectionString = connStr;

			if (string.IsNullOrEmpty(connStr) == false)
			{
				// connectionString = _connStr;
				connectionString = $"{_connStr};allowuservariables=True;";
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

						// コマンドパラメータを設定

						var parameters = columns.Zip(values, (column, value) => new MySqlParameter(column, value)).ToArray();
						cmd.Parameters.AddRange(parameters);

						// トランザクションを開始
						tran = conn.BeginTransaction();

						// コマンドを実行
						var result = cmd.ExecuteNonQuery();
						// タイムアウト設定の変更
						// cmd.CommandTimeout = 999999;

						// 実行された結果が1行未満のとき
						if (result <= 0)
						{
							ResultMessage = "データ更新の失敗";

							// ロールバックする
							tran.Rollback();
							return false;
						}

						// コミットする
						tran.Commit();

						// 挿入に成功
						ResultMessage = "データ更新に成功";
						ExceptionMessage = string.Empty;
						return true;
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
			/*
			// 接続文字列を設定
			string connectionString = connStr;
			if (connStr == "")
			{
				return false;
			}

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				using (MySqlCommand cmd = new MySqlCommand(command, conn))
				{
					// トランザクション用のオブジェクトを生成
					MySqlTransaction tran = null;

					try
					{
						// コネクションをオープン
						conn.Open();
						// トランザクションを開始
						conn.BeginTransaction();
						
						// コマンドパラメータを設定
						for (int i = 0; i >= columnNames.Count; i++)
						{
							cmd.Parameters.Add(new MySqlParameter(columnNames[i], values[i]));
						}

						// クエリを実行し、クエリの適用された列数を取得
						int result = cmd.ExecuteNonQuery();

						// データの更新に失敗した場合
						if (result < 1)
						{
							ResultMessage = "データを更新できませんでした";

							// ロールバック
							tran.Rollback();
							return false;
						}

						// トランザクションをコミット
						tran.Commit();
					}
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


			// 更新に成功
			ResultMessage = "データ更新に成功";
			ExceptionMessage = string.Empty;
			return true;
			*/
		}
	}
}
