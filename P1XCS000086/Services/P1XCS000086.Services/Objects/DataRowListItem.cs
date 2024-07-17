using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Objects
{
	internal class DataRowListItem
	{
		public DataTable DataTable { get; }
		public int ColumnsCount { get; }
		public int RowCount { get; }
		public List<string> ColumnNames
		{
			get => GetColumnNames();
		}
		public List<DataRow> DataRows
		{
			get => GetDataRowsList();
		}
		public List<List<string>> DataRowItems
		{
			get => GetDataTableToLists();
		}



		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dt">Target DataTable</param>
		public DataRowListItem(DataTable dt)
		{
			DataTable = dt;

			// initializing
			ColumnsCount = dt.Columns.Count;
			RowCount = dt.Rows.Count;
		}


		// Public Methods
		public List<(DataRowListItem, DataRowListItem)> RowItemsPairs(DataTable targetDataTable)
		{
			List<(List<string>, List<string>)> lists = new();

			DataRowListItem dataRowListItem = new(targetDataTable);

			for (int i = 0; i < (dataRowListItem.RowCount > RowCount ? dataRowListItem.RowCount : RowCount); i++)
			{
				foreach ()
			}

			return null;
		}



		// Private Methods
		private List<string> GetColumnNames()
		{
			List<string> columnNames = new();

			foreach (var columnName in DataTable.Columns)
			{
				columnNames.Add(columnName.ToString());
			}

			return columnNames;
		}
		private List<DataRow> GetDataRowsList()
		{
			List<DataRow> dataRows = new();

            for (int i = 0; i < DataTable.Rows.Count; i++)
			{
				dataRows.Add(DataTable.Rows[i]);
			}

			return dataRows;
        }
		private List<List<string>> GetDataTableToLists()
		{
			List<List<string>> lists = new();

			for (int i = 0; i < RowCount; i++)
			{
				List<string> list = new();

				DataRow row = DataTable.Rows[i];

				foreach (string columnName in ColumnNames)
				{
					list.Add(row[columnName].ToString());
				}

				lists.Add(list);
			}

			return lists;
		}
	}
}
