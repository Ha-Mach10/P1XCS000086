using P1XCS000086.Modules.DirectoryManager.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace P1XCS000086.Modules.DirectoryManager
{
	public class DirectoryManagerModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterDialog<MovieDirectryManagerDialog>();
		}
	}
}