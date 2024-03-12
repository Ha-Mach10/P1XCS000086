using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster
{
	public interface IMasterEditorModel
	{
		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// テーブル名のリスト
		/// </summary>
		public List<string> TableNames { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたモデルを注入
		/// </summary>
		/// <param name="integrModel">共通モデル</param>
		/// <param name="tableField">データベースフィールド編集用モデル</param>
		/// <param name="connStr">接続文字列のモデル</param>
		/// <param name="select">SELECTクエリ用モデル</param>
		/// <param name="insert">INSERTクエリ用モデル</param>
		/// <param name="update">UPDATEクエリ用モデル</param>
		/// <param name="delete">DELETEクエリ用モデル</param>
		/// <param name="shwoTables">SHOWTABLESクエリ用モデル</param>
		public void InjectModels(IIntegrMasterModel integrModel, IJsonConnectionStrings connStr, ISqlSelect select, ISqlInsert insert, ISqlUpdate update, ISqlDelete delete, ISqlShowTables shwoTables);

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
	}
}
