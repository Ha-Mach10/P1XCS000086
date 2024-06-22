using P1XCS000086.Modules.CodeManagerView.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace P1XCS000086.Modules.CodeManagerView
{
	public class CodeManagerViewModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MasterManager>();
			containerRegistry.RegisterForNavigation<CodeManager>();
			containerRegistry.RegisterForNavigation<CodeRegister>();
		}
	}
}