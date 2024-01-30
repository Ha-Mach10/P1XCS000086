using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Sql.MySql;


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
		/// 日本語名で与えられた使用用途をマスタから英語名で取得
		/// </summary>
		/// <param name="selectedValue">使用用途欄から入力された値</param>
		/// <returns></returns>
		public string GetUseApplicationValue(string selectedValue)
		{
			string queryCommand = $"SELECT use_name_en FROM manager_use_application WHERE use_name_jp='{selectedValue}';";
			string getValue = GetSelectItem(selectedValue, queryCommand);

			return getValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="developType"></param>
		/// <param name="languageType"></param>
		/// <returns></returns>
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
		public string GetDevelopNumber()
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
		public T GetValue<T>()
		{

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
	}
}
