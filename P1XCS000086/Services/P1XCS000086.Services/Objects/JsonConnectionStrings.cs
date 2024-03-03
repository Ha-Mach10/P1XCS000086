using Newtonsoft.Json;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using P1XCS000086.Services.Interfaces.Objects;
using System.Linq;
using System.Text.RegularExpressions;

namespace P1XCS000086.Services.Objects
{
    [JsonObject("JsonConnectionStrings")]
	public class JsonConnectionStrings : IJsonConnectionStrings
	{
		// ****************************************************************************
		// Fields
		// 不要かも
		// ****************************************************************************

		private string _jsonConnectionString = string.Empty;



		// ****************************************************************************
		// Properties
		// 不要かも
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		public string JsonConnectionString
		{
			get => _jsonConnectionString;
			set
			{
				GenerateConnectionString();
			}
		}


		// ****************************************************************************
		// Static Properties
		// ****************************************************************************

		/// <summary>
		/// スキーマ名と接続文字列のペアのディクショナリ
		/// </summary>
		public static Dictionary<string, string> JsonConnectionStringItems { get; private set; } = new Dictionary<string, string>();



		// ****************************************************************************
		// Json Properties
		// ****************************************************************************

		/// <summary>
		/// サーバ名
		/// </summary>
		[JsonProperty("Server")]
		public string Server {  get; set; }

		/// <summary>
		/// ユーザ名
		/// </summary>
		[JsonProperty("User")]
		public string User {  get; set; }

		/// <summary>
		/// データベース名
		/// </summary>
		[JsonProperty("DatabaseName")]
		public string DatabaseName {  get; set; }

		/// <summary>
		/// パスワード
		/// </summary>
		[JsonProperty("Password")]
		public string Password { get; set; }

		/// <summary>
		/// セキュリティ情報の保持
		/// </summary>
		[JsonProperty("PersistSecurityInfo")]
		public bool PersistSecurityInfo { get; set; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// プロパティが設定されているか否かの判定
		/// </summary>
		/// <returns>プロパティが全て設定されている場合 true。それ以外の場合 false。</returns>
		public bool IsPropertiesExists()
		{
			if (Server is not null &&
				User is not null &&
				DatabaseName is not null &&
				Password is not null &&
				PersistSecurityInfo is not false)
			{
				return true;
			}

			var aa = Server.Split('+');
			var regex = Regex.Matches(aa.ToString(), @"a");
			int r = regex.Count;

			return false;
		}

		/// <summary>
		/// 接続文字列の追加
		/// </summary>
		public void AddConnectionString()
		{
			// スキーマ名と接続文字列のペアを追加
			JsonConnectionStringItems.Add(DatabaseName, GenerateConnectionString());
		}
		/// <summary>
		/// 接続文字列の削除
		/// </summary>
		/// <param name="schemaNameKey">削除するデータベース名</param>
		public void RemoveConnectionString(string schemaNameKey)
		{
			JsonConnectionStringItems.Remove(schemaNameKey);
		}

		/// <summary>
		/// データベース名と接続文字列のディクショナリから接続文字列を取得
		/// </summary>
		/// <param name="schemaNameKey">目的のデータベース名</param>
		/// <param name="result">取得可否</param>
		/// <returns>接続文字列を返す。例外の場合は空文字列を返す。</returns>
		public string PickConnectionString(string schemaNameKey, out bool result)
		{
			try
			{
				// データベース名から接続文字列を取得
				string connStr = JsonConnectionStringItems.Where(x => x.Key == schemaNameKey).First().Value;
				result = true;

				return connStr;
			}
			catch (InvalidOperationException ex)
			{
				// 例外処理
				result = false;
				return string.Empty;
			}
		}



		// ****************************************************************************
		// Private Methods
		// 不要かも
		// ****************************************************************************

		private string GenerateConnectionString()
		{
			// プロパティが設定されている場合、値を返す
			if (IsPropertiesExists())
			{
				MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
				builder.Server = Server;
				builder.UserID = User;
				builder.Database = DatabaseName;
				builder.Password = Password;
				builder.PersistSecurityInfo = PersistSecurityInfo;

				return builder.ConnectionString;
			}

			return string.Empty;
		}
	}
}
