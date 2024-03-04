using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class IntegrMasterModel : IIntegrMasterModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public DataTable DataTable { get; private set; }
		public string SelectedMasterTable { get; private set; }
		public DataRow SelectedRow { get; private set; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 選択されたDataRowをセット
		/// </summary>
		/// <param name="dataRow">セットする選択行</param>
		public void SetSelectedRow(DataRow dataRow)
		{
			SelectedRow = dataRow;
		}

		/// <summary>
		/// DataTableをセット
		/// </summary>
		/// <param name="dataTable">セットするDataTable</param>
		public void SetDataTable(DataTable dataTable)
		{
			DataTable = dataTable;
		}
	}
}
