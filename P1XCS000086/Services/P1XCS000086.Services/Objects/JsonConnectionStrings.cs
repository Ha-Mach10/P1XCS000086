using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

using P1XCS000086.Services.Interfaces;

namespace P1XCS000086.Services.Objects
{
	[JsonObject("JsonConnectionStrings")]
	public class JsonConnectionStrings : IJsonConnectionStrings
	{
		[JsonProperty("Server")]
		public string Server {  get; set; }

		[JsonProperty("User")]
		public string User {  get; set; }

		[JsonProperty("DatabaseName")]
		public string DatabaseName {  get; set; }

		[JsonProperty("Password")]
		public string Password { get; set; }

		[JsonProperty("PersistSecurityInfo")]
		public bool PersistSecurityInfo { get; set; }
	}
}
