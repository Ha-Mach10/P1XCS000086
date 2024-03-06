using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains;

namespace P1XCS000086.Services.Models.CodeManageMaster.Domains
{
	public class TableField : ITableField
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// カラム名
		/// </summary>
		public string ColumnName { get; private set; }

		/// <summary>
		/// フィールドの値
		/// </summary>
		public string Value { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public TableField()
		{

		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// カラム名をセット
		/// </summary>
		/// <param name="columnName">セットするカラム名</param>
		public void SetColumnName(string columnName)
		{
			ColumnName = columnName;
		}

		/// <summary>
		/// 値をセット
		/// </summary>
		/// <param name="value">セットする値</param>
		public void SetValue(string value)
		{
			Value = value;
		}
	}
}
