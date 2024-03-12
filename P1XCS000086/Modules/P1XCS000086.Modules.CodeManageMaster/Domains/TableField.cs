using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.CodeManageMaster.Domains
{
	public class TableField
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
