using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster
{
	public interface IIntegrMasterModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public DataTable DataTable { get; }
		public string SelectedMasterTable { get; }
		public DataRow SelectedRow { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 選択されたDataRowをセット
		/// </summary>
		/// <param name="dataRow">セットする選択行</param>
		public void SetSelectedRow(DataRow dataRow);

		/// <summary>
		/// DataTableをセット
		/// </summary>
		/// <param name="dataTable">セットするDataTable</param>
		public void SetDataTable(DataTable dataTable);
	}
}
