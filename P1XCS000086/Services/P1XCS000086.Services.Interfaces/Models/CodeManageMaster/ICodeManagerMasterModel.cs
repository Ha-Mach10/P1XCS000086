using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster
{
	public interface ICodeManagerMasterModel
	{
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
			ISqlShowTables showTables);

		/// <summary>
		/// テーブル名のリスト一覧を返却
		/// </summary>
		/// <returns>テーブル名のリスト一覧</returns>
		public List<string> SetTableNames();

		/// <summary>
		/// 選択されたデータベースの各カラム入力用フィールド
		/// </summary>
		/// <param name="databaseName">データベース名</param>
		/// <returns>入力用フィールドオブジェクト</returns>
		public List<ITableField> GetTableFields(string databaseName);

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
		public string TestDatabaseConnection(out bool result)
	}
}
