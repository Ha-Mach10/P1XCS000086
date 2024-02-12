using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Data;

using P1XCS000086.Services.Interfaces.Models.CodeManageRegister;

using Visibility = P1XCS000086.Services.Interfaces.Models.CodeManageRegister.IIntegrRegisterModel.Visibility;
using System.Globalization;


namespace P1XCS000086.Services.Models.CodeManageRegister
{
	public class IntegrRegisterModel : IIntegrRegisterModel
	{
		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public IntegrRegisterModel() { }



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 開発種別の値
		/// </summary>
		public static string DevTypeValue { get; set; } = string.Empty;

		/// <summary>
		/// 言語種別の値
		/// </summary>
		public static string LangTypeValue { get; set; } = string.Empty;

		/// <summary>
		/// プロジェクトのディレクトリ
		/// </summary>
		public static string ProjectDirectryText { get; set; } = string.Empty;

		/// <summary>
		/// 開発種別の選択アイテムのインデックス
		/// </summary>
		public static int DevItemSelectedIndex { get; set; } = 0;

		/// <summary>
		/// 開発種別が選択されているか
		/// </summary>
		public static bool IsDevItemSelected { get; set; } = false;

		/// <summary>
		/// 取得レコード数
		/// </summary>
		public static int RecordCount { get; set; } = 0;

		/// <summary>
		/// 登録ビューの可視性
		/// </summary>
		public static Visibility DevelopNumberContentControlVisibility { get; private set; } = Visibility.Visible;

		/// <summary>
		/// CodeManageRegisterのメインビュー用DataTable
		/// </summary>
		public static DataTable GridDataTable { get; set; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// Visibilityを変更する
		/// </summary>
		/// <param name="visibility"></param>
		public void ChangeVisibility(int visibility)
		{
			Visibility settingVisibility;

			switch (visibility)
			{
				case 0:
					settingVisibility = Visibility.Visible;
					break;
				case 1:
					settingVisibility = Visibility.Hidden;
					break;
				case 2:
					settingVisibility = Visibility.Collapsed;
					break;
				default:
					return;
			}

			DevelopNumberContentControlVisibility = settingVisibility;
		}

		/// <summary>
		/// Visibilityをintで取得する
		/// </summary>
		/// <returns></returns>
		public int GetVisibility()
		{
			int visibility = (int)DevelopNumberContentControlVisibility;
			return visibility;
		}
	}
}
