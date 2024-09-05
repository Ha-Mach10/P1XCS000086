using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Core;
using P1XCS000086.Services.Interfaces.Models.Automation;
using Prism.Regions;
using Reactive.Bindings.Disposables;
using System.ComponentModel;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Microsoft.Web.WebView2.Wpf;
using MahApps.Metro.Controls;
using P1XCS000086.Modules.AutomationView.Domains;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086.Modules.AutomationView.ViewModels
{
	public class PixivDataViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// *****************************************************************************
		// Fields
		// *****************************************************************************

		private IRegionManager _regionManager;
		private IPixivDataModel _model;
		private ISqlSelect _select;



		// *****************************************************************************
		// Properties
		// *****************************************************************************

		public bool KeepAlive { get; private set; } = true;



		// *****************************************************************************
		// Reactive Properties
		// *****************************************************************************

		public ReactivePropertySlim<WebView2> WebView { get; }
		public ReactivePropertySlim<EventHandler> WebViewEvents { get; }
		public ReactiveCollection<LoginButtonItem> LoginButtons { get; }



		public PixivDataViewModel(IRegionManager regionManager, IPixivDataModel model, ISqlSelect select)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;
			_select = select;


			// 
			WebView = new ReactivePropertySlim<WebView2>(new()).AddTo(_disposables);
			WebViewEvents = new ReactivePropertySlim<EventHandler>().AddTo(_disposables);



			// Commands

			Login = new();
			Login.Subscribe(OnLogin).AddTo(_disposables);
		}



		// *****************************************************************************
		// Reactive Commands
		// *****************************************************************************

		/// <summary>
		/// ログイン用コマンド
		/// </summary>
		public ReactiveCommandSlim Login { get; }
		private async void OnLogin()
		{
			// await _model.PixivLogin();
		}



		// *****************************************************************************
		// Transfer Reactive Commands
		// *****************************************************************************

		private void OnTransitionLoginPage(string commandParamator)
		{
			_regionManager.RequestNavigate(RegionNames.AutomationLoginSendParamaterRegion, commandParamator);
		}



		// *****************************************************************************
		// Private Methods
		// *****************************************************************************

		private IEnumerable<LoginButtonItem> GenerateLoginButtonItems()
		{
			yield return null;
		}
	}
}
