using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Pqc.Crypto.Lms;
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
		private List<string> _columnNames;
		private List<string> _values;
		



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
		/// <param name="command">クエリ文</param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string command)
		{
			return SelectExecute(command, _connStr);
		}
		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		/// <param name="command">クエリ文</param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string connStr, string command)
		{
			return SelectExecute(command, connStr);
		}

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="command">クエリ文</param>
		/// <param name="columnNames">カラム名のリスト</param>
		/// <param name="values">値のリスト</param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string command, List<string> columnNames, List<string> values)
		{
			return SelectExecute(command, _connStr, columnNames, values);
		}

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="command">クエリ文</param>
		/// <param name="columnNames">カラム名のリスト</param>
		/// <param name="values">値のリスト</param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string connStr, string command, List<string> columnNames, List<string> values)
		{
			return SelectExecute(command, connStr, columnNames, values);
		}

		/// <summary>
		/// 接続文字列を内部変数へ登録
		/// 
		/// ※使用しない
		/// </summary>
		/// <param name="sqlConnStr">IMySqlConnectionStringインターフェース</param>
		public void SetConnectionString(IMySqlConnectionString sqlConnStr)
		{
			// 
			if (! sqlConnStr.IsGetConnectionString(out string connStr)) { return; }
			// 
			_connStr = connStr;
		}

		/// <summary>
		/// プレースホルダ用のカラム名と値のリストを登録
		/// 
		/// ※使用しない
		/// </summary>
		/// <param name="columnNames">カラム名のリスト</param>
		/// <param name="values">値のリスト</param>
		public void SetColumnNamesAndValues(List<string> columnNames, List<string> values)
		{
			_columnNames = columnNames;
			_values = values;
		}

		/// <summary>
		/// クエリを実行し、取得した列からただ１つの項目を返す
		/// </summary>
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
		/// クエリを実行し、取得した列からただ１つの項目を返す
		/// </summary>
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <param name="columnNames">カラム名のリスト</param>
		/// <param name="values">値のリスト</param>
		/// <returns>取得されたただひとつの値</returns>
		public string GetJustOneSelectedItem(string columnName, string query, List<string> columnNames, List<string> values)
		{
			// SELECTを実行
			DataTable dt = Select(query, columnNames, values);

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
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <returns>リスト化された値</returns>
		public List<string> SelectedColumnToList(string columnName, string query)
		{
			return SelectedColumnToList(_connStr, columnName, query);
		}
		/// <summary>
		/// クエリを実行し、取得した列をリストへ格納
		/// </summary>
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <returns>リスト化された値</returns>
		public List<string> SelectedColumnToList(string connStr, string columnName, string query)
		{
			if (IsExistConnectionString(connStr, out List<string> failList))
			{
				return failList;
			}

			return DataTableToList(Select(connStr, query), columnName);
		}
		/// <summary>
		/// クエリを実行し、取得した列をリストへ格納
		/// </summary>
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <param name="columnNames">パラメータ用のカラム名リスト</param>
		/// <param name="values">パラメータ用の値リスト</param>
		/// <returns>リスト化された値</returns>
		public List<string> SelectedColumnToList(string columnName, string query, List<string> columnNames, List<string> values)
		{
			return SelectedColumnToList(_connStr, columnName, query, columnNames, values);
		}
		/// <summary>
		/// クエリを実行し、取得した列をリストへ格納
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <param name="columnNames">パラメータ用のカラム名リスト</param>
		/// <param name="values">パラメータ用の値リスト</param>
		/// <returns>リスト化された値</returns>
		public List<string> SelectedColumnToList(string connStr, string columnName, string query, List<string> columnNames, List<string> values)
		{
			if (IsExistConnectionString(connStr, out List<string> failList))
			{
				return failList;
			}

			return DataTableToList(Select(connStr, query, columnNames, values), columnName);
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
				using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connStr))
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

		/// <summary>
		/// データベースのテーブルをDataTableへ格納して返す
		/// </summary>
		/// <param name="query">クエリ</param>
		/// <param name="connStr">接続文字列</param>
		/// <param name="columnNames"></param>
		/// <param name="values"></param>
		/// <returns>データベースのテーブル</returns>
		private DataTable SelectExecute(string query, string connStr, List<string> columnNames, List<string> values)
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connStr))
				using (MySqlCommand cmd = new MySqlCommand(query, conn))
				{
					conn.Open();

					// パラメータを設定
					int count = 0;
					foreach (string columnName in columnNames)
					{
						// パラメータの追加
						cmd.Parameters.Add(new MySqlParameter(columnName, values[count]));
						count++;
					}

					using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
					{
						// queryでselectした
						adapter.Fill(dt);
					}

					conn.Close();
				}
			}
			catch (MySqlException ex)
			{
				Debug.Print(ex.ToString());
			}

			return dt;
		}

		/// <summary>
		/// DataTableから指定したカラム名の列からList<string>へ変換する
		/// </summary>
		/// <param name="dt">リストへ変換するDataTable</param>
		/// <param name="columnName">取得したDataTableのカラム名</param>
		/// <returns></returns>
		private List<string> DataTableToList(DataTable dt, string columnName)
		{
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

		/// <summary>
		/// 接続文字列のnull/空文字列チェック
		/// </summary>
		/// <param name="connStr">判定する接続文字列</param>
		/// <param name="failList">成功時/失敗時の値を格納するリスト</param>
		/// <returns>空文字列またはnullでないtrue. それ以外false.</returns>
		private bool IsExistConnectionString(string connStr, out List<string> failList)
		{
			// 接続文字列が空またはnullの場合、「Non Items」の文字列のみ格納したリストを返す
			if (string.IsNullOrEmpty(connStr))
			{
				failList = new List<string>() { "Non Items" };
				return false;
			}

			// null を返す
			failList = null;
			return true;
		}
	}
}
