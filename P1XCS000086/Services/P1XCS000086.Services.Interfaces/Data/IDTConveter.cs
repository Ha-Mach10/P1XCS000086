using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Data
{
	public interface IDTConveter
	{
		// Properties
		public List<string> ExceptionMessages { get; }
		public List<string> ResultMessages { get; }



		// Public Methods
		public DataTable Convert(List<string> columnNames, object[,] gridObjects);
		public void DataTablesCompatation(DataTable beforeTable, DataTable afterTable, string databaseName, string tableName);
	}
}
