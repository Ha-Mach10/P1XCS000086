using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;
using P1XCS000086.Services.IO;

namespace P1XCS000086.Services.Sql
{
	public static class SqlConnectionStrings
	{
		// Readonly Fields
		// 埋め込みリソース
		private static readonly string databaseTextFilePath = "P1XCS000086.Services.Resources.ConnectionStrings.txt";



		private static string _server;
		private static string _user;
		private static string _database;
		private static string _password;
		private static bool _persistSecurityInfo;
		
		

		/// <summary>
		/// ユーザー名(キー)・接続文字列（値）のペアを保持する静的ディクショナリ
		/// </summary>
		public static Dictionary<string, string> ConnectionStrings { get; private set; } = new Dictionary<string, string>();



		public static Dictionary<string, string> GetConnectionStrings()
		{
			DataTable dt = CsvParser.ReadCsv(databaseTextFilePath, typeof(SqlConnectionStrings));
			SetConnectionStrings(dt);

			return ConnectionStrings;
		}



		#region Private Methods

		/// <summary>
		/// DataTableから接続文字列生成に必要な値を抜き出し、
		/// 生成した接続文字列を「データベース名, 接続文字列」のディクショナリへ格納する
		/// </summary>
		/// <param name="dt">接続文字列の生成に必要なDataTable</param>
		private static void SetConnectionStrings(DataTable dt)
		{
			// DataTableからLINQを使用し、接続文字列生成に必要な値をフィールドに格納
			var dtItems = dt.Rows.Cast<DataRow>().Select(x =>
			{
				// カラムを指定して、各プロパティへ値を格納
				_server					= x["server"].ToString();
				_user					= x["user"].ToString();
				_database				= x["database"].ToString();
				_password				= x["password"].ToString();
				_persistSecurityInfo	= bool.Parse(x["persistSecurityInfo"].ToString());

				// ディクショナリへ接続文字列を追加
				SetDictionary();

				return x;
			}).ToList();

			
		}

		/// <summary>
		/// ディクショナリにデータベース名と接続文字列のペアを格納する
		/// </summary>
		private static void SetDictionary()
		{
			// MySqlの接続文字列ビルダを生成
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
			{
				// 接続文字列生成に必要な各プロパティに値をセット
				Server = _server,
				UserID = _user,
				Database = _database,
				Password = _password,
				PersistSecurityInfo = _persistSecurityInfo
			};

			// データベース名,接続文字列のペアを追加
			ConnectionStrings.Add(_database, builder.ConnectionString);
		}
		#endregion
	}
}
