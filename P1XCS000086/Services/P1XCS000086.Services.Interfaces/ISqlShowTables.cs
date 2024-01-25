using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces
{
	public interface ISqlShowTables
	{
		public List<string> ShowTables();
		public List<string> ShowTables(string tableName);
	}
}
