using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageRegister
{
	public interface IIntegrRegisterModel
	{
		// ****************************************************************************
		// enum
		// ****************************************************************************

		// Visibility
		public enum Visibility
		{
			Visible = 0,
			Hidden = 1,
			Collapsed = 2
		}



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 開発種別の値のプロパティ
		/// </summary>
		public static string DevTypeValue { get; set; }
		
		/// <summary>
		/// 言語種別の値のプロパティ
		/// </summary>
		public static string LangTypeValue { get; set; }

		/// <summary>
		/// プロジェクトのディレクトリ
		/// </summary>
		public static string ProjectDirectryText { get; set; }

		/// <summary>
		/// 開発種別の選択アイテムのインデックス
		/// </summary>
		public static int DevItemSelectedIndex { get; set; }

		/// <summary>
		/// 開発種別が選択されているか
		/// </summary>
		public static bool IsDevItemSelected { get; set; } = false;

		/// <summary>
		/// 取得レコード数のプロパティ
		/// </summary>
		public static int RecordCount { get; set; }
		
		/// <summary>
		/// 登録ビューの可視性
		/// </summary>
		public static Visibility DevelopNumberContentControlVisibility { get; }
		
		/// <summary>
		/// CodeManageRegisterのメインビュー用
		/// </summary>
		public static DataTable GridDataTable { get; set; }



		// ****************************************************************************
		// Methods
		// ****************************************************************************

		/// <summary>
		/// Visibilityを変更する
		/// </summary>
		/// <param name="visibility"></param>
		public void ChangeVisibility(int visibility);

		/// <summary>
		/// Visibilityをintで取得する
		/// </summary>
		/// <returns></returns>
		public int GetVisibility();
	}
}
