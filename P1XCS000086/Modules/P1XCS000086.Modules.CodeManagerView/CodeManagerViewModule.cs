using P1XCS000086.Modules.CodeManagerView.ViewModels;
using P1XCS000086.Modules.CodeManagerView.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
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
			// View-ViewModel間ワイヤの明示的な宣言
			// ViewModelLocationProvider.Register<CodeRegister, CodeRegisterViewModel>();

			// 
			containerRegistry.RegisterForNavigation<MasterManager>();
			containerRegistry.RegisterForNavigation<CodeManager>();
			containerRegistry.RegisterForNavigation<CodeRegister>();
			// containerRegistry.RegisterForNavigation<CodeRegisterEsc>();
			containerRegistry.RegisterDialog<VSCreateDialog>();

		}
	}
}