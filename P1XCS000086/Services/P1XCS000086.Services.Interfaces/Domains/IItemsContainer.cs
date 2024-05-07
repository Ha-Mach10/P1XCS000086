using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace P1XCS000086.Services.Interfaces.Domains
{
	public interface IItemsContainer<T>
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Header { get; }
		public ReactivePropertySlim<bool> IsExpanded { get; }
		public IEnumerable<T> Contents { get; }
	}
}
