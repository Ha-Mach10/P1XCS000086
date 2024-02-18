using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlSchemaNames : ISqlSchemaNames
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Manager { get; } = "manager";
	}
}
