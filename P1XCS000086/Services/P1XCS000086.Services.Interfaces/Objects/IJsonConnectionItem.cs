using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Objects
{
	public interface IJsonConnectionItem
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public List<IJsonConnectionStrings> ConnectionStrings { get; set; }
	}
}
