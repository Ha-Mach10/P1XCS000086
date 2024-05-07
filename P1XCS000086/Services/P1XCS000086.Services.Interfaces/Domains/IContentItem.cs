using System;
using System.Collections.Generic;
using System.Text;

using Reactive.Bindings;

namespace P1XCS000086.Services.Interfaces.Domains
{
	public interface IContentItem
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Name { get; }
		public string Content { get; }
		public string IconKind { get; }
	}
}
