using P1XCS000086.Services.Interfaces.Models.CodeManageRegister;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Sql.MySql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseApplicationSign = P1XCS000086.Services.Interfaces.Models.CodeManageRegister.IDevelopTypeSelectorModel.UseApplicationSign;


namespace P1XCS000086.Services.Models.CodeManageRegister
{
    public class DevelopTypeSelectorModel : IDevelopTypeSelectorModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IMySqlConnectionString _connStr;
		private ISqlSelect _select;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public DevelopTypeSelectorModel()
		{

		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// ViewModelへ注入されたインターフェースをセット
		/// </summary>
		/// <param name="select">ISqlSelectインターフェース</param>
		/// <param name="connStr">IMySqlConnectionStringインターフェース</param>
		public void SetModelBuiltin(ISqlSelect select, IMySqlConnectionString connStr)
		{
			// 
			_connStr = connStr;
			_select = select;

			// SqlSelectクラス内のフィールド変数「_connStr」へ接続文字列をセットする
			_select.SetConnectionString(_connStr);
		}


		/// <summary>
		/// 開発名称一覧をテーブルからリストで取得
		/// </summary>
		/// <param name="languageType">言語種別（日本語名）</param>
		/// <returns>開発名称一覧</returns>
		public List<string> DevelopmentComboBoxItemSetting(string languageType)
		{
			string query = $"SELECT develop_type FROM manager_develop_type WHERE script_type=(SELECT script_type FROM manager_language_type WHERE language_type='{languageType}');";
			List<string> list = _select.SelectedColumnToList("develop_type", query);

			return list;
		}

		/// <summary>
		/// 使用用途一覧をテーブルからリストで取得
		/// </summary>
		/// <returns>使用用途一覧</returns>
		public List<string> UseApplicationComboBoxItemSetting(UseApplicationSign sign)
		{
			string query = $"SELECT use_name_jp FROM manager_use_application WHERE sign='*';";
			List<string> items = new();

			switch (sign)
			{
				case UseApplicationSign.Main:
					query = query.Replace("*", $"{(int)UseApplicationSign.Main}");
					break;

				case UseApplicationSign.Sub:
					query = query.Replace("*", $"{(int)UseApplicationSign.Sub}");
					break;

				default:
					return null;
			}

			_select.SelectedColumnToList("use_name_jp", query);

			return items;
		}

		/// <summary>
		/// 「言語種別」にて変更した言語種別から対象の言語で作成された開発番号のテーブルを取得
		/// </summary>
		/// <param name="languageType"></param>
		/// <returns>開発番号テーブル</returns>
		public DataTable CodeManagerDataGridItemSetting(string languageType)
		{
			// 号番検索用に「type_code」を「language_type」から取得
			string queryCommand = $"SELECT language_type_code FROM manager_language_type WHERE language_type='{languageType}'";
			// 「type_code」を取得
			string typeCode = _select.GetJustOneSelectedItem("language_type_code", queryCommand);

			// クエリを再生成
			queryCommand = $"SELECT * FROM manager_codes WHERE develop_number LIKE '%{typeCode}%';";
			DataTable dt = _select.Select(queryCommand);
			dt = CodeManagerColumnHeaderTrancelate(dt);

			return dt;
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// DataGridへ表示するDataTableのヘッダ名（ColumnName）を日本語へ変換する
		/// </summary>
		/// <param name="dataTable">変換元のDataTable</param>
		/// <returns>ヘッダ変換後のDataTable</returns>
		private DataTable CodeManagerColumnHeaderTrancelate(DataTable dataTable)
		{
			// 
			string queryCommand = $"SELECT japanese FROM table_translator WHERE table_name='manager_codes' AND type='column';";
			// カラムヘッダを日本語変換するためテーブルから取得
			List<string> columnHeaders = _select.SelectedColumnToList("japanese", queryCommand);
			int count = 0;
			foreach (string columnHeader in columnHeaders)
			{
				dataTable.Columns[count].ColumnName = columnHeader;
				count++;
			}

			return dataTable;
		}
	}
}
