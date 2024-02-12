using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces.Sql
{
    public interface ISqlSelect
    {
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select();

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="command">クエリ文</param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string command);

		/*
		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="whereColumn"></param>
		/// <param name="whereValue"></param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(string whereColumn, string whereValue);

		/// <summary>
		/// SELECTクエリを実行する
		/// </summary>
		/// <param name="whereColumns"></param>
		/// <param name="whereValues"></param>
		/// <returns>SELECTされたDataTable</returns>
		public DataTable Select(List<string> columns, List<string> whereValues);
		*/

		/// <summary>
		/// 接続文字列を内部変数へ登録
		/// </summary>
		/// <param name="connStr">IMySqlConnectionStringインターフェース</param>
		public void SetConnectionString(IMySqlConnectionString sqlConnStr);

		/// <summary>
		/// クエリを実行し、取得した列からただ１つの項目を返す
		/// </summary>
		/// <param name="connectionString">接続文字列生成用インターフェース</param>
		/// <param name="columnName">カラム名</param>
		/// <param name="query">クエリ</param>
		/// <returns>取得されたただひとつの値</returns>
		public string GetJustOneSelectedItem(string columnName, string query);

		/// <summary>
		/// クエリを実行し、取得した列をリストへ格納
		/// </summary>
		/// <param name="command">クエリ</param>
		/// <returns>リスト化された値</returns>
		public List<string> SelectedColumnToList(string columnName, string query);

	}
}
