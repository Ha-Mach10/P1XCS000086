using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
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
		private IJsonConnectionStrings _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;
		private ISqlUpdate _update;
		private ISqlDelete _delete;
		private ISqlShowTables _showTables;



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
		/// <param name="connStr">接続文字列のモデル</param>
		/// <param name="select">SELECTクエリ用モデル</param>
		/// <param name="insert">INSERTクエリ用モデル</param>
		/// <param name="update">UPDATEクエリ用モデル</param>
		/// <param name="delete">DELETEクエリ用モデル</param>
		/// <param name="shwoTables">SHOWTABLESクエリ用モデル</param>
		public void InjectModels(IIntegrMasterModel integrModel, IJsonConnectionStrings connStr, ISqlSelect select, ISqlInsert insert, ISqlUpdate update, ISqlDelete delete, ISqlShowTables shwoTables)
		{
			_integrModel = integrModel;
			_connStr = connStr;
			_select = select;
			_insert = insert;
			_update = update;
			_delete = delete;
			_showTables = shwoTables;
		}


	}
}
