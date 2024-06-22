﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManager
{
	public interface IMasterManagerModel
	{
		// Properties
		public List<string> LangTypes { get; }
		public List<string> DevTypes { get; }
		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Public Methods

	}
}