using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
    public interface IMySqlConnectionString
    {
        // Properties
        public string Server { get; }
        public string User { get; }
        public string Database { get; }
        public string Password { get; }
        public bool PersistSecurityInfo { get; }


        // Methods
        public string GetConnectionString();
        public string GenelateConnectionString(string server, string user, string database, string password, bool persistSecurityInfo);
        public bool IsGetConnectionString(out string connectionString);
    }
}
