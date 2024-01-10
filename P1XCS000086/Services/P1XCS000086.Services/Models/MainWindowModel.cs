using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Sql.MySql;

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Net.Http.Headers;
using MySqlX.XDevAPI.Relational;
using System.Reflection.Emit;
using System.Linq;
using MySqlX.XDevAPI;


namespace P1XCS000086.Services.Models
{
	public class MainWindowModel : IMainWindowModel
	{
		// Properies 
		public string Server { get; set; }
		public string User {  get; set; }
		public string Database {  get; set; }
		public string Password { get; set; }
		public bool PersistSecurityInfo { get; set; }

		public JsonConnectionStrings JsonConnString { get; private set; }

		public string ResultMessage { get; private set; }
		public string ExceptionMessage { get; private set; }


		public MainWindowModel()
		{

		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<string> LanguageComboBoxItemSetting()
		{
			string queryCommand = "SELECT language_type FROM manager_language_type;";
			List<string> list = QueryExecuteToList("language_type", queryCommand);
			return list;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="languageType"></param>
		/// <returns></returns>
		public List<string> DevelopmentComboBoxItemSetting(string languageType)
		{
			string queryCommand = $"SELECT develop_type FROM manager_develop_type WHERE script_type=(SELECT script_type FROM manager_language_type WHERE language_type='{languageType}');";
			List<string> list = QueryExecuteToList("develop_type", queryCommand);
			return list;
		}
		public List<string> UseApplicationComboBoxItemSetting()
		{
			string queryCommand = $"SELECT use_name_jp FROM manager_use_application WHERE sign='1';";
			List<string> items = QueryExecuteToList("use_name_jp", queryCommand);

			return items;
		}
		public List<string> UseApplicationSubComboBoxItemSetting()
		{
			string queryCommand = $"SELECT use_name_jp FROM manager_use_application WHERE sign='2';";
			List<string> items = QueryExecuteToList("use_name_jp", queryCommand);

			return items;
		}
		public List<string> SearchTextUseApplicationComboBoxItemSetting(string developType, string languageType)
		{
			string queryCommand = @$"SELECT DISTINCT use_applications
									 FROM manager_codes
									 WHERE develop_number
									 LIKE
									 (
										SELECT CONCAT('%', d.develop_type_code, l.language_type_code, '%')
										FROM manager_language_type AS l
										JOIN manager_develop_type AS d
										ON l.script_type = d.script_type
										WHERE l.language_type='{languageType}' AND d.develop_type='{developType}'
									 );";

			List<string> items = QueryExecuteToList("use_application", queryCommand);

			return items;
		}
		/// <summary>
		/// 「言語種別」にて変更した言語種別から対象の言語で作成された号番を取得
		/// </summary>
		/// <param name="languageType"></param>
		/// <returns></returns>
		public DataTable CodeManagerDataGridItemSetting(string languageType)
		{
			// SELECTクエリ実行用のオブジェクトを生成
			ISqlSelect selectExecute = GetConnectedSqlSelect();

			// 号番検索用に「type_code」を「language_type」から取得
			string queryCommand = $"SELECT language_type_code FROM manager_language_type WHERE language_type='{languageType}'";
			// 「type_code」を格納するためのDataTableを用意し、SELECTクエリを実行
			DataTable typeCodeDt = selectExecute.Select(queryCommand);
			// 「type_code」を取得
			string typeCode = typeCodeDt.Rows[0][0].ToString();

			// クエリを再生成
			queryCommand = $"SELECT * FROM manager_codes WHERE develop_number LIKE '%{typeCode}%';";
			DataTable dt = selectExecute.Select(queryCommand);
			dt = CodeManagerColumnHeaderTrancelate(dt);

			return dt;
		}
		/// <summary>
		/// 「言語種別」にて変更した言語種別から対象の言語で作成された号番を取得
		/// </summary>
		/// <param name="developType"></param>
		/// <param name="languageType"></param>
		/// <returns></returns>
		public DataTable CodeManagerDataGridItemSetting(string developType, string languageType)
		{
			// SELECTクエリ実行用のオブジェクトを生成
			ISqlSelect selectExecute = GetConnectedSqlSelect();

			// クエリを作成
			string queryCommand = @$"SELECT *
									 FROM manager_codes
									 WHERE develop_number
									 LIKE
									 (
										SELECT CONCAT('%', d.develop_type_code, l.language_type_code, '%')
										FROM manager_language_type AS l
										JOIN manager_develop_type AS d
										ON l.script_type = d.script_type
										WHERE l.language_type='{languageType}' AND d.develop_type='{developType}'
									 );";

			// クエリからDataTableを取得
			DataTable dt = selectExecute.Select(queryCommand);

			dt = CodeManagerColumnHeaderTrancelate(dt);

			return dt;
		}
		/// <summary>
		/// DataGridへ表示するDataTableのヘッダ名（ColumnName）を日本語へ変換する
		/// </summary>
		/// <param name="dataTable">変換元のDataTable</param>
		/// <returns>ヘッダ変換後のDataTable</returns>
		public DataTable CodeManagerColumnHeaderTrancelate(DataTable dataTable)
		{
			string queryCommand = $"SELECT japanese FROM table_translator WHERE table_name='manager_codes' AND type='column';";
			List<string> columnHeaders = QueryExecuteToList("japanese", queryCommand);
			int count = 0;
			foreach (string columnHeader in columnHeaders)
			{
				dataTable.Columns[count].ColumnName = columnHeader;
				count++;
			}

			return dataTable;
		}

		public string RegistCodeNumberComboBoxItemSelect(string selectedValue)
		{
			string queryCommand = $"SELECT use_name_en FROM manager_use_application WHERE use_name_jp='{selectedValue}';";
			string getValue = GetSelectItem(selectedValue, queryCommand);

			return getValue;
		}
		public string CodeNumberClassification(string developType, string languageType)
		{
			// SELECTクエリ実行用のオブジェクトを生成
			ISqlSelect selectExecute = GetConnectedSqlSelect();

			// クエリを作成
			string queryCommand = @$"SELECT CONCAT(d.develop_type_code, l.language_type_code)
									 FROM manager_language_type AS l
									 JOIN manager_develop_type AS d
									 ON l.script_type = d.script_type
									 WHERE l.language_type='{languageType}' AND d.develop_type='{developType}';";
			string columnName = "CONCAT(d.develop_type_code, l.language_type_code)";
			string classificationString = GetSelectItem(columnName, queryCommand);

			return classificationString;
		}
		public string GetProjectDirectry(string languageType)
		{
			ISqlSelect selectExecute = GetConnectedSqlSelect();

			string queryCommand = @$"SELECT language_directry
									 FROM project_language_directry
									 WHERE language_type=
									 (
										SELECT language_type_code
										FROM manager_language_type
										WHERE language_type='{languageType}'
									 );";
			string directryPath = GetSelectItem("language_directry", queryCommand);

			return directryPath;
		}
		private ISqlSelect GetConnectedSqlSelect()
		{
			string connStr = ConnectionString();
			ISqlSelect slectedExecute = new SqlSelect(connStr);

			return slectedExecute;
		}
		/// <summary>
		/// クエリを実行し、取得した列からただ１つの項目を返す
		/// ※取得される項目がただ１つのみになるようクエリを作成すること
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="queryCommand"></param>
		/// <returns></returns>
		public string GetSelectItem(string columnName, string queryCommand)
		{
			// SELECTクエリを実行し、結果をDataTableへ格納
			string connStr = ConnectionString();

			// SELECTクエリ実行用のクラスをインターフェース経由で生成
			ISqlSelect selectExecute = new SqlSelect(connStr, queryCommand);
			DataTable dt = selectExecute.Select();

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
		/// ※取得される列は１列になるようクエリを作成すること
		/// </summary>
		/// <param name="command">クエリ</param>
		/// <returns></returns>
		private List<string> QueryExecuteToList(string columnName, string queryCommand)
		{
			// 接続文字列取得
			string connStr = ConnectionString();

			// 接続文字列が空の場合、「Non Items」の文字列のみ格納したリストを返す
			if (connStr == string.Empty)
			{
				return new List<string>() { "Non Items" };
			}

			// SELECTクエリ実行用のクラスをインターフェース経由で生成
			ISqlSelect selectExecute = new SqlSelect(connStr, queryCommand);
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
		public bool RegistExecute(List<string> values)
		{
			// カラム名のリストを生成
			List<string> columns = new List<string>()
			{
				"develop_number",
				"develop_name",
				"code_name",
				"create_date",
				"use_applications",
				"version",
				"revision_date", "old_number", "new_number", "inheritence_number",
				"explanation", "summary"
			};

			// INSERT用のカラム列を生成
			string columnsStr = string.Join(", ", columns);
			// パラメータクエリ用の値列を生成(SQLインジェクション対策)
            string valueStr = $"@{string.Join(", @", columns)}";

			// クエリ文字列を生成
            string queryCommand = @$"INSERT INTO manager_codes({columnsStr}) VALUES({valueStr});";

			// 接続文字列を生成
			string connStr = ConnectionString();
			ISqlInsert insertExecute = new SqlInsert(connStr);
			// INSERTクエリを実行
			bool result = insertExecute.Insert(queryCommand, columns, values);
			
			// 結果文字列または例外文字列に値がセットされている場合
			if (insertExecute.ResultMessage != "" || insertExecute.ExceptionMessage != "")
			{
				// メッセージを取得
				ResultMessage = insertExecute.ResultMessage;
				ExceptionMessage = insertExecute.ExceptionMessage;
			}

			return result;
		}


		/// <summary>
		/// 接続文字列をJSONシリアル化
		/// </summary>
		/// <param name="server">サーバー</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">接続データベース</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo"></param>
		public void JsonSerialize(string server, string user, string database, string password, bool persistSecurityInfo)
		{
			JsonExtention jsonExtention = new JsonExtention();

			// フォルダ・ファイル存在チェック。存在しなければ生成
			jsonExtention.PathCheckAndGenerate();

			// SQL接続文字列をJSONシリアライズ用クラスの各プロパティへ設定
			JsonConnectionStrings connStrings = new JsonConnectionStrings();
			connStrings.Server = server;
			connStrings.User = user;
			connStrings.DatabaseName = database;
			connStrings.Password = password;
			connStrings.PersistSecurityInfo = persistSecurityInfo;

			// SQLの接続文字列をJSONファイルへシリアライズ化
			string jsonFilePath = JsonExtention.JsonFilePath;
			jsonExtention.SerializeJson(connStrings, jsonFilePath, false);
		}
		/// <summary>
		/// SQLの接続テスト用メソッド
		/// </summary>
		/// <returns>接続の成功/失敗</returns>
		public bool SqlConnection()
		{
			string connStr = ConnectionString();
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
		/// <summary>
		/// SQLの接続文字列を生成し返すメソッド
		/// </summary>
		/// <returns>接続文字列</returns>
		public string ConnectionString()
		{
			string connStr = string.Empty;

			IJsonConnectionStrings jsonConnString = JsonDeserialize();
			MySqlStringBuilder builder = new MySqlStringBuilder();
			if (jsonConnString is not null)
			{
				builder.Server = jsonConnString.Server;
				builder.User = jsonConnString.User;
				builder.Database = jsonConnString.DatabaseName;
				builder.Password = jsonConnString.Password;
				builder.PersistSecurityInfo = jsonConnString.PersistSecurityInfo;
				connStr = builder.GetConnectionString();
			}
			else
			{
                builder.Server = Server;
                builder.User = User;
                builder.Database = Database;
                builder.Password = Password;
                builder.PersistSecurityInfo = PersistSecurityInfo;
                connStr = builder.GetConnectionString();
            }

			return connStr;
		}
		/// <summary>
		/// JSONファイルからSQL接続文字列を復号
		/// </summary>
		public IJsonConnectionStrings JsonDeserialize()
		{
			JsonExtention jsonExtention = new JsonExtention();

			if (jsonExtention is null) { return null; }
			// ファイルが存在していなければ生成し、処理を抜ける
			if (!jsonExtention.PathCheckAndGenerate()) { return null; }

			// JSONファイルからSQL接続文字列を復号し、プロパティにセット
			string jsonFilePath = JsonExtention.JsonFilePath;
			IJsonConnectionStrings jsonConnString = jsonExtention.DeserializeJson<JsonConnectionStrings>(jsonFilePath);
			if (jsonConnString is null || !jsonConnString.IsPropertiesExists()) { return null; }

			return jsonConnString;
		}
	}
}
