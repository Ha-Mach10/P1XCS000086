using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces
{
	internal interface ISelectQuery
	{
		public DataTable Select(string query);
	}
}
