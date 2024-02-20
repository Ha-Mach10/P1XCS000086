using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Services.Interfaces.Models
{
    public interface IMainWindowModel
    {
        // Properties
        public string Server { get; set; }
        public string User { get; set; }
        public string Database { get; set; }
        public string Password { get; set; }
        public bool PersistSecurityInfo { get; set; }

        public string ResultMessage { get; }
        public string ExceptionMessage { get; }


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
		public void InjectModels(IJsonConnectionStrings jsonConnStr, IJsonExtention jsonExtention, IMySqlConnectionString sqlConnStr, IJsonConnectionItem jsonConnStrings, ISqlSchemaNames schemaNames);

        /// <summary>
        /// JSONファイルに設定された接続文字列情報をSQL接続文字列として復号
        /// </summary>
        public void SetConnectionString();
    }
}
