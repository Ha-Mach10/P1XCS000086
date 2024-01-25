using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Sql.MySql;

using System;
using System.Text;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Http.Headers;
using System.Reflection.Emit;

using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using MySqlX.XDevAPI;
using System.IO;


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
		public List<string> ViewUseApplicationComboBoxItemSetting()
		{
			// SELECTクエリ実行用のオブジェクトを生成
			ISqlSelect selectExecute = new SqlSelect(ConnectionString());

			// 号番検索用に「type_code」を「language_type」から取得
			string queryCommand = $"SELECT DISTINCT use_applications FROM manager_codes;";

			// 「type_code」から「use_applications」カラムをリストで取得
			List<string> useApps = selectExecute.Select(queryCommand).AsEnumerable().Select(x => x["use_applications"].ToString()).ToList();

			// IEnumerable<IEnumerable<string>>の型で整形
			var splitedUseApps = useApps.Select(x => ToWords(x));

			List<string> items = new();
			// useApplication
			foreach (var item in splitedUseApps)
			{
				switch (item.Count())
				{
					case 1:
						// ただ一つの値を取得
						string useNameEn = item.Select(x => x).First();
						queryCommand = $"SELECT use_name_jp FROM manager_use_application WHERE use_name_en = '{useNameEn}';";
						break;
					case >= 2:
						// IEnumerable<string>型をJoinで文字列に整形
						string joinedUseNameEn = string.Join(", ", item.Select(x => $"\'{x}\'"));
						queryCommand = $"SELECT use_name_jp FROM manager_use_application WHERE use_name_en IN ({joinedUseNameEn});";
						break;
					default:
						break;
				}

				// 「use_name_jp」カラムから取得したリストを半角スペースで区切り、取得（例：「学習用 汎用機能」）
				string useAppNameJp = string.Join(" ", QueryExecuteToList("use_name_jp", queryCommand).Select(x => x));
				items.Add(useAppNameJp);
			}

			return items;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="developType"></param>
		/// <param name="languageType"></param>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <returns></returns>
		public List<string> ShowTableItems()
		{
			string connStr = ConnectionString();

			string queryCommand = $"SELECT `japanese`, `table_name` FROM table_translator WHERE `type` = 'table';";
			ISqlSelect selectExecute = new SqlSelect(connStr);
			DataTable dt = selectExecute.Select(queryCommand);

			// ISqlShowTables showTables = new SqlShowTables(connStr);
			List<string> tables = dt.AsEnumerable().Select(x => x["japanese"].ToString()).ToList();

			return tables;
		}
		public List<string> GetInPutFieldColumns(string tableName)
		{
			string queryCommand = @$"SELECT japanese
									 FROM table_translator
									 WHERE `table_name` = (
										 SELECT `table_name`
										 FROM table_translator
										 WHERE `type` = 'table'
										 AND japanese = '{tableName}'
									 )
									 AND `type` = 'column';";

			List<string> columnNames = QueryExecuteToList("japanese", queryCommand);
			return columnNames;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="langValue"></param>
		/// <param name="useAppValue"></param>
		/// <returns></returns>
		public DataTable GetViewDataTable(string langValue = "", string useAppValue = "")
		{
			// 
			ISqlSelect selectExecute = new SqlSelect(ConnectionString());

			// 
			string queryCommand = $"SELECT * FROM manager_codes **;";


			StringBuilder sb = new StringBuilder();

			// 引数が全て空の時
			if (langValue == "" && useAppValue == "")
			{
				queryCommand = queryCommand.Replace(" **", "");
			}
			if (langValue != "")
			{
				// 号番検索用に「type_code」を「language_type」から取得
				string queryCommandSub = $"SELECT language_type_code FROM manager_language_type WHERE language_type='{langValue}'";
				// 「type_code」を取得
				string typeCode = QueryExecuteToList("language_type_code", queryCommandSub).Select(x => x).First().ToString();

				sb.Append($"WHERE develop_number LIKE '%{typeCode}%'");
			}
			if (useAppValue != "")
			{
				string spliteItem = string.Join(", ", useAppValue.Split(' ').Select(x => $"\'{x}\'"));
				string queryCommandSub = string.Empty;
				if (useAppValue.Split(' ').Count() > 1)
				{
					queryCommandSub = $"SELECT use_name_en FROM manager_use_application WHERE use_name_jp IN ({spliteItem});";
				}
				else
				{
					queryCommandSub = $"SELECT use_name_en FROM manager_use_application WHERE use_name_jp = '{useAppValue}';";
				}
				
				string useApp = string.Join("", QueryExecuteToList("use_name_en", queryCommandSub).Select(x => x));
				string replaceUseApp = $"WHERE use_applications = '{useApp}'";

				if (langValue != "")
				{
					replaceUseApp = $" AND use_applications = '{useApp}'";
				}

				sb.Append(replaceUseApp);
			}

			string replaceStr = sb.ToString();
			queryCommand = queryCommand.Replace("**", replaceStr);

			DataTable dt = selectExecute.Select(queryCommand);
			dt = CodeManagerColumnHeaderTrancelate(dt);

			return dt;
		}
		/// <summary>
		/// 「言語種別」にて変更した言語種別から対象の言語で作成された号番を取得
		/// </summary>
		/// <param name="languageType"></param>
		/// <returns></returns>
		public DataTable CodeManagerDataGridItemSetting(string languageType)
		{
			// SELECTクエリ実行用のオブジェクトを生成
			ISqlSelect selectExecute = new SqlSelect(ConnectionString());

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
			ISqlSelect selectExecute = new SqlSelect(ConnectionString());

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
		public DataTable MasterTableData(string tableName)
		{
			string lastTableName = string.Empty;

			Encoding encoding = Encoding.GetEncoding("Shift-JIS");
			// 全角文字の場合、falseを返す
			bool isSameLength = tableName.Length != encoding.GetByteCount(tableName);
			if (isSameLength)
			{
				// 引数「tableName」が2バイト文字の場合
				string subQueryCommand = $"SELECT `table_name` FROM table_translator WHERE japanese = '{tableName}';";
				lastTableName = GetSelectItem("table_name", subQueryCommand);
			}

			string queryCommand = $"SELECT * FROM {lastTableName};";

			ISqlSelect selectExecute = new SqlSelect(ConnectionString());
			DataTable dt = selectExecute.Select(queryCommand);

			return dt;
		}
		public string RegistCodeNumberComboBoxItemSelect(string selectedValue)
		{
			string queryCommand = $"SELECT use_name_en FROM manager_use_application WHERE use_name_jp='{selectedValue}';";
			string getValue = GetSelectItem(selectedValue, queryCommand);

			return getValue;
		}
		public string CodeNumberClassification(string developType, string languageType)
		{
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
		
		/// <summary>
		/// 
		/// 参考サイト：https://qiita.com/mytk-k/items/4a338965cb7bc3d584ec
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<string> ToWords(string source)
		{
			// 作業用変数の宣言           
			var wordbreakIndex = 0;          // 現在の単語の始まりのインデックス
			var currentWordLength = 0;       // 現在の単語の文字数
			var current = '\0';              // ループの中で現在参照している文字
			var isLowerBefore = false;       // 一つ前の文字が小文字だったかどうか
			var isUpperCurrent = false;      // 現在の文字が大文字かどうか

			for (var i = 0; i < source.Length; i++)
			{
				current = source[i];
				isUpperCurrent = char.IsUpper(current);

				if (isLowerBefore && isUpperCurrent)
				{
					// 小文字から大文字に切り替わった時に単語を切り出す。
					yield return source.Substring(wordbreakIndex, currentWordLength);
					wordbreakIndex = i;
					currentWordLength = 0;
				}

				currentWordLength++;
				isLowerBefore = char.IsLower(current);
			}

			// 最後の単語の返却漏れがないように
			yield return source.Substring(wordbreakIndex, source.Length - wordbreakIndex);
		}
	}
}
