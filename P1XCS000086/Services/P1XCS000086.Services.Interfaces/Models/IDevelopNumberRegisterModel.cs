using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models
{
	public interface IDevelopNumberRegisterModel
	{
		public string GetUseApplicationValue(string selectedValue);
		public void SetModelBuiltin(ISqlSelect select, ISqlInsert insert, IMySqlConnectionString connStr);
		public string CodeNumberClassification(string developType, string languageType);
	}
}
