using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces
{
	public interface IJsonConnectionStrings
	{
		public string Server { get; set; }
		public string User { get; set; }
		public string DatabaseName { get; set; }
		public string Password { get; set; }
		public bool PersistSecurityInfo { get; set; }

		public bool IsPropertiesExists();
	}
}
