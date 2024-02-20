using P1XCS000086.Services.IO;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Sql.MySql;

using System;
using System.Text;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Http.Headers;
using System.Reflection.Emit;

using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using MySqlX.XDevAPI;
using System.IO;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Objects;


namespace P1XCS000086.Services.Models
{
    public class MainWindowModel : IMainWindowModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IJsonConnectionStrings _jsonConnStr;
		private IJsonExtention _jsonExtention;
		private IMySqlConnectionString _sqlConnStr;
		private IJsonConnectionItem _jsonConnStrings;
		private ISqlSchemaNames _schemaNames;



		// Properies 
		public string Server { get; set; }
		public string User {  get; set; }
		public string Database {  get; set; }
		public string Password { get; set; }
		public bool PersistSecurityInfo { get; set; }

		public JsonConnectionStrings JsonConnString { get; private set; }

		public string ResultMessage { get; private set; }
		public string ExceptionMessage { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public MainWindowModel()
		{

		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたオブジェクトをModelに注入
		/// </summary>
		/// <param name="jsonConnStr">JSON接続文字列生成</param>
		/// <param name="jsonExtention">JSONオブジェクト</param>
		/// <param name="sqlConnStr">接続文字列用オブジェクト</param>
		/// <param name="jsonConnStrings">JSONファイル接続文字列用オブジェクト</param>
		/// <param name="schemaNames">データベース名オブジェクト</param>
		public void InjectModels(IJsonConnectionStrings jsonConnStr, IJsonExtention jsonExtention, IMySqlConnectionString sqlConnStr, IJsonConnectionItem jsonConnStrings, ISqlSchemaNames schemaNames)
		{
			_jsonConnStr = jsonConnStr;
			_jsonConnStrings = jsonConnStrings;
			_jsonExtention = jsonExtention;
			_sqlConnStr = sqlConnStr;
			_schemaNames = schemaNames;
		}

		/// <summary>
		/// JSONファイルに設定された接続文字列情報をSQL接続文字列として復号
		/// </summary>
		public void SetConnectionString()
		{
			// 
			_jsonConnStrings = _jsonExtention.DeserializeJson<JsonConnectionItem>(_jsonExtention.JsonSqlFilePath);
			// IJsonConnectionStrings connection = _jsonConnStrings.ConnectionStrings.Where(x => x.DatabaseName == _schemaNames.Manager).First();

			foreach (var connection in _jsonConnStrings.ConnectionStrings)
			{
				connection.AddConnectionString();
			}
		}
		
		/// <summary>
		/// 
		/// 参考サイト：https://qiita.com/mytk-k/items/4a338965cb7bc3d584ec
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<string> ToWords(string source)
		{
			// 作業用変数の宣言           
			var wordbreakIndex = 0;          // 現在の単語の始まりのインデックス
			var currentWordLength = 0;       // 現在の単語の文字数
			var current = '\0';              // ループの中で現在参照している文字
			var isLowerBefore = false;       // 一つ前の文字が小文字だったかどうか
			var isUpperCurrent = false;      // 現在の文字が大文字かどうか

			for (var i = 0; i < source.Length; i++)
			{
				current = source[i];
				isUpperCurrent = char.IsUpper(current);

				if (isLowerBefore && isUpperCurrent)
				{
					// 小文字から大文字に切り替わった時に単語を切り出す。
					yield return source.Substring(wordbreakIndex, currentWordLength);
					wordbreakIndex = i;
					currentWordLength = 0;
				}

				currentWordLength++;
				isLowerBefore = char.IsLower(current);
			}

			// 最後の単語の返却漏れがないように
			yield return source.Substring(wordbreakIndex, source.Length - wordbreakIndex);
		}
	}
}
