using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Objects
{
    public interface IJsonConnectionStrings
    {
		// ****************************************************************************
		// Static Properties
		// ****************************************************************************

		/// <summary>
		/// スキーマ名と接続文字列のペアのディクショナリ
		/// </summary>
		public static Dictionary<string, string> JsonConnectionStringItems { get; }



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// サーバ名
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// ユーザ名
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// データベース名
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// セキュリティ情報の保持
		/// </summary>
		public bool PersistSecurityInfo { get; set; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// プロパティが設定されているか否かの判定
		/// </summary>
		/// <returns>プロパティが全て設定されている場合 true。それ以外の場合 false。</returns>
		public bool IsPropertiesExists();

		/// <summary>
		/// 接続文字列の追加
		/// </summary>
		public void AddConnectionString();
		/// <summary>
		/// 接続文字列の削除
		/// </summary>
		/// <param name="schemaNameKey">削除するデータベース名</param>
		public void RemoveConnectionString(string schemaNameKey);

		/// <summary>
		/// データベース名と接続文字列のディクショナリから接続文字列を取得
		/// </summary>
		/// <param name="schemaNameKey">目的のデータベース名</param>
		/// <param name="result">取得可否</param>
		/// <returns>接続文字列を返す。例外の場合は空文字列を返す。</returns>
		public string PickConnectionString(string schemaNameKey, out bool result);
	}
}
