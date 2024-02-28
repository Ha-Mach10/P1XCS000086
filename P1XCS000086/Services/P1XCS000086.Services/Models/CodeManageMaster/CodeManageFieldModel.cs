using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.IO;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class CodeManageFieldModel : ICodeManageFieldModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private ISqlSelect _select;
		private ISqlShowTables _showTables;
		private ISqlConnectionTest _connectionTest;
		private IJsonExtention _jsonExtention;
		private IJsonConnectionItem _jsonConnItem;
		private IJsonConnectionStrings _jsonConnStr;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// サーバー名
		/// </summary>
		public string Server {  get; private set; }

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
		public bool IsSetConnStr {  get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManageFieldModel()
		{
			// 初期値設定
			IsSetConnStr = false;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたロジックモデル群を注入
		/// </summary>
		/// <param name="select">SELECTクエリ実行用</param>
		/// <param name="showTables">SHOWTABLEクエリ実行用</param>
		/// <param name="connectionTest">接続テスト用</param>
		/// <param name="jsonExtention">JSON化用クラス</param>
		/// <param name="jsonConnItem">JSON接続文字列リスト用</param>
		/// <param name="jsonConnStr">JSON接続文字列</param>
		public void InjectModels
			(ISqlSelect select, ISqlShowTables showTables, ISqlConnectionTest connectionTest, IJsonExtention jsonExtention, IJsonConnectionItem jsonConnItem, IJsonConnectionStrings jsonConnStr)
		{
			// 
			_select = select;
			_showTables = showTables;
			_connectionTest = connectionTest;
			_jsonExtention = jsonExtention;
			_jsonConnItem = jsonConnItem;
			_jsonConnStr = jsonConnStr;
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
			_jsonConnStr.Server = Server;
			_jsonConnStr.User = User;
			_jsonConnStr.DatabaseName = Database;
			_jsonConnStr.Password = Password;
			_jsonConnStr.PersistSecurityInfo = PersistSecurityInfo;

			// JSON書き込み用の接続文字列のリストへ登録
			_jsonConnItem.ConnectionStrings.Add(_jsonConnStr);

			// 接続文字列用のJSONファイルパス
			string filePath = _jsonExtention.JsonSqlFilePath;


			// 接続文字列が取得出来るか否かの判定
			_jsonConnStr.PickConnectionString(Database, out bool result);
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
				// 設定したプロパティをディクショナリへ追加
				_jsonConnStr.AddConnectionString();
				IsSetConnStr = false;
            }
        }

		/// <summary>
		/// データベースへの接続テスト
		/// </summary>
		/// <returns>接続成否のメッセージ</returns>
		public string TestDatabaseConnection()
		{
			// 接続文字列の取得
			string connStr = GetConnectionString();

			// 接続テスト実行
			if (_connectionTest.SqlConnection(connStr))
			{
				return "接続成功";
			}
			else
			{
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
	}
}
