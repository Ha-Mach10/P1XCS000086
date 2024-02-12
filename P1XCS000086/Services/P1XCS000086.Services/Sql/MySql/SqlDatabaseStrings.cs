using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlDatabaseStrings : ISqlDatabaseStrings
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// データベース名のリスト
		/// </summary>
		public static List<string> DatabaseNames { get; set; } = new List<string>();



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// データベース名のリストをプロパティに追加
		/// </summary>
		/// <param name="databaseNames">データベース名のリスト</param>
		public void SetDatabaseNames(List<string> databaseNames)
		{
			DatabaseNames = databaseNames;
		}
	}
}
