using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces
{
	public interface IMainWindowModel
	{
		public void JsonSerialize(string server, string user, string database, string password, bool persistSecurityInfo);
		public List<string> LanguageComboBoxItemSetting();
		public List<string> DevelopmentComboBoxItemSetting(string scriptType);
		public List<string> UseApplicationComboBoxItemSetting();
		public List<string> UseApplicationSubComboBoxItemSetting();
		public DataTable CodeManagerDataGridItemSetting(string languageType);
		public DataTable CodeManagerDataGridItemSetting(string developType, string languageType);
		public string RegistCodeNumberComboBoxItemSelect(string selectedValue);
		public string ConnectionString();
		public bool SqlConnection();
		public IJsonConnectionStrings JsonDeserialize();
	}
}
