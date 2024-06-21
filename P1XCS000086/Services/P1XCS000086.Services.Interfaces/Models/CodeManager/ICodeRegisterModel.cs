using System;
using System.Collections.Generic;
using System.Data;
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
		public List<string> DevTypes { get; }

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
		/// 指定した言語種別からその言語の開発ディレクトリの親ディレクトリをエクスプローラーで開く
		/// </summary>
		/// <param name="langType"></param>
		public void OpenProjectParentDirectry(string langType);
	}
}
