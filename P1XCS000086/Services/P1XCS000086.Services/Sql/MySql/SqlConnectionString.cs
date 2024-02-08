using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Sql.MySql
{
    public class SqlConnectionString : IMySqlConnectionString
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		// 接続文字列を保持する静的変数
		private static string _connectionString = string.Empty;



		// ****************************************************************************
		// Reactive Properties
		// ****************************************************************************

		/// <summary>
		/// サーバ名
		/// </summary>
		public string Server { get; private set; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string User {  get; private set; }

		/// <summary>
		/// データベース名
		/// </summary>
		public string Database {  get; private set; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password {  get; private set; }

		/// <summary>
		/// セキュリティ情報の保持
		/// </summary>
		public bool PersistSecurityInfo { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		public SqlConnectionString() { }

		/// <summary>
		/// コンストラクタ
		/// 接続文字列を生成する
		/// ※persistSecurityInfoは常に true
		/// </summary>
		/// <param name="server">サーバー名</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">接続データベース</param>
		/// <param name="password">パスワード</param>
		public SqlConnectionString(string server, string user, string database, string password = "")
			: this()
		{
			Server = server;
			User = user;
			Database = database;
			Password = password;
			PersistSecurityInfo = true;
		}

		/// <summary>
		/// コンストラクタ
		/// 接続文字列を生成する
		/// </summary>
		/// <param name="server">サーバー名</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">接続データベース</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo"></param>
		public SqlConnectionString(string server, string user, string database, string password, bool persistSecurityInfo)
			: this(server, user, database, password)
		{
			PersistSecurityInfo = persistSecurityInfo;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 接続文字列を生成する
		/// </summary>
		/// <returns>接続文字列</returns>
		public string GetConnectionString()
		{
			return _connectionString;
		}

		/// <summary>
		/// 接続文字列を生成する
		/// </summary>
		/// <param name="server">サーバー名</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">接続データベース</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo"></param>
		/// <returns>
		/// 接続文字列
		/// </returns>
		public string GenelateConnectionString(string server, string user, string database, string password, bool persistSecurityInfo)
		{
			// 接続文字列ビルダーを生成
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
			builder.Server = Server;
			builder.UserID = User;
			builder.Database = Database;
			builder.Password = Password;
			builder.PersistSecurityInfo = PersistSecurityInfo;

			// 接続文字列を生成
			string connStr = builder.ToString();

			// 生成した接続文字列を保持
			_connectionString = connStr;

			return connStr;
		}

		/// <summary>
		/// 接続文字列が設定されているか、真理値で返す
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns>
		/// 接続文字列が取得できれば true
		/// 接続文字列が取得できなければ false 
		/// </returns>
		public bool IsGetConnectionString(out string connectionString)
		{
			switch (_connectionString)
			{
				case "":
					connectionString = string.Empty;
					return false;
				default:
					connectionString = _connectionString;
					break;
			}

			return true;
		}
	}
}
