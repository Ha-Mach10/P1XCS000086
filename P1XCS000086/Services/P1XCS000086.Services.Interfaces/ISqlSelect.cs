using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces
{
	public interface ISqlSelect
	{
		public string GenerateQuery(string table, List<string> columns);
		public DataTable Select(string query);
	}
}
