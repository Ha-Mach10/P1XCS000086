using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Objects;

using Newtonsoft.Json;

namespace P1XCS000086.Services.Objects
{
	[JsonObject("JsonSqlDatabaseNames")]
	public class JsonSqlDatabaseNames : IJsonSqlDatabaseNames
	{
		[JsonProperty("DatabaseNames")]
		public List<IJsonSqlDatabaseName> DatabaseNames { get; set; }
	}
}
