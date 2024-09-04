using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Models.Automation;
using Prism.Regions;
using Reactive.Bindings.Disposables;
using System.ComponentModel;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Web.WebView2.Wpf;
using MahApps.Metro.Controls;

namespace P1XCS000086.Modules.AutomationView.ViewModels
{
	public class WebAutoMationBaseViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// *****************************************************************************
		// Fields
		// *****************************************************************************

		private IPixivDataModel _model;



		// *****************************************************************************
		// Properties
		// *****************************************************************************

		public bool KeepAlive { get; private set; } = true;



		// *****************************************************************************
		// Reactive Properties
		// *****************************************************************************

		public ReactivePropertySlim<WebView2> WebView { get; }
		public ReactivePropertySlim<EventHandler> WebViewEvents { get; }



		public WebAutoMationBaseViewModel(IRegionManager regionManager, IPixivDataModel model)
			: base(regionManager)
		{
			_model = model;


			// 
			WebView = new ReactivePropertySlim<WebView2>(new()).AddTo(_disposables);
			WebViewEvents = new ReactivePropertySlim<EventHandler>().AddTo(_disposables);



			// Commands

			Login = new();
			Login.Subscribe(OnLogin).AddTo(_disposables);
		}



		// *****************************************************************************
		// Reactive Properties
		// *****************************************************************************

		/// <summary>
		/// ログイン用コマンド
		/// </summary>
		public ReactiveCommandSlim Login { get; }
		private async void OnLogin()
		{
			await _model.PixivLogin("gunhounan@gmail.com", "S#f-59Xr6q3YY4C");
		}
	}
}
