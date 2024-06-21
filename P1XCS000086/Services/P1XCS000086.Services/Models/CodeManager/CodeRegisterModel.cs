using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Bcpg.OpenPgp;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Sql;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.CodeManager
{
	public class CodeRegisterModel : ICodeRegisterModel
	{
		public enum TranslateDataType
		{
			Table,
			Column,
		}
		public enum TranslateTargetLanguage
		{
			En,
			Jp,
		}

		// *****************************************************************************
		// Fields
		// *****************************************************************************

		private string _connStr;

		private SqlShowTables _showTables;
		private SqlSelect _select;
		private SqlInsert _insert;

		private string _columnName = string.Empty;
		private List<string> _columns = new List<string>();
		private List<string> _values = new List<string>();



		// *****************************************************************************
		// Properties
		// *****************************************************************************

		public string CodeDevType { get; private set; }

		public List<string> LangTypes { get; private set; }
		public List<string> DevTypes { get; private set; }

		public List<string> UseAppMajor { get; private set; }
		public List<string> UseAppRange { get; private set; }
		public List<string> UiFramework { get; private set; }

		public DataTable Table { get; private set; }


		// *****************************************************************************
		// Contructor
		// *****************************************************************************

		public CodeRegisterModel()
		{
			RefreshValue();
		}


		// *****************************************************************************
		// Publick Methods
		// *****************************************************************************

		public void RefreshValue()
		{
			// Keyに"manager"が含まれているか判別
			if (SqlConnectionStrings.ConnectionStrings.TryGetValue("manager", out string connStr) is false)
			{
				return;
			}

			// 接続文字列を取得
			_connStr = connStr;

			// MySQLのSELECT用クラスのインスタンスを生成し、初期化
			_showTables = new SqlShowTables(connStr);
			_select = new SqlSelect(connStr);
			_insert = new SqlInsert(connStr);


			// "manager_language_type"テーブルから"language_type"カラムを文字列のリストで取得
			LangTypes = _select.SelectedColumnToList("language_type", "SELECT `language_type` FROM `manager_language_type`;");
			DevTypes = _select.SelectedColumnToList("develop_type", "SELECT `develop_type` FROM `manager_develop_type`;");
			UseAppMajor = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 1;");
			UseAppRange = _select.SelectedColumnToList("use_name_jp", "SELECT `use_name_jp` FROM `manager_use_application` WHERE `sign` = 2;");

			int a = 0;
		}
		public List<string> SetDevType(string selectedValue)
		{
			string subQuery = "SELECT `script_type` FROM `manager_language_type` WHERE `language_type` = @language_type;";
			_columns = new List<string>() { "language_type" };
			_values = new List<string>() { selectedValue };

			// "script_type" を取得
			string subResult = _select.GetJustOneSelectedItem("script_type", subQuery, _columns, _values);
			
			// "subResult"から"develop_type"を取得
			string query = $"SELECT `develop_type` FROM `manager_develop_type` WHERE `script_type` = '{subResult}';";
			DevTypes = _select.SelectedColumnToList("develop_type", query);
			return DevTypes;
		}
		public List<string> SetFrameworkName(string selectedLangValue)
		{
			_columns = new List<string> { "target_language" };
			_values = new List<string> { selectedLangValue };
			string query = "SELECT `framework_name` FROM `manager_master_ui_framework` WHERE `target_language` = @target_language;";
			UiFramework = _select.SelectedColumnToList("framework_name", query, _columns, _values);

			return UiFramework;
		}
		public DataTable SetTable(string selectedLangType, string selectedDevType)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(@"SELECT CONCAT(`develop_type_code`, `language_type_code`)");
			sb.AppendLine("FROM `manager_language_type` AS `lang`");
			sb.AppendLine("INNER JOIN `manager_develop_type` AS `dev`");
			sb.AppendLine("ON `lang`.`script_type` = `dev`.`script_type`");
			sb.AppendLine("WHERE `language_type` = @language_type");
			sb.AppendLine("AND `develop_type` = @develop_type;");

			// 
			string subQuery = sb.ToString();
			_columns = new List<string>() { "language_type", "develop_type" };
			_values = new List<string>() { selectedLangType, selectedDevType };
			// 
			string tmpColumn = "CONCAT(`develop_type_code`, `language_type_code`)";
			// 
			string subResult = _select.GetJustOneSelectedItem(tmpColumn, subQuery, _columns, _values);
			CodeDevType = subResult;

			// 
			string query = $"SELECT * FROM `manager_register_code` WHERE `develop_number` LIKE '{subResult}%';";

			// 
			Table = _select.Select(query);

			return Table;
		}
		public string SetUseApplication(string useApplication)
		{
			string query = $"SELECT `use_name_en` FROM `manager_use_application` WHERE `use_name_jp` = '{useApplication}';";
			string result = _select.GetJustOneSelectedItem("use_name_en", query);

			return result;
		}
		public void InsertCodeManager(string devNum, string devName, string uiFramework, string date, string useApp, string explanation = "", string summary = "")
		{
			_columns = new List<string>
			{
				"develop_number",
				"develop_name",
				"ui_framework",
				"created_on",
				"use_applications",
				"explanation",
				"summary"
			};
			_values = new List<string>
			{
				devNum,
				devName,
				uiFramework,
				date,
				useApp,
				explanation,
				summary
			};

			var parametors = _columns.Select(x => $"?{x}").ToList();

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(@"INSERT INTO `manager_register_code`");
			sb.AppendLine($"({string.Join(", ", _columns)})");
			sb.AppendLine($"VALUES ({string.Join(", ", parametors)});");

			string query = sb.ToString();

			_insert.Insert(query, _columns, _values);
		}
		/// <summary>
		/// 指定した言語からその言語の開発ディレクトリの親ディレクトリをエクスプローラーで開く
		/// </summary>
		/// <param name="langType">言語名称</param>
		public void OpenProjectParentDirectry(string langType)
		{
			string languageTypeCode = GetLanguageType(langType);
			string parentDirPath = GetLanguageDirectry(languageTypeCode);

			Process.Start(parentDirPath);
		}
		/// <summary>
		/// 指定した言語から、その言語の指定された開発番号の親ディレクトリをエクスプローラーで開く
		/// </summary>
		/// <param name="developNumber">開発番号</param>
		/// <param name="langType">言語名称</param>
		public void OpenProjectDirectry(string developNumber, string langType)
		{
			string languageTypeCode = GetLanguageType(langType);
			string parentDirPath = GetLanguageDirectry(languageTypeCode);

			string path = $"{parentDirPath}\\{developNumber}";

			Process.Start(path);
		}
		/// <summary>
		/// 指定した言語から、その言語の開発用ファイルを規定のソフトウェアで開く
		/// </summary>
		/// <param name="developNumber">開発番号</param>
		/// <param name="dirFilePath">開発ファイル名</param>
		/// <param name="langType">言語名称</param>
		public void OpenProjectFile(string developNumber, string dirFilePath, string langType)
		{
			string languageTypeCode = GetLanguageType(langType);
			string dirPath = GetLanguageDirectry(languageTypeCode);

			string path = $"{dirFilePath}\\{developNumber}\\{dirFilePath}";

			Process.Start(path);
		}



		// *****************************************************************************
		// Private Methods
		// *****************************************************************************

		/// <summary>
		/// 言語種別を取得
		/// </summary>
		/// <param name="langTypeCode"></param>
		/// <returns></returns>
		private string GetLanguageType(string langTypeCode)
		{
			_columnName = "language_type_code";
			_columns = new List<string> { "language_type" };
			_values = new List<string> { langTypeCode };
			string query = $"SELECT `{_columnName}` FROM `manager_language_type` WHERE `language_type` = @language_type;";

			// リスト破棄
			ResetQueryFieldValiable();

			return _select.GetJustOneSelectedItem(_columnName, query, _columns, _values);
		}
		/// <summary>
		/// 開発種別を取得
		/// </summary>
		/// <param name="devType"></param>
		/// <returns></returns>
		private string GetDevelopmentType(string devType)
		{
			_columnName = "develop_type_code";
			_columns = new List<string> { "develop_type" };
			_values = new List<string> { devType };
			string query = $"SELECT `{_columnName}` FROM `manager_develop_type` WHERE `develop_type` = @develop_type;";

			// フィールド変数初期化
			ResetQueryFieldValiable();

			return _select.GetJustOneSelectedItem(_columnName, query, _columns, _values);
		}
		/// <summary>
		/// 指定言語のプロジェクトを格納した親ディレクトリを取得
		/// </summary>
		/// <param name="langTypeCode">言語種別</param>
		/// <returns></returns>
		private string GetLanguageDirectry(string langTypeCode)
		{
			_columnName = "language_directry";
			_columns = new List<string> { "language_type_code" };
			_values = new List<string> { langTypeCode };
			string query = $"SELECT `{_columnName}` FROM `development_project_directry` WHERE `language_type_code` = @language_type_code;";

			ResetQueryFieldValiable();

			return _select.GetJustOneSelectedItem(_columnName, query, _columns, _values);
		}
		/// <summary>
		/// DataTableのカラム名を翻訳する
		/// </summary>
		/// <param name="dt">対象のDataTable</param>
		/// <param name="tableName">対象となるテーブル</param>
		/// <param name="type">翻訳する対象（Table / Column）</param>
		/// <param name="targetLang">翻訳後言語</param>
		/// <returns></returns>
		private DataTable SetColumnName(DataTable dt, string tableName, TranslateTargetLanguage targetLang = TranslateTargetLanguage.Jp)
		{
			// 翻訳する言語（English or Japanese）
			switch (targetLang)
			{
				case TranslateTargetLanguage.En:
					_columnName = "column_name";
					break;
				case TranslateTargetLanguage.Jp:
					_columnName = "japanese";
					break;
			}

			string query1 = $"SELECT `{_columnName}` FROM `table_tranclator` WHERE `type` = 'column' AND WHERE `table_name` = '{tableName}'";
			List<string> columnNames = _select.SelectedColumnToList(_columnName, query1);
			List<string> tableColumnNames = _showTables.ShowTables("table_translator");

			// ColumnNameを置換
			columnNames.Zip(tableColumnNames, (baseColumnName, targetColumnName) =>
			{
				dt.Columns[baseColumnName].ColumnName = targetColumnName;

				return (baseColumnName, targetColumnName);
			});

			ResetQueryFieldValiable();

			return dt;
		}
		/// <summary>
		/// フィールド変数のリスト破棄
		/// </summary>
		private void ResetQueryFieldValiable()
		{
			_columnName = string.Empty;
			_columns = null;
			_values = null;
		}
	}
}
