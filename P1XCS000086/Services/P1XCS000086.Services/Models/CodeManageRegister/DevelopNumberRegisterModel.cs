﻿using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Sql.MySql;

using Reactive.Bindings;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace P1XCS000086.Services.Models.CodeManageRegister
{
	public class DevelopNumberRegisterModel : IDevelopNumberRegisterModel
	{
		// 
		private IMySqlConnectionString _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;


		public DevelopNumberRegisterModel()
		{
			
		}


		/// <summary>
		/// ViewModelへ注入されたインターフェースをセット
		/// </summary>
		/// <param name="select"></param>
		/// <param name="insert"></param>
		/// <param name="connStr"></param>
		public void SetModelBuiltin(ISqlSelect select, ISqlInsert insert, IMySqlConnectionString connStr)
		{
			// 
			_connStr = connStr;
			_select = select;
			_insert = insert;
		}
		/// <summary>
		/// 開発番号を返す
		/// </summary>
		/// <returns>
		/// 
		/// </returns>
		public string GetDevelopmentNumber()
		{
			// 開発番号の開発記号生成
			string devTypeValue = IntegrRegisterModel.DevTypeValue;
			string langTypeValue = IntegrRegisterModel.LangTypeValue;
			if (devTypeValue == "" && langTypeValue == "") { return string.Empty; }
			string codeNumberClassificationString = CodeNumberClassification(devTypeValue, langTypeValue);

			// 開発番号の追番生成
			int recordCount = IntegrRegisterModel.RecordCount;
			string codeNumber = $"{codeNumberClassificationString}{(recordCount + 1).ToString("000000")}";

			string developNumber = codeNumber;

			return developNumber;
		}
		/// <summary>
		/// ReactivePropertySlim<T>から値を取得する
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourcePropertiy">値を取得するReactivePropertySlim<T></param>
		/// <param name="substutituteValue">ReactivePropertySlimがnullの場合に代わりとなる値</param>
		/// <returns></returns>
		public T GetValue<T>(ReactivePropertySlim<T> sourcePropertiy, T substutituteValue)
		{
			// ReactivePropertySlim<T>.Valueがnullでない場合
			if (sourcePropertiy is not null)
			{
				T value = sourcePropertiy.Value;
				return value;
			}

			// ReactivePropertySlim<T>.Valueがnullであった場合、代替の値を返す
			return substutituteValue;
		}

		/// <summary>
		/// 使用用途の取得
		/// </summary>
		/// <param name="useAppSelectedValue"></param>
		/// <param name="useAppSubSelectedValue"></param>
		/// <param name="useApplicationManual"></param>
		/// <returns></returns>
		public string GetUseApplication(
			ReactivePropertySlim<string> useAppSelectedValue,
			ReactivePropertySlim<string> useAppSubSelectedValue,
			ReactivePropertySlim<string> useApplicationManual)
		{
			string useApplication = string.Empty;

			// ComboBoxで選択された値
			if (useAppSelectedValue is not null || useAppSubSelectedValue is not null)
			{
				useApplication = GetUseApplicationValue(useAppSelectedValue.Value);
				string useApplicationSub = GetUseApplicationValue(useAppSubSelectedValue.Value);

				useApplication = $"{useApplication}_{useApplicationSub}";
			}
			// TextBoxで選択された値
			else if (useApplicationManual is not null)
			{
				useApplication = useApplicationManual.Value;
			}

			return useApplication;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="developNumber"></param>
		/// <param name="developName"></param>
		/// <param name="codeName"></param>
		/// <param name="useApplication"></param>
		/// <param name="referenceNumber"></param>
		/// <param name="oldNumber"></param>
		/// <param name="newNumber"></param>
		/// <param name="inheritenceNumber"></param>
		/// <param name="explanation"></param>
		/// <param name="summary"></param>
		public string RegistValues(
			string developNumber,
			string developName,
			string codeName,
			string useApplication,
			string referenceNumber,
			string oldNumber,
			string newNumber,
			string inheritenceNumber,
			string explanation,
			string summary)
		{
			// 日付を取得
			string date = DateTime.Now.ToString();

			// Insert用の値リストを生成
			List<string> values = new List<string>
			{
				developNumber,
				developName,
				codeName,
				date,
				useApplication,
				referenceNumber,
				oldNumber,
				newNumber,
				inheritenceNumber,
				explanation,
				summary
			};

			if (!RegisterDevelopmentNumber(values, out string resultMessege))
			{
				// 例外文字列を返す
				return resultMessege;
			}

			return string.Empty;
		}


		/// <summary>
		/// 日本語名で与えられた開発種別・言語種別をマスタからコードで取得
		/// </summary>
		/// <param name="developType">開発種別</param>
		/// <param name="languageType">言語種別</param>
		/// <returns>
		/// 開発番号用の値
		/// </returns>
		public string CodeNumberClassification(string developType, string languageType)
		{
			// クエリを作成
			string queryCommand = @$"SELECT CONCAT(d.develop_type_code, l.language_type_code)
									 FROM manager_language_type AS l
									 JOIN manager_develop_type AS d
									 ON l.script_type = d.script_type
									 WHERE l.language_type='{languageType}' AND d.develop_type='{developType}';";
			string columnName = "CONCAT(d.develop_type_code, l.language_type_code)";
			string classificationString = GetSelectItem(columnName, queryCommand);

			return classificationString;
		}
		/// <summary>
		/// 日本語名で与えられた使用用途をマスタから英語名で取得
		/// </summary>
		/// <param name="selectedValue">使用用途欄から入力された値</param>
		/// <returns></returns>
		private string GetUseApplicationValue(string selectedValue)
		{
			string queryCommand = $"SELECT use_name_en FROM manager_use_application WHERE use_name_jp='{selectedValue}';";
			string getValue = GetSelectItem(selectedValue, queryCommand);

			return getValue;
		}
		/// <summary>
		/// クエリを実行し、取得した列からただ１つの項目を返す
		/// ※取得される項目がただ１つのみになるようクエリを作成すること
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="queryCommand"></param>
		/// <returns></returns>
		private string GetSelectItem(string columnName, string queryCommand)
		{
			// 接続文字列を取得
			if (!_connStr.IsGetConnectionString(out string connStr)) { return string.Empty; }

			// SELECTクエリ実行用のクラスをインターフェース経由で生成
			ISqlSelect selectExecute = new SqlSelect(connStr, queryCommand);
			DataTable dt = selectExecute.Select();

			// LINQで「dt」から指定のカラムのEnumerableRowCollection<DataRow>を取得
			var rowItmes = dt.AsEnumerable().Select(x => x[columnName]).ToList();

			// もし「rowItems」の項目数が１未満のとき、"Empty"を返す
			if (rowItmes.Count < 1) { return "Empty"; }

			// 取得したコレクションから、LINQで最初の項目を取得
			string item = rowItmes.First().ToString();

			return item;
		}

		/// <summary>
		/// manager_codesへInsertを実行し、成否の結果をboolで返す
		/// </summary>
		/// <param name="values"></param>
		/// <returns>
		/// Insert に成功した場合 true。失敗した場合 false。
		/// </returns>
		private bool RegisterDevelopmentNumber(List<string> values, out string resultMessage)
		{
			// カラム名のリストを生成
			List<string> columns = new List<string>()
			{
				"develop_number",
				"develop_name",
				"code_name",
				"create_date",
				"use_applications",
				"version",
				"revision_date", "old_number", "new_number", "inheritence_number",
				"explanation", "summary"
			};

			// 例外文字列の格納
			resultMessage = string.Empty;

			// INSERT用のカラム列を生成
			string columnsStr = string.Join(", ", columns);
			// パラメータクエリ用の値列を生成(SQLインジェクション対策)
			string valueStr = $"@{string.Join(", @", columns)}";

			// クエリ文字列を生成
			string queryCommand = @$"INSERT INTO manager_codes({columnsStr}) VALUES({valueStr});";

			// 
			string connStr = _connStr.GetConnectionString();
			// INSERTクエリを実行
			bool result = _insert.Insert(connStr, queryCommand, columns, values);

			// 結果文字列または例外文字列に値がセットされている場合
			if (_insert.ResultMessage != "" || _insert.ExceptionMessage != "")
			{
				// メッセージを取得
				resultMessage = $"{_insert.ResultMessage}\n{_insert.ExceptionMessage}";
			}

			return result;
		}
	}
}