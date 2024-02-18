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



		public void JsonSerialize(string server, string user, string database, string password, bool persistSecurityInfo);
        public List<string> LanguageComboBoxItemSetting();
        public List<string> DevelopmentComboBoxItemSetting(string scriptType);
        public List<string> UseApplicationComboBoxItemSetting();
        public List<string> UseApplicationSubComboBoxItemSetting();
        public List<string> ViewUseApplicationComboBoxItemSetting();
        public List<string> ShowTableItems();
        public List<string> SearchTextUseApplicationComboBoxItemSetting(string developType, string languageType);
        public List<string> GetInPutFieldColumns(string tableName);
        public DataTable MasterTableData(string tableName);
        public DataTable GetViewDataTable(string langValue = "", string useAppValue = "");
        public DataTable CodeManagerDataGridItemSetting(string languageType);
        public DataTable CodeManagerDataGridItemSetting(string developType, string languageType);
        public string RegistCodeNumberComboBoxItemSelect(string selectedValue);
        public string CodeNumberClassification(string developType, string languageType);
        public string GetProjectDirectry(string languageType);
        public bool RegistExecute(List<string> values);
        public string ConnectionString();
        public bool SqlConnection();
        public IJsonConnectionStrings JsonDeserialize();
    }
}
