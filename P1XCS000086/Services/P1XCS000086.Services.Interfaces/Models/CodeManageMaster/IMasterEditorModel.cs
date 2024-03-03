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
		public void InjectModels(IJsonConnectionStrings connStr, ISqlSelect select, ISqlInsert insert, ISqlUpdate update, ISqlDelete delete, ISqlShowTables shwoTables);
	}
}
