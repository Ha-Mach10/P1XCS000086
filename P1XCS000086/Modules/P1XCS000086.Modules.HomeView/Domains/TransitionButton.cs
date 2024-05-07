using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Domains;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Core;

namespace P1XCS000086.Modules.HomeView.Domains
{
	public class TransitionButton : IContentItem
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Name { get; }
		public string Content { get; }
		public string IconKind { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public TransitionButton(string name, string content, string iconKind, Action<string, string, string> TransitionAction)
		{
			Name = name;
			Content = content;
			IconKind = iconKind;

			ViewTransiton = new ReactiveCommandSlim();
			ViewTransiton.Subscribe(_ => TransitionAction(RegionNames.ContentRegion, Name, Content));
		}


		// ****************************************************************************
		// ReactiveCommand
		// ****************************************************************************

		public ReactiveCommandSlim ViewTransiton { get; }
	}
}
