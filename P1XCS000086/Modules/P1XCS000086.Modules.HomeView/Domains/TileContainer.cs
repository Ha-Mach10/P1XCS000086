using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Domains;
using Prism.Navigation.Regions;
using Reactive.Bindings;

namespace P1XCS000086.Modules.HomeView.Domains
{
	public class TileContainer<T> : IItemsContainer<T>
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Header { get; }
		public ReactivePropertySlim<bool> IsExpanded { get; }
		public IEnumerable<T> Contents { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public TileContainer(string header, bool isExpanded, IEnumerable<T> contents)
		{
			Header = header;
			IsExpanded = new ReactivePropertySlim<bool>(isExpanded);
			Contents = contents;
		}
	}
}
