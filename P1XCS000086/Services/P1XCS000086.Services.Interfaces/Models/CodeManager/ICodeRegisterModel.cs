using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManager
{
	public interface ICodeRegisterModel
	{
		// *****************************************************************************
		// Properties
		// *****************************************************************************

		public string CodeDevType { get; }

		public List<string> LangTypes { get; }

		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }

		public List<string> UiFramework { get; }

		public DataTable Table { get; }



		// *****************************************************************************
		// Methods
		// *****************************************************************************

		public void RefreshValue();
		public List<string> SetDevType(string selectedValue);
		public List<string> SetFrameworkName(string selectedLangValue);
		public DataTable SetTable(string selectedLangType, string selectedDevType);
		public string SetUseApplication(string useApplication);
		public void InsertCodeManager(string devNum, string devName, string uiFramework, string date, string useApp, string explanation = "", string summary = "");
		
		/// <summary>
		/// 指定した言語からその言語の開発ディレクトリの親ディレクトリをエクスプローラーで開く
		/// </summary>
		/// <param name="langType"></param>
		public void OpenProjectParentDirectry(string langType);
		
		/// <summary>
		/// 指定した言語から、その言語の指定された開発番号の親ディレクトリをエクスプローラーで開く
		/// </summary>
		/// <param name="developNumber">開発番号</param>
		/// <param name="langType">言語名称</param>
		public void OpenProjectDirectry(string developNumber, string langType);
		
		/// <summary>
		/// 指定した言語から、その言語の開発用ファイルを規定のソフトウェアで開く
		/// </summary>
		/// <param name="developNumber">開発番号</param>
		/// <param name="dirFilePath">開発ファイル名</param>
		/// <param name="langType">言語名称</param>
		public void OpenProjectFile(string developNumber, string dirFilePath, string langType);
	}
}
