using System;
using System.Collections.Generic;
using System.Text;

using P1XCS000086.Services.Interfaces.Models.CodeManager;

namespace P1XCS000086.Services.Models.CodeManager
{
	public class MasterManagerModel : IMasterManagerModel
	{
		// Fields
		private CommonModel _common;



		// Properties
		public List<string> LangTypes { get; }
		public List<string> DevTypes { get; }
		public List<string> UseAppMajor { get; }
		public List<string> UseAppRange { get; }



		// Constructor
		public MasterManagerModel()
		{
			_common = new CommonModel();


			LangTypes = _common.LangTypes;
			DevTypes = _common.DevTypes;
			UseAppMajor = _common.UseAppMajor;
			UseAppRange = _common.UseAppRange;
		}



		// Public Methods

	}
}
