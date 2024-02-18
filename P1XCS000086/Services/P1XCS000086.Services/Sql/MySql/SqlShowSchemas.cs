using MySql.Data.MySqlClient;

using P1XCS000086.Services.Interfaces.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Sql.MySql
{
	public class SqlShowSchemas : ISqlShowSchemas
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		// 接続文字列
		private string _connStr;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SqlShowSchemas()
		{

		}
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		public SqlShowSchemas(string connStr)
		{
			_connStr = connStr;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// スキーマ名のリストを取得する
		/// </summary>
		/// <returns>スキーマ名のリスト</returns>
		public List<string> ShowSchemas()
		{
			// スキーマ名のリストを取得
			return GetSchemas();
		}

		/// <summary>
		/// 接続文字列のセット
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		public void SetConnectionString(string connStr)
		{
			_connStr = connStr;
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// スキーマ名をリストで取得
		/// </summary>
		/// <returns>スキーマ名のリスト</returns>
		private List<string> GetSchemas()
		{
			DataTable dt = new DataTable();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connStr))
				using (MySqlDataAdapter adapter = new MySqlDataAdapter("SHOW SCHEMAS;", conn))
				{
					adapter.Fill(dt);
				}
			}
			catch (MySqlException ex)
			{
				Debug.Print(ex.Message);
				return null;
			}

			List<string> schemas = dt.AsEnumerable()
									 .Select(x => x["Database"].ToString())
									 .ToList();

			return schemas;
		}
	}
}
