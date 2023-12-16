using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces
{
	public interface IMySqlCommand
	{
		public DataTable Select(string sql);
	}
}
