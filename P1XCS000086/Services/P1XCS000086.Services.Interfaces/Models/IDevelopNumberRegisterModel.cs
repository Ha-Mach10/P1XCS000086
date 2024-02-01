using P1XCS000086.Services.Interfaces.Sql;

using Reactive.Bindings;

using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models
{
	public interface IDevelopNumberRegisterModel
	{
		public void SetModelBuiltin(ISqlSelect select, ISqlInsert insert, IMySqlConnectionString connStr);
		public string CodeNumberClassification(string developType, string languageType);
		public string GetDevelopmentNumber();
		public T GetValue<T>(ReactivePropertySlim<T> sourcePropertiy, T substutituteValue);
		public string GetUseApplication(
			ReactivePropertySlim<string> useAppSelectedValue,
			ReactivePropertySlim<string> useAppSubSelectedValue,
			ReactivePropertySlim<string> useApplicationManual);
		public string RegistValues(
			string developNumber,
			string developName,
			string codeName,
			string useApplication,
			string referenceNumber,
			string oldNumber,
			string newNumber,
			string inheritenceNumber,
			string explanation,
			string summary);
	}
}
