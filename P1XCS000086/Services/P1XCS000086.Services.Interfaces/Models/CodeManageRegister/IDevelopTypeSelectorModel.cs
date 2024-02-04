using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageRegister
{
    public interface IDevelopTypeSelectorModel
    {
		// ****************************************************************************
		// enums
		// ****************************************************************************

		/// <summary>
		/// 使用用途の選択種別
		/// </summary>
		public enum UseApplicationSign
		{
			// プロジェクトの大まかな属性
			Main = 1,
			// プロジェクトの規模
			Sub = 2,
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// ViewModelへ注入されたインターフェースをセット
		/// </summary>
		/// <param name="select">ISqlSelectインターフェース</param>
		/// <param name="connStr">IMySqlConnectionStringインターフェース</param>
		public void SetModelBuiltin(ISqlSelect select, IMySqlConnectionString connStr);

		/// <summary>
		/// 指定言語のプロジェクトフォルダ名をデータベースから取得
		/// </summary>
		/// <param name="languageType">言語種別名</param>
		/// <returns>プロジェクトのディレクトリ</returns>
		public string GetProjectDirectry(string languageType)

		/// <summary>
		/// 開発名称一覧をテーブルからリストで取得
		/// </summary>
		/// <param name="languageType">言語種別（日本語名）</param>
		/// <returns>開発名称一覧</returns>
		public List<string> DevelopmentComboBoxItemSetting(string languageType);

		/// <summary>
		/// 使用用途一覧をテーブルからリストで取得
		/// </summary>
		/// <returns>使用用途一覧</returns>
		public List<string> UseApplicationComboBoxItemSetting(UseApplicationSign sign);

		/// <summary>
		/// 「言語種別」にて変更した言語種別から対象の言語で作成された開発番号のテーブルを取得
		/// </summary>
		/// <param name="languageType">言語種別</param>
		/// <returns>開発番号テーブル</returns>
		public DataTable CodeManagerDataGridItemSetting(string languageType);

		/// <summary>
		/// 「言語種別」にて変更した言語種別から対象の言語で作成された号番を取得
		/// </summary>
		/// <param name="developType">開発種別</param>
		/// <param name="languageType">言語種別</param>
		/// <returns>開発番号テーブル</returns>
		public DataTable CodeManagerDataGridItemSetting(string developType, string languageType)
	}
}
