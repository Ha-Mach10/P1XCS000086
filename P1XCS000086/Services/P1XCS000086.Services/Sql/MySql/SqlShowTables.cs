using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P1XCS000086.Services.Sql.MySql
{
    public class SqlShowTables : ISqlShowTables
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
		public SqlShowTables()
		{

		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="connStr">接続文字列</param>
		public SqlShowTables(string connStr)
		{
			_connStr = connStr;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// テーブル一覧を取得する
		/// </summary>
		/// <returns>テーブル一覧のリスト</returns>
		public List<string> ShowTables()
		{
			string command = "SHOW TABLES FROM manager;";
			List<string> tables = GetShowTableItems(command);

			return tables;
		}
		/// <summary>
		/// テーブル一覧を取得する
		/// </summary>
		/// <param name="databaseName">データベース名称</param>
		/// <returns>テーブル一覧のリスト</returns>
		public List<string> ShowTables(string databaseName)
		{
			// 接続文字列がnullの時nullを返す
			if (_connStr is null) { return null; }

			// 
			string command = $"SHOW TABLES FROM {databaseName};";
			List<string> tables = GetShowTableItems(command);

			return tables;
		}

		/// <summary>
		/// 接続文字列のセット
		/// </summary>
		/// <param name="connStr"></param>
		public void SetConnectionString(string connStr)
		{
			_connStr = connStr;
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		private List<string> GetShowTableItems(string command)
		{
			List<string> tables = new List<string>();

			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connStr))
				{
					// コネクションを開く
					conn.Open();

					// アダプターを生成
					using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, _connStr))
					{
						DataTable dt = new DataTable();
						adapter.Fill(dt);

						tables = dt.AsEnumerable().Select(x => x.ToString()).ToList();
					}
				}
			}
			catch (Exception ex)
			{
				return new List<string>() { "no data" };
			}

			return tables;
		}
    }
}
