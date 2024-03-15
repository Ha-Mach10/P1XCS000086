using P1XCS000086.Modules.CodeManageMaster.Views;
using P1XCS000086.Modules.CodeManageMaster.ViewModels;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace P1XCS000086.Modules.CodeManageMaster
{
	public class CodeManageMasterModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{
			var regionManager = containerProvider.Resolve<RegionManager>();

			regionManager.RegisterViewWithRegion("CodeManagerField", typeof(CodeManageField));
			regionManager.RegisterViewWithRegion("MasterEditor", typeof(MasterEditor));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<CodeManageMaster>();
		}
	}
}