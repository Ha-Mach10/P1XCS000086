﻿using System;
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

		private string _getColumn = string.Empty;
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
			List<string> columnNames = new List<string>() { "language_type" };
			List<string> values = new List<string>() { selectedValue };

			// "script_type" を取得
			string subResult = _select.GetJustOneSelectedItem("script_type", subQuery, columnNames, values);
			
			// "subResult"から"develop_type"を取得
			string query = $"SELECT `develop_type` FROM `manager_develop_type` WHERE `script_type` = '{subResult}';";
			DevTypes = _select.SelectedColumnToList("develop_type", query);
			return DevTypes;
		}
		public List<string> SetFrameworkName(string selectedLangValue)
		{
			List<string> columns = new List<string> { "target_language" };
			List<string> values = new List<string> { selectedLangValue };
			string query = "SELECT `framework_name` FROM `manager_master_ui_framework` WHERE `target_language` = @target_language;";
			UiFramework = _select.SelectedColumnToList("framework_name", query, columns, values);

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
			List<string> columnNames = new List<string>() { "language_type", "develop_type" };
			List<string> values = new List<string>() { selectedLangType, selectedDevType };
			// 
			string tmpColumn = "CONCAT(`develop_type_code`, `language_type_code`)";
			// 
			string subResult = _select.GetJustOneSelectedItem(tmpColumn, subQuery, columnNames, values);
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
			List<string> columnNames = new List<string>
			{
				"develop_number",
				"develop_name",
				"ui_framework",
				"created_on",
				"use_applications",
				"explanation",
				"summary"
			};
			List<string> values = new List<string>
			{
				devNum,
				devName,
				uiFramework,
				date,
				useApp,
				explanation,
				summary
			};

			var parametors = columnNames.Select(x => $"?{x}").ToList();

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(@"INSERT INTO `manager_register_code`");
			sb.AppendLine($"({string.Join(", ", columnNames)})");
			sb.AppendLine($"VALUES ({string.Join(", ", parametors)});");

			string query = sb.ToString();

			bool a = _insert.Insert(query, columnNames, values);

			int b = 0;
		}
		/// <summary>
		/// 指定した言語種別からその言語の開発ディレクトリの親ディレクトリをエクスプローラーで開く
		/// </summary>
		/// <param name="langType"></param>
		public void OpenProjectParentDirectry(string langType)
		{
			string langageTypeCode = GetLanguageType(langType);

			string parentDirPath = GetLanguageDirectry(langageTypeCode);

			Process.Start(parentDirPath);
		}
		public void OpenProjectDirectry(string langType, string devType)
		{
			// string 
		}



		// *****************************************************************************
		// Private Methods
		// *****************************************************************************


		private string GetLanguageType(string langTypeCode)
		{
			_columns = new List<string> { "language_type" };
			_values = new List<string> { langTypeCode };
			string query = "SELECT `language_type_code` FROM `manager_language_type` WHERE `language_type` = @language_type;";

			// リスト破棄


			return _select.GetJustOneSelectedItem("language_type", query, _columns, _values);
		}

		private string GetLanguageDirectry(string langTypeCode)
		{
			_getColumn = "language_directry";
			_columns = new List<string> { "language_type_code" };
			_values = new List<string> { langTypeCode };
			string query = $"SELECT `{_getColumn}` FROM `development_project_directry` WHERE `{_columns[0]}` = @{_columns[0]};";

			return _select.GetJustOneSelectedItem(_getColumn, query, _columns, _values);
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
			string targetColumn = string.Empty;
			switch (targetLang)
			{
				case TranslateTargetLanguage.En:
					targetColumn = "column_name";
					break;
				case TranslateTargetLanguage.Jp:
					targetColumn = "japanese";
					break;
			}

			string query1 = $"SELECT `{targetColumn}` FROM `table_tranclator` WHERE `type` = 'column' AND WHERE `table_name` = '{tableName}'";
			List<string> columnNames = _select.SelectedColumnToList(targetColumn, query1);
			List<string> tableColumnNames = _showTables.ShowTables("table_translator");

			// ColumnNameを置換
			columnNames.Zip(tableColumnNames, (baseColumnName, targetColumnName) =>
			{
				dt.Columns[baseColumnName].ColumnName = targetColumnName;

				return (baseColumnName, targetColumnName);
			});

			return dt;
		}
		/// <summary>
		/// フィールド変数のリスト破棄
		/// </summary>
		private void ClearLists()
		{
			_columns.Clear();
			_values.Clear();

			_columns.Capacity = 0;
			_values.Capacity = 0;
		}
	}
}
