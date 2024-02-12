using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
	public interface ISqlDatabaseStrings
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// データベース名のリスト
		/// </summary>
		public static List<string> DatabaseNames { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// データベース名のリストをプロパティに追加
		/// </summary>
		/// <param name="databaseNames">データベース名のリスト</param>
		public void SetDatabaseNames(List<string> databaseNames);
	}
}
