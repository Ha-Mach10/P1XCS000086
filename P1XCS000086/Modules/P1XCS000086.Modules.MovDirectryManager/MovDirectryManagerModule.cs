using P1XCS000086.Modules.MovDirectryManager.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;

namespace P1XCS000086.Modules.MovDirectryManager
{
	public class MovDirectryManagerModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MovieDirectryManager>();
		}
	}
}