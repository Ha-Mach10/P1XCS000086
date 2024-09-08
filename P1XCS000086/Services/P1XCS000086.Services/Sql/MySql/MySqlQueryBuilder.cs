using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
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
		[Flags]
		public enum ReservedKeyWord
		{
			SELECT		= 0B_0000_0000_0000_0001,	// 1   :SELECT 
			INSERT		= 0B_0000_0000_0000_0010,	// 2   :INSERT
			UPDATE		= 0B_0000_0000_0000_0100,	// 4   :UPDATE
			DELETE		= 0B_0000_0000_0000_1000,	// 8   :DELETE
			FROM		= 0B_0000_0000_0001_0000,	// 16  :FROM
			SET			= 0B_0000_0000_0010_0000,	// 32  :SET
			WHERE		= 0B_0000_0000_0100_0000,	// 64  :WHERE
			AND			= 0B_0000_0000_1000_0000,	// 128 :AND
			OR			= 0B_0000_0001_0000_0000,	// 256 :OR
			ORDER_BY	= 0B_0000_0010_0000_0000,	// 512 :ORDER BY
		}
		public enum QueryOperator
		{

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

		private static int _bitFlag = 0;

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
		public static void Select()
		{
			_bitFlag = (int)ReservedKeyWord.SELECT;
			Query = $"{ReservedKeyWord.SELECT.ToString()} *";
		}
		public static void Select(string targetColumnName)
		{
			_bitFlag = (int)ReservedKeyWord.SELECT;
			Query = $"{ReservedKeyWord.SELECT.ToString()} `{targetColumnName}`";
		}
		public static void Select(List<string> targetColumnsName)
		{
			_bitFlag = (int)ReservedKeyWord.SELECT;
			string column = string.Join(",", targetColumnsName.Select(x => $"`{x}`"));
			Query = $"{ReservedKeyWord.SELECT.ToString()} {column}";
		}
		public static void Delete()
		{
			_bitFlag = (int)ReservedKeyWord.DELETE;
			Query = $"{ReservedKeyWord.DELETE.ToString()} ";
		}
		public static string From(string targetTableName)
		{
			if (_bitFlag is not (int)ReservedKeyWord.SELECT ||
				_bitFlag is not (int)ReservedKeyWord.DELETE)
			{
				return string.Empty;
			}


			if (Query == string.Empty ||
				Query.Contains(ReservedKeyWord.SELECT.ToString()) ||
				Query.Contains(ReservedKeyWord.DELETE.ToString()))
			{
				Query = $"{Query} {ReservedKeyWord.FROM.ToString()} `{targetTableName}`";
			}
            else
            {
                 Query = string.Empty;
            }

            return Query;
		}
		/*
		public static string Where()
		{

		}
		public static string Where(string columnName, string value)
		{
			Query
		}
		*/



		private static int SubstractBinary(int substractTarget, int substractValue)
		{
			// 排他的論理和（XOR = ^）で要素の減算を行う
			int queryBin = substractTarget ^ substractValue;
			return queryBin;
		}
	}
}
