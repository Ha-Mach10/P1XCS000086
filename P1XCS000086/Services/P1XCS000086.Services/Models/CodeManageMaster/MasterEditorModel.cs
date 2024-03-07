using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class MasterEditorModel : IMasterEditorModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IIntegrMasterModel _integrModel;
		private ITableField _tableField;
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
	}
}
