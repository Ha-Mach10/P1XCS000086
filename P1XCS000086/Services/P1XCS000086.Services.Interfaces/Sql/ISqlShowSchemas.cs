using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
	public interface ISqlShowSchemas
	{
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// スキーマ名のリストを取得する
		/// </summary>
		/// <returns>スキーマ名のリスト</returns>
		public List<string> ShowSchemas();

		/// <summary>
		/// 接続文字列のセット
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		public void SetConnectionString(string connStr);
	}
}
