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
		/// <param name="connStr"></param>
		/// <param name="select"></param>
		/// <param name="insert"></param>
		/// <param name="update"></param>
		/// <param name="delete"></param>
		/// <param name="shwoTables"></param>
		public void InjectModels(IJsonConnectionStrings connStr, ISqlSelect select, ISqlInsert insert, ISqlUpdate update, ISqlDelete delete, ISqlShowTables shwoTables)
		{
			_connStr = connStr;
			_select = select;
			_insert = insert;
			_update = update;
			_delete = delete;
			_showTables = shwoTables;
		}


	}
}
