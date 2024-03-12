using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Models.CodeManageMaster.Domains;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class MasterEditorModel : IMasterEditorModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IIntegrMasterModel _integrModel;
		private IJsonConnectionStrings _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;
		private ISqlUpdate _update;
		private ISqlDelete _delete;
		private ISqlShowTables _showTables;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// テーブル名のリスト
		/// </summary>
		public List<string> TableNames
		{
			get => SetTableNames();
		}



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public MasterEditorModel() { }



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
		public void InjectModels(IIntegrMasterModel integrModel, IJsonConnectionStrings connStr, ISqlSelect select, ISqlInsert insert, ISqlUpdate update, ISqlDelete delete, ISqlShowTables showTables)
		{
			_integrModel = integrModel;
			_connStr = connStr;
			_select = select;
			_insert = insert;
			_update = update;
			_delete = delete;
			_showTables = showTables;
		}

		/// <summary>
		/// テーブル名のリスト一覧を返却
		/// </summary>
		/// <returns>テーブル名のリスト一覧</returns>
		public List<string> SetTableNames()
		{
			string databaseName = "manager";
			string connStr = _connStr.PickConnectionString(databaseName, out bool result);

			if (result)
			{
				return _showTables.ShowTables(databaseName);
			}

			return new List<string>() { "None" };
		}

		/// <summary>
		/// 選択されたデータベースの各カラム入力用フィールド
		/// </summary>
		/// <param name="databaseName">データベース名</param>
		/// <returns>入力用フィールドオブジェクト</returns>
		public List<ITableField> GetTableFields(string databaseName)
		{
			List<string> columnNames = _showTables.ShowTables(databaseName);

			string query = $"SELECT japanese FROM table_translator WHERE table_name = '{databaseName}' AND type = 'column';";
			List<string> columnNamesJp = _select.SelectedColumnToList("japanese", query);

			List<ITableField> tableFields = GenerateTableFields(columnNames, columnNamesJp).ToList();
			return tableFields;
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		private IEnumerable<ITableField> GenerateTableFields(List<string> columnNames, List<string> columnNamesJp)
		{
			int count = 0;
			foreach (string columnName in columnNames)
			{
				ITableField tableField = new TableField(columnName);
				tableField.SetColumnName(columnNamesJp[count]);

				count++;

				yield return tableField;
			}
		}
	}
}
