using Org.BouncyCastle.X509;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Domains;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;

namespace P1XCS000086.Domains
{
	public class TabButton : ITabButton
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Header { get; }
		public string RegionName { get; }
		public string ViewName { get; }




		// ****************************************************************************
		// Constructor
		// ****************************************************************************
		
		public TabButton(string header, string regionName, string viewName)
		{
			// プロパティの初期化
			Header = header;
			RegionName = regionName;
			ViewName = viewName;

			/*
			// コマンドの初期化・メソッドの購読
			ViewTransiton = new ReactiveCommandSlim();
			ViewTransiton.Subscribe(_ => OnViewTransiton(RegionName, ViewName)).AddTo(_disposables);
			*/
		}

		public void CopyParent(ITabButton tabButton)
		{

		}

		// ****************************************************************************
		// ReactiveCommand
		// ****************************************************************************
		/*
		public ReactiveCommandSlim ViewTransiton { get; }
		private void OnViewTransiton(string regionName, string viewName)
		{
			_regionManager.RequestNavigate(regionName, viewName);
		}
		*/
	}
}
