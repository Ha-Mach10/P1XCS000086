using P1XCS000086.Services.Interfaces.Domains;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models
{
	public interface IMergeModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<string> RegionName { get; }
		public ReactivePropertySlim<List<ITabButton>> TabButtons { get; }
		// public List<ITabButton> TabButtons { get; }


		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		public void ChangeRegionName(string newRegionName);
		public void AddTabButtons(ITabButton tabButton);
	}
}
