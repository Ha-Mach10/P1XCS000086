using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster.Domains
{
	public interface ITableField
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// カラム名
		/// </summary>
		public string ColumnName { get; }

		/// <summary>
		/// フィールドの値
		/// </summary>
		public string Value { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// カラム名をセット
		/// </summary>
		/// <param name="columnName">セットするカラム名</param>
		public void SetColumnName(string columnName);

		/// <summary>
		/// 値をセット
		/// </summary>
		/// <param name="value">セットする値</param>
		public void SetValue(string value);
	}
}
