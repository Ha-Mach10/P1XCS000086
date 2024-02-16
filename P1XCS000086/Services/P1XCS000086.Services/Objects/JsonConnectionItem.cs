using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Interfaces.Objects;

using Newtonsoft.Json;

namespace P1XCS000086.Services.Objects
{
	[JsonObject("JsonConnectionItem")]
	public class JsonConnectionItem : IJsonConnectionItem
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("ConnectionStrings")]
		public List<IJsonConnectionStrings> ConnectionStrings { get; set; } = new List<IJsonConnectionStrings>();
	}
}
