using P1XCS000086.Modules.ModuleName;
using P1XCS000086.Modules.ViewControls;
using P1XCS000086.Services;
using P1XCS000086.Services.Interfaces;
using P1XCS000086.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace P1XCS000086
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IMessageService, MessageService>();
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<ModuleNameModule>();
			moduleCatalog.AddModule<ViewControlsModule>();
		}
	}
}
