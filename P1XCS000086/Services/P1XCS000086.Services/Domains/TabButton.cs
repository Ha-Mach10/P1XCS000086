using System;
using System.Collections.Generic;
using System.Text;

using P1XCS000086.Services.Interfaces.Domains;

namespace P1XCS000086.Services.Domains
{
	public class TabButton : ITabButton
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Header { get; private set; }
		public string RegionName { get; private set; }
		public string ViewName { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public TabButton(string header, string regionName, string viewName)
		{
			// プロパティの初期化
			Header = header;
			RegionName = regionName;
			ViewName = viewName;
		}



		/// <summary>
		/// 親のインターフェースプロパティをコピー
		/// </summary>
		/// <param name="tabButton"></param>
		public void CopyParent(ITabButton tabButton)
		{
			Header = tabButton.Header;
			RegionName = tabButton.RegionName;
			ViewName = tabButton.ViewName;
		}
	}
}
