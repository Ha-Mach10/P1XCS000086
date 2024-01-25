using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Domain;

namespace P1XCS000086.Services.Domains
{
	public class DBTableColumnFieldItem : IDBTableColumnFieldItem
	{
		public string ColumnName { get; private set; }
		public string FieldValue { get; set; }

		public DBTableColumnFieldItem(string columnName)
		{
			ColumnName = columnName;
		}
	}
}
