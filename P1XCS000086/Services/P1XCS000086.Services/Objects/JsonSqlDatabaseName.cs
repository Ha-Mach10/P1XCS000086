using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Objects;

using Newtonsoft.Json;
using P1XCS000086.Services.Interfaces.Objects;

namespace P1XCS000086.Services.Objects
{
	[JsonObject("JsonSqlDatabaseName")]
	public class JsonSqlDatabaseName : IJsonSqlDatabaseName
	{
		[JsonProperty("DatabaseName")]
		public string DatabaseName { get; set; }
	}
}
