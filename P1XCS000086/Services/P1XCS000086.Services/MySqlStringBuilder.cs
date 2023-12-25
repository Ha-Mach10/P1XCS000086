using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Principal;
using System.Text;

using MySql.Data;
using MySql.Data.MySqlClient;
using P1XCS000086.Services.Interfaces;

namespace P1XCS000086.Services
{
	public class MySqlStringBuilder : IMySqlStringBuilder
	{
		// Properties
		public string Server { get; set; }
		public string User { get; set; }
		public string Database {  get; set; }
		public string Password {  get; set; }
		public bool PersistSecurityInfo {  get; set; }

		public string ConnectionString { get; private set; }


		// Methods
		public string GetConnectionString()
		{
			return SetConnectionString();
		}

		public string SetConnectionString()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
			builder.Server = Server;
			builder.UserID = User;
			builder.Database = Database;
			builder.Password = Password;
			builder.PersistSecurityInfo = PersistSecurityInfo;

			return builder.ToString();
		}
	}
}
