using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManager
{
	public interface ICodeRegisterModel
	{
		public List<string> LangTypes { get; }
		public List<string> DevTypes { get; }
		public DataTable Table { get; }



		public void RefreshValue();
		public List<string> SetDevTpe(string selectedValue);
		public DataTable SetTable(string selectedLangType, string selectedDevType);
	}
}
