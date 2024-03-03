using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster
{
	public interface ICodeManageFieldModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// サーバー名
		/// </summary>
		public string Server { get; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string User { get; }

		/// <summary>
		/// データベース
		/// </summary>
		public string Database { get; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// 接続状態の保持
		/// </summary>
		public bool PersistSecurityInfo { get; }

		/// <summary>
		/// 接続文字列の取得成否判定用
		/// </summary>
		public bool IsSetConnStr { get; }



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
			(ISqlSelect select, ISqlShowTables showTables, ISqlConnectionTest connectionTest, IJsonExtention jsonExtention, IJsonConnectionItem jsonConnItem, IJsonConnectionStrings jsonConnStr);

		/// <summary>
		/// 接続文字列をJSONファイルへ追加
		/// </summary>
		/// <param name="server">サーバー名</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">データベース名</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo">接続状態保持有無</param>
		public void RegistConnectionString(string server, string user, string database, string password, bool persistSecurityInfo);

		/// <summary>
		/// データベースへの接続テスト
		/// </summary>
		/// <param name="result">接続の成否</param>
		/// <returns>接続成否のメッセージ</returns>
		public string TestDatabaseConnection(out bool result);
	}
}
