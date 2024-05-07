using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P1XCS000086.Services.Domains;
using P1XCS000086.Services.Interfaces.Domains;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Models;
using Reactive.Bindings;

namespace P1XCS000086.Services.Models
{
	public class MergeModel : IMergeModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<string> RegionName { get; }
		public ReactivePropertySlim<List<ITabButton>> TabButtons { get; }
		// public List<ITabButton> TabButtons { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public MergeModel()
		{
			RegionName = new ReactivePropertySlim<string>(string.Empty);
			TabButtons = new ReactivePropertySlim<List<ITabButton>>();
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		public void ChangeRegionName(string newRegionName)
		{
			RegionName.Value = newRegionName;
		}
		public void AddTabButtons(ITabButton tabButton)
		{
			// 
			if (TabButtons is null) { return; }
			if (TabButtons.Value is null)
			{
				TabButtons.Value = new List<ITabButton>() { tabButton };
				return;
			}

			var pickTabItem = TabButtons.Value.Where(x => x.Header == tabButton.Header);
			if (TabButtons.Value.Where(x => x.Header == tabButton.Header).Any())
			{
				return;
			}

			TabButtons.Value.Add(tabButton);
		}
	}
}
