using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
	public interface ISqlConnectionTest
	{
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// SQLの接続テスト用メソッド
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		/// <returns>接続の成功/失敗</returns>
		public bool SqlConnection(string connStr);
	}
}
