using OpenQA.Selenium.DevTools.V125.Browser;
using P1XCS000086.Services.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace P1XCS000086.Services.Sql
{
	/// <summary>
	/// 接続文字列保持用クラス
	/// </summary>
	public class SqlConnStringParamerers
	{
		/// <summary>
		/// サーバー名
		/// </summary>
		public string ServerName { get; private set; } = string.Empty;
		/// <summary>
		/// ユーザー名
		/// </summary>
		public string UserName { get; private set; } = string.Empty;
		/// <summary>
		/// データベース名
		/// </summary>
		public string DatabaseName { get; private set; } = string.Empty;
		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; private set; } = string.Empty;
		/// <summary>
		/// 接続情報の保護
		/// </summary>
		public bool PersistSecurityInfo { get; private set; } = false;


		/// <summary>
		/// 接続文字列の各パラメータを保持する
		/// </summary>
		/// <param name="serverName"></param>
		/// <param name="userName"></param>
		/// <param name="databaseName"></param>
		/// <param name="password"></param>
		/// <param name="persistSecurityInfo"></param>
		public SqlConnStringParamerers(string serverName, string userName, string databaseName, string password, bool persistSecurityInfo)
		{
			ServerName = serverName;
			UserName = userName;
			DatabaseName = databaseName;
			Password = password;
			PersistSecurityInfo = persistSecurityInfo;
		}
	}
}
