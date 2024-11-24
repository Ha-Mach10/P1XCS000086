using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces.Models.CodeManager
{
	public interface ICodeRegisterModel
	{
		// *****************************************************************************
		// Properties
		// *****************************************************************************

		public Dictionary<string, string> ConnStrings { get; }
		public string CodeDevType { get; }

		public List<string> LangTypes { get; }

		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }

		public List<string> UiFramework { get; }

		public DataTable Table { get; }

		public string ResultMessage { get; }

		// public List<dynamic> Windows { get; }



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
		/// 
		/// </summary>
		/// <param name="selectedRow"></param>
		/// <returns></returns>
		public List<(string columnNames, string propertyText)> GetSelectedRowPropertyFieldItem(DataRowView selectedRow);

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

		/// <summary>
		/// 
		/// </summary>
		public void AwakeVS2019();

		/// <summary>
		/// 
		/// </summary>
		public void AwakeVS2022();

		/// <summary>
		/// 
		/// </summary>
		public Task<IntPtr> FindProcessMainwindowHandle(int delayTicks);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="selectedLanguageTypeCode"></param>
		/// <returns></returns>
		public string GetProjectDirTableToDirPath(string selectedLanguageTypeCode);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="selectedLanguageTypeCode"></param>
		/// <returns></returns>
		public bool UpdateProjectFileName(string selectedLanguageTypeCode, string developNumber);
	}
}
