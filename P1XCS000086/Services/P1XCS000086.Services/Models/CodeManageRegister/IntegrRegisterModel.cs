using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Data;

using P1XCS000086.Services.Interfaces.Models.CodeManageRegister;

using Visibility = P1XCS000086.Services.Interfaces.Models.CodeManageRegister.IIntegrRegisterModel.Visibility;


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
		/// 開発種別の値のプロパティ
		/// </summary>
		public static string DevTypeValue { get; private set; } = string.Empty;

		/// <summary>
		/// 言語種別の値のプロパティ
		/// </summary>
		public static string LangTypeValue { get; private set; } = string.Empty;

		/// <summary>
		/// 取得レコード数のプロパティ
		/// </summary>
		public static int RecordCount { get; private set; } = 0;

		/// <summary>
		/// 登録ビューの可視性
		/// </summary>
		public static Visibility DevelopNumberContentControlVisibility { get; private set; } = Visibility.Visible;

		/// <summary>
		/// CodeManageRegisterのメインビュー用
		/// </summary>
		public static DataTable GridDataTable { get; private set; } = new();



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 取得した開発種別・言語種別をプロパティに設定
		/// </summary>
		/// <param name="developmentValue">開発種別</param>
		/// <param name="languageValue">言語種別</param>
		public void RegistDevLangValues(string developmentValue, string languageValue)
		{
			DevTypeValue = developmentValue;
			LangTypeValue = languageValue;
		}

		/// <summary>
		/// 取得したレコード数をプロパティに設定
		/// </summary>
		/// <param name="recordCount">レコード数</param>
		public void RegistRecordCount(int recordCount)
		{
			RecordCount = recordCount;
		}

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

		/// <summary>
		/// データテーブルの値をプロパティにセット
		/// </summary>
		/// <param name="dt"></param>
		public void SetDataTable(DataTable dt)
		{
			GridDataTable = dt;
		}
	}
}
