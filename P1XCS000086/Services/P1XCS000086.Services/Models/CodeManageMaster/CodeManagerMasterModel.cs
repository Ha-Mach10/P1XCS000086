using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.IO;
using MySql.Data.MySqlClient;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class CodeManagerMasterModel : ICodeManagerMasterModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private ISqlConnectionTest _connectionTest;
		private IJsonExtention _jsonExtention;
		private IJsonConnectionItem _jsonConnItem;
		// private IJsonConnectionStrings _jsonConnStr;

		private IJsonConnectionStrings _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;
		private ISqlUpdate _update;
		private ISqlDelete _delete;
		private ISqlShowTables _showTables;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// サーバー名
		/// </summary>
		public string Server { get; private set; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string User { get; private set; }

		/// <summary>
		/// データベース
		/// </summary>
		public string Database { get; private set; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; private set; }

		/// <summary>
		/// 接続状態の保持
		/// </summary>
		public bool PersistSecurityInfo { get; private set; }

		/// <summary>
		/// 接続文字列の取得成否判定用
		/// </summary>
		public bool IsSetConnStr { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManagerMasterModel()
		{
			// 初期値設定
			IsSetConnStr = false;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたモデルを注入
		/// </summary>
		/// <param name="connectionTest"></param>
		/// <param name="jsonExtention"></param>
		/// <param name="jsonConnItem"></param>
		/// <param name="connStr"></param>
		/// <param name="select"></param>
		/// <param name="insert"></param>
		/// <param name="update"></param>
		/// <param name="delete"></param>
		/// <param name="showTables"></param>
		public void InjectModels(
			ISqlConnectionTest connectionTest,
			IJsonExtention jsonExtention,
			IJsonConnectionItem jsonConnItem,
			IJsonConnectionStrings connStr,
			ISqlSelect select,
			ISqlInsert insert,
			ISqlUpdate update,
			ISqlDelete delete,
			ISqlShowTables showTables)
		{
			_connectionTest = connectionTest;
			_jsonExtention = jsonExtention;
			_jsonConnItem = jsonConnItem;
			// _jsonConnStr = jsonConnStr;

			_connStr = connStr;
			_select = select;
			_insert = insert;
			_update = update;
			_delete = delete;
			_showTables = showTables;
		}

		/// <summary>
		/// テーブル名のリスト一覧を返却
		/// </summary>
		/// <returns>テーブル名のリスト一覧</returns>
		public List<string> SetTableNames()
		{
			string databaseName = "manager";
			string connStr = _connStr.PickConnectionString(databaseName, out bool result);

			if (result)
			{
				return _showTables.ShowTables(databaseName);
			}

			return new List<string>() { "None" };
		}

		/// <summary>
		/// 選択されたデータベースの各カラム入力用フィールド
		/// </summary>
		/// <param name="databaseName">データベース名</param>
		/// <returns>入力用フィールドオブジェクト</returns>
		public List<TableField> GetTableFields(string databaseName)
		{
			List<string> columnNames = _showTables.ShowTables(databaseName);

			string query = $"SELECT japanese FROM table_translator WHERE table_name = '{databaseName}' AND type = 'column';";
			List<string> columnNamesJp = _select.SelectedColumnToList("japanese", query);

			List<ITableField> tableFields = GenerateTableFields(columnNames, columnNamesJp).ToList();
			return tableFields;
		}

		/// <summary>
		/// 接続文字列をJSONファイルへ追加
		/// </summary>
		/// <param name="server">サーバー名</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">データベース名</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo">接続状態保持有無</param>
		public void RegistConnectionString(string server, string user, string database, string password, bool persistSecurityInfo)
		{
			// 接続文字列のプロパティを設定
			Server = server;
			User = user;
			Database = database;
			Password = password;
			PersistSecurityInfo = persistSecurityInfo;

			// JSON書き込み用の接続文字列を登録
			_connStr.Server = Server;
			_connStr.User = User;
			_connStr.DatabaseName = Database;
			_connStr.Password = Password;
			_connStr.PersistSecurityInfo = PersistSecurityInfo;

			// JSON書き込み用の接続文字列のリストへ登録
			_jsonConnItem.ConnectionStrings.Add(_connStr);

			// 接続文字列用のJSONファイルパス
			string filePath = _jsonExtention.JsonSqlFilePath;


			// 接続文字列が取得出来るか否かの判定
			_connStr.PickConnectionString(Database, out bool result);
			// ファイルの存在チェックと接続文字列が取得可能か確認
			// 存在または取得可能でなければファイルを生成し、接続文字列を追加
			if (_jsonExtention.PathCheckAndGenerate() == false || result == false)
			{
				// 接続文字列をJSONファイルへ書き込み(直列化)
				_jsonExtention.SerializeJson(_jsonConnItem, filePath, true);
				IsSetConnStr = true;
			}
			else
			{
				// すでに同じKeyで追加されている接続文字列のペアを削除
				_connStr.RemoveConnectionString(Database);
				// 設定したプロパティをディクショナリへ追加
				_connStr.AddConnectionString();
				IsSetConnStr = false;
			}
		}

		/// <summary>
		/// データベースへの接続テスト
		/// </summary>
		/// <param name="result">接続の成否</param>
		/// <returns>接続成否のメッセージ</returns>
		public string TestDatabaseConnection(out bool result)
		{
			// 接続文字列の取得
			string connStr = GetConnectionString();

			// 接続テスト実行
			if (_connectionTest.SqlConnection(connStr))
			{
				result = true;
				return "接続成功";
			}
			else
			{
				result = false;
				return "接続に失敗しました";
			}
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// 指定の接続文字列の各パラメータをプロパティにセット
		/// </summary>
		private void SetConnectionStringProperties()
		{
			// 「manager」データベースの接続文字列を検索し、取得
			string connStr = GetConnectionString();

			// 取得した接続文字列を解析し、各パラメータのプロパティを取得
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connStr);
			Server = builder.Server;
			User = builder.UserID;
			Database = builder.Database;
			Password = builder.Password;
			PersistSecurityInfo = builder.PersistSecurityInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private string GetConnectionString()
		{
			// 「manager」データベースの接続文字列を検索し、取得
			string connStr = IJsonConnectionStrings.JsonConnectionStringItems
												   .Where(x => x.Key == "manager")
												   .First()
												   .Value;
			return connStr;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnNames"></param>
		/// <param name="columnNamesJp"></param>
		/// <returns></returns>
		private IEnumerable<ITableField> GenerateTableFields(List<string> columnNames, List<string> columnNamesJp)
		{
			int count = 0;
			foreach (string columnName in columnNames)
			{
				ITableField tableField = new TableField(columnName);
				tableField.SetColumnName(columnNamesJp[count]);

				count++;

				yield return tableField;
			}
		}
	}
}
