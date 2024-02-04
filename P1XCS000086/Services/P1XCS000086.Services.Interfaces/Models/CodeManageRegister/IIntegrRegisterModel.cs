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
		public static string DevTypeValue { get; }
		
		/// <summary>
		/// 言語種別の値のプロパティ
		/// </summary>
		public static string LangTypeValue { get; }
		
		/// <summary>
		/// 取得レコード数のプロパティ
		/// </summary>
		public static int RecordCount { get; }
		
		/// <summary>
		/// 登録ビューの可視性
		/// </summary>
		public static Visibility DevelopNumberContentControlVisibility { get; }
		
		/// <summary>
		/// CodeManageRegisterのメインビュー用
		/// </summary>
		public static DataTable GridDataTable { get; }



		// ****************************************************************************
		// Methods
		// ****************************************************************************

		/// <summary>
		/// 取得した開発種別・言語種別をプロパティに設定
		/// </summary>
		/// <param name="developmentValue">開発種別</param>
		/// <param name="languageValue">言語種別</param>
		public void RegistDevLangValues(string developmentValue, string languageValue);
		
		/// <summary>
		/// 取得したレコード数をプロパティに設定
		/// </summary>
		/// <param name="recordCount">レコード数</param>
		public void RegistRecordCount(int recordCount);

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

		/// <summary>
		/// データテーブルの値をプロパティにセット
		/// </summary>
		/// <param name="dt"></param>
		public void SetDataTable(DataTable dt);
	}
}
