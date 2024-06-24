using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.CodeManagerView.Domains
{
	public class TableNameListItem
	{
		public string TableNameJp { get; }
		public string TableName { get; }



		public TableNameListItem(string tableNameJp, string tableName)
		{
			TableNameJp = tableNameJp;
			TableName = tableName;
		}
	}
}
