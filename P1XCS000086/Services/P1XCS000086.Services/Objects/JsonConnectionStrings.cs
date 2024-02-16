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
		/// スキーマ名
		/// </summary>
		[JsonProperty("SchimaName")]
		public string SchimaName {  get; set; }

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
		/// <param name="connectionString"></param>
		public void AddConnectionString(string connectionString)
		{
			// スキーマ名と接続文字列のペアを追加
			JsonConnectionStringItems.Add(SchimaName, connectionString);
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
