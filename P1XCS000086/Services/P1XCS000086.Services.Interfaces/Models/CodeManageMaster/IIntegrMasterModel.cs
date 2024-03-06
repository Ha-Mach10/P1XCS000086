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

		/// <summary>
		/// 選択されたテーブル名
		/// </summary>
		public string SelectedTableName { get; }
		/// <summary>
		/// 表示されるDataTable
		/// </summary>
		public DataTable DataTable { get; }
		/// <summary>
		/// 選択行
		/// </summary>
		public DataRow SelectedRow { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 選択されたテーブル名をセット
		/// </summary>
		/// <param name="selectedTableName">選択されたテーブル名</param>
		public void SetSelectedTableName(string selectedTableName);

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
