using Newtonsoft.Json;
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
		// Properties
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
	}
}
