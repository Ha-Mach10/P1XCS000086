using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Domains
{
	public interface IDBTableColumnFieldItem
	{
		string ColumnName { get; }
		public string FieldValue { get; set; }
	}
}
