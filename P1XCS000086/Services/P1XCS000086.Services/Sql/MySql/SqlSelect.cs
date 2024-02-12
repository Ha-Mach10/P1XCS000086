using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Sql.MySql
{
    public class SqlSelect : ISqlSelect
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************
		
		private string _connStr;
		private string _command;



		// ****************************************************************************
		// Constructors
		// ****************************************************************************

		public SqlSelect()
		{

		}
		public SqlSelect(string connStr)
		{
			_connStr = connStr;
		}
		public SqlSelect(string conStr, string command) : this(conStr)
		{
			_command = command;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select()
		{
			return SelectExecute(_command, _connStr);
		}

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="command">クエリ文</param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string command)
		{
			return SelectExecute(command, _connStr);
		}

		/*
		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="whereColumn"></param>
		/// <param name="whereValue"></param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string whereColumn, string whereValue)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connStr))
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

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="whereColumns"></param>
		/// <param name="whereValues"></param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(List<string> whereColumns, List<string> whereValues)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connStr))
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
						// 複数のマッチからループ処理を行う
						foreach (Match match in matches)
						{
							// コマンドパラメータを設定
							command.Parameters.Add(new MySqlParameter(match.ToString(), whereValues[count]));
							// インクリメント
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
		*/



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 接続文字列を内部変数へ登録
		/// </summary>
		/// <param name="connStr">IMySqlConnectionStringインターフェース</param>
		public void SetConnectionString(IMySqlConnectionString sqlConnStr)
		{
			// 
			if (! sqlConnStr.IsGetConnectionString(out string connStr)) { return; }
			// 
			_connStr = connStr;
		}

		/// <summary>
		/// クエリを実行し、取得した列からただ１つの項目を返す
		/// </summary>
		/// <param name="connectionString">接続文字列生成用インターフェース</param>
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <returns>取得されたただひとつの値</returns>
		public string GetJustOneSelectedItem(string columnName, string query)
		{
			// SELECTを実行
			DataTable dt = Select(query);

			// LINQで「dt」から指定のカラムのEnumerableRowCollection<DataRow>を取得
			var rowItmes = dt.AsEnumerable().Select(x => x[columnName]).ToList();

			// もし「rowItems」の項目数が１未満のとき、"Empty"を返す
			if (rowItmes.Count < 1) { return "Empty"; }

			// 取得したコレクションから、LINQで最初の項目を取得
			string item = rowItmes.First().ToString();

			return item;
		}

		/// <summary>
		/// クエリを実行し、取得した列をリストへ格納
		/// </summary>
		/// <param name="command">クエリ</param>
		/// <returns>リスト化された値</returns>
		public List<string> SelectedColumnToList(string columnName, string query)
		{
			// 接続文字列が空の場合、「Non Items」の文字列のみ格納したリストを返す
			if (_connStr == string.Empty)
			{
				return new List<string>() { "Non Items" };
			}

			// SELECTクエリ実行用のクラスをインターフェース経由で生成
			ISqlSelect selectExecute = new SqlSelect(_connStr, query);
			DataTable dt = selectExecute.Select();

			// 戻り値用リストを生成
			List<string> items = new List<string>();

			// LINQで「dt」から指定のカラムのEnumerableRowCollection<DataRow>を取得し、foreachでリストへ格納
			var rowItems = dt.AsEnumerable().Select(x => x[columnName]).ToList();
			foreach (var rowItem in rowItems)
			{
				items.Add(rowItem.ToString());
			}

			return items;
		}


		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// データベースのテーブルをDataTableへ格納して返す
		/// </summary>
		/// <param name="query">クエリ</param>
		/// <param name="connStr">接続文字列</param>
		/// <returns>データベースのテーブル</returns>
		private DataTable SelectExecute(string query, string connStr)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connStr))
				using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
				{
					// queryでselectした
					adapter.Fill(dt);
				}
			}
			catch (MySqlException ex)
			{
				Debug.Print(ex.ToString());
			}

			return dt;
		}
	}
}
