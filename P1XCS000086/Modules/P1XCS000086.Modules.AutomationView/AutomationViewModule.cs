using P1XCS000086.Modules.AutomationView.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using P1XCS000086.Modules.AutomationView.Views;

namespace P1XCS000086.Modules.AutomationView
{
	public class AutomationViewModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<PixivData>();
		}
	}
}