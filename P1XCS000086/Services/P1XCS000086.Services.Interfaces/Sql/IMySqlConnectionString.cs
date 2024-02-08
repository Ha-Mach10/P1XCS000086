using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
    public interface IMySqlConnectionString
    {
		// ****************************************************************************
		// Reactive Properties
		// ****************************************************************************

		/// <summary>
		/// サーバ名
		/// </summary>
		public string Server { get; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string User { get; }

		/// <summary>
		/// データベース名
		/// </summary>
		public string Database { get; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// セキュリティ情報の保持
		/// </summary>
		public bool PersistSecurityInfo { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 接続文字列を生成する
		/// </summary>
		/// <returns>接続文字列</returns>
		public string GetConnectionString();

		/// <summary>
		/// 接続文字列を生成する
		/// </summary>
		/// <param name="server">サーバー名</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">接続データベース</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo"></param>
		/// <returns>接続文字列</returns>
		public string GenelateConnectionString(string server, string user, string database, string password, bool persistSecurityInfo);

		/// <summary>
		/// 接続文字列が設定されているか、真理値で返す
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns>
		/// 接続文字列が取得できれば true
		/// 接続文字列が取得できなければ false 
		/// </returns>
		public bool IsGetConnectionString(out string connectionString);
    }
}
