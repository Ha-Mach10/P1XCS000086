﻿using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Sql.MySql;

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Net.Http.Headers;
using MySqlX.XDevAPI.Relational;


namespace P1XCS000086.Services.Models
{
	public class MainWindowModel : IMainWindowModel
	{
		// Properies 
		public string Server { get; private set; }
		public string User {  get; private set; }
		public string Database {  get; private set; }
		public string Password { get; private set; }
		public bool PersistSecurityInfo { get; private set; }

		public JsonConnectionStrings JsonConnString { get; private set; }


		public MainWindowModel()
		{

		}


		/// <summary>
		/// 接続文字列をJSONシリアル化
		/// </summary>
		/// <param name="server">サーバー</param>
		/// <param name="user">ユーザー名</param>
		/// <param name="database">接続データベース</param>
		/// <param name="password">パスワード</param>
		/// <param name="persistSecurityInfo"></param>
		public void JsonSerialize(string server, string user, string database, string password, bool persistSecurityInfo)
		{
			JsonExtention jsonExtention = new JsonExtention();

			// フォルダ・ファイル存在チェック。存在しなければ生成
			jsonExtention.PathCheckAndGenerate();

			// SQL接続文字列をJSONシリアライズ用クラスの各プロパティへ設定
			JsonConnectionStrings connStrings = new JsonConnectionStrings();
			connStrings.Server = server;
			connStrings.User = user;
			connStrings.DatabaseName = database;
			connStrings.Password = password;
			connStrings.PersistSecurityInfo = persistSecurityInfo;

			// SQLの接続文字列をJSONファイルへシリアライズ化
			string jsonFilePath = JsonExtention.JsonFilePath;
			jsonExtention.SerializeJson(connStrings, jsonFilePath, false);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<string> LanguageComboBoxItemSetting()
		{
			string queryCommand = "SELECT language_type FROM manager_language_type;";
			List<string> list = QueryExecuteToList(queryCommand);
			return list;
		}
		public List<string> DevelopmentComboBoxItemSetting(string languageType)
		{
			string queryCommand = $"SELECT develop_type FROM manager_develop_type WHERE script_type=(SELECT script_type FROM manager_language_type WHERE language_type='{languageType}');";
			List<string> list = QueryExecuteToList(queryCommand);
			return list;
		}
		/// <summary>
		/// 「言語種別」にて変更した言語から対象の言語で作成された号番を取得
		/// </summary>
		/// <param name="languageType"></param>
		/// <returns></returns>
		public DataTable CodeManagerDataGridItemSetting(string languageType)
		{
			// コマンドクエリを宣言
			string queryCommand = string.Empty;

			// 号番検索用に「type_code」を「language_type」から取得
			queryCommand = $"SELECT type_code FROM manager_language_type WHERE language_type='{languageType}'";
			// 接続文字列を取得
			string connStr = ConnectionString();
			// SELECTコマンド実行用のクラスを生成
			ISqlSelect selectExecute = new SqlSelect(connStr);
			// 「type_code」を格納するためのDataTableを用意し、SELECTクエリを実行
			DataTable typeCodeDt = selectExecute.Select(queryCommand);
			// 「type_code」を取得
			string typeCode = typeCodeDt.Rows[0][0].ToString();

			// クエリを再生成
			queryCommand = $"SELECT * FROM manager_codes WHERE develop_number LIKE '%{typeCode}%';";
			DataTable dt = selectExecute.Select(queryCommand);

			return dt;
		}

		private List<string> QueryExecuteToList(string command)
		{
			string connStr = ConnectionString();
			if (connStr == string.Empty)
			{
				return new List<string>() { "Non Items" };
			}

			ISqlSelect selectExecute = new SqlSelect(connStr, command);
			DataTable dt = selectExecute.Select();

			List<string> items = new List<string>();
			// if (dt)
			foreach (DataRow dr in dt.Rows)
			{
				items.Add(dr[0].ToString());
			}

			return items;
		}
		private string QueryExecute(string command)
		{
			string connStr = ConnectionString();
			ISqlSelect selectExecute = new SqlSelect(connStr, command);
			DataTable dt = selectExecute.Select();

			string item = dt.Rows[0][0].ToString();

			return item;
		}
		/// <summary>
		/// SQLの接続テスト用メソッド
		/// </summary>
		/// <returns>接続の成功/失敗</returns>
		public bool SqlConnection()
		{
			string connStr = ConnectionString();
			if (connStr == string.Empty)
			{
				return false;
			}

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connStr))
				{
					conn.Open();
					conn.Close();

					return true;
				}
			}
			catch (MySqlException msex)
			{
				Debug.WriteLine(msex.Message);

				return false;
			}
		}
		/// <summary>
		/// SQLの接続文字列を生成し返すメソッド
		/// </summary>
		/// <returns>接続文字列</returns>
		public string ConnectionString()
		{
			string connStr = string.Empty;

			IJsonConnectionStrings jsonConnString = JsonDeserialize();
			MySqlStringBuilder builder = new MySqlStringBuilder();
			if (jsonConnString is not null)
			{
				builder.Server = jsonConnString.Server;
				builder.User = jsonConnString.User;
				builder.Database = jsonConnString.DatabaseName;
				builder.Password = jsonConnString.Password;
				builder.PersistSecurityInfo = jsonConnString.PersistSecurityInfo;
				connStr = builder.GetConnectionString();
			}

			return connStr;
		}
		/// <summary>
		/// JSONファイルからSQL接続文字列を復号
		/// </summary>
		public IJsonConnectionStrings JsonDeserialize()
		{
			JsonExtention jsonExtention = new JsonExtention();

			// ファイルが存在していなければ生成し、処理を抜ける
			if (!jsonExtention.PathCheckAndGenerate()) { return null; }

			// JSONファイルからSQL接続文字列を復号し、プロパティにセット
			string jsonFilePath = JsonExtention.JsonFilePath;
			return jsonExtention.DeserializeJson<JsonConnectionStrings>(jsonFilePath);
		}
	}
}
