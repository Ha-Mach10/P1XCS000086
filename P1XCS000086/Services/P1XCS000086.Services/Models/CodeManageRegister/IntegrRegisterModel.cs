using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Models.CodeManageRegister
{
	public static class IntegrRegisterModel
	{
		#region Properties
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
		#endregion


		/// <summary>
		/// 取得した開発種別・言語種別をプロパティに設定
		/// </summary>
		/// <param name="developmentValue">開発種別</param>
		/// <param name="languageValue">言語種別</param>
		public static void RegistDevLangValues(string developmentValue, string languageValue)
		{
			DevTypeValue = developmentValue;
			LangTypeValue = languageValue;
		}
		/// <summary>
		/// 取得したレコード数をプロパティに設定
		/// </summary>
		/// <param name="recordCount">レコード数</param>
		public static void RegistRecordCount(int recordCount)
		{
			RecordCount = recordCount;
		}
	}
}
