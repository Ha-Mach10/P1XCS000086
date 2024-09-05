using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace P1XCS000086.Services.Sql.MySql
{
	internal static class MySqlQueryBuilder
	{
		public enum Sig
		{
			SigC,	// Target Column
			SigT,	// Target Table
			SigD,	// Target Datatable
			SigW,	// Where Option
			SigV,	// Insert Or Update Value Option
			SigO,	// Order By Option
			SigL,	// Limit Option
		}



		private static readonly string c_targetColumnsSig = "*C";
		private static readonly string c_targetTableSig = "*T";
		private static readonly string c_whereOptionSig = "*W";
		private static readonly string c_insrtOrUpdateValueOtionpSig = "*V";
		private static readonly string c_orderbyOptionSig = "*O";
		private static readonly string c_limitOptionSig = "*L";


		private static readonly string r_select = "SELECT `SigC` FROM `SigT` SigW SigO";
		private static readonly string r_insert = "INSERT INTO `SigC` VALUES SigV";
		private static readonly string r_update = "UPDATE `SigT` SET SigV SigW SigO";
		private static readonly string r_delete = "DELETE FROM `SigT` SigW SigO SigL";
		private static readonly string r_showtables = "SHOW TABLES `SigT` SigW";
		private static readonly string r_showschemas = "SHOW SHEMAS `SigT` SigW";



		// 
		public static string Query { get; private set; } = string.Empty;

		/*
		public MySqlQueryBuilder()
		{

		}
		*/

		/*
		public static string Select(string whereTable)
		{
			string queryStr = r_select;

			return string.Empty;
		}
		*/
		public static string Select(string targetColumn)
		{
			Query = $"SELECT `{targetColumn}`";
			return Query;
		}
		public static string Select(List<string> targetColumns)
		{
			string column = string.Join(",", targetColumns.Select(x => $"`{x}`"));
			Query = $"SELECT {column}";
			return Query;
		}
	}
}
