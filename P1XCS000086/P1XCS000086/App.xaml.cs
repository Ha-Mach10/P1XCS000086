using P1XCS000086.Modules.CodeManagerView;
using P1XCS000086.Modules.HomeView;
using P1XCS000086.Modules.HouseholdExpenses;
using P1XCS000086.Modules.AutomationView;
using P1XCS000086.Modules.DirectoryManager;
using P1XCS000086.Modules;
using P1XCS000086.Services;
using P1XCS000086.Services.Data;
using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.Interfaces.Data;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.Models.Automation;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Models.HouseholdExpenses;
using P1XCS000086.Services.Models;
using P1XCS000086.Services.Models.Automation;
using P1XCS000086.Services.Models.CodeManager;
using P1XCS000086.Services.Models.HouseholdExpenses;
using P1XCS000086.Views;

using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

using System.Windows;
using P1XCS000086.Modules.CodeManagerView.Views;
using P1XCS000086.Modules.CodeManagerView.ViewModels;
using P1XCS000086.Services.Interfaces.Models.DirectoryManager;
using P1XCS000086.Services.Models.DirectoryManager;

namespace P1XCS000086
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : PrismApplication
	{
		// メインウィンドウを生成
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			// モデルの依存関係をインジェクション
			// 初期
			containerRegistry.RegisterSingleton<IMessageService, MessageService>();

			// Models
			// # Merged
			containerRegistry.RegisterSingleton<IMergeModel, MergeModel>();
			// # MainWindow
			containerRegistry.RegisterSingleton<IMainWindowModel, MainWindowModel>();
			// # Home
			containerRegistry.RegisterSingleton<IHomeModel, HomeModel>();
			// # CodeManageMaster
			containerRegistry.RegisterSingleton<ICodeRegisterModel, CodeRegisterModel>();
			containerRegistry.RegisterSingleton<IMasterManagerModel, MasterManagerModel>();
			containerRegistry.RegisterSingleton<IVSCreateDialogModel, VSCreateDialogModel>();
			// # HouseholdExpenses
			containerRegistry.RegisterSingleton<IHEHomeModel, HEHomeModel>();
			// # Common Models
			containerRegistry.RegisterSingleton<IDTConveter, DTConverter>();
			// # Automation
			containerRegistry.RegisterSingleton<IPixivDataModel, PixivDataModel>();
			// # DirectryManager
			containerRegistry.RegisterSingleton<IMovieDirectryManagerDialogModel, MovieDirectryManagerDialogModel>();


		}
		// モジュールカタログの設定
		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			// Home Module
			moduleCatalog.AddModule<HomeViewModule>();
			// Code Manager
			moduleCatalog.AddModule<CodeManagerViewModule>();
			// 
			moduleCatalog.AddModule<HouseholdExpensesModule>();
			//
			moduleCatalog.AddModule<AutomationViewModule>();
			// 
			moduleCatalog.AddModule<DirectoryManagerModule>();
		}
	}
}
