using Prism.Regions;
using System;
using System.Reactive.Disposables;

namespace P1XCS000086.Core.Mvvm
{
	public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
	{
		protected IRegionManager RegionManager { get; private set; }

		public RegionViewModelBase(IRegionManager regionManager)
		{
			_disposables = new CompositeDisposable();

			RegionManager = regionManager;
		}

		public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
		{
			continuationCallback(true);
		}

		public virtual bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public virtual void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public virtual void OnNavigatedTo(NavigationContext navigationContext)
		{

		}
	}
}
