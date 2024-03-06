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

		/// <summary>
		/// 選択されたテーブル名
		/// </summary>
		public string SelectedTableName { get; private set; }
		/// <summary>
		/// 表示されるDataTable
		/// </summary>
		public DataTable DataTable { get; private set; }
		/// <summary>
		/// 選択行
		/// </summary>
		public DataRow SelectedRow { get; private set; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 選択されたテーブル名をセット
		/// </summary>
		/// <param name="selectedTableName">選択されたテーブル名</param>
		public void SetSelectedTableName(string selectedTableName)
		{
			SelectedTableName = selectedTableName;
		}

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
