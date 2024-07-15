﻿using P1XCS000086.Services;
using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Models;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Modules.HomeView;
using P1XCS000086.Modules.CodeManagerView;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Models.CodeManager;

using P1XCS000086.Views;

using Prism.Ioc;
using Prism.Modularity;

using System.Windows;
using Prism.DryIoc;
using P1XCS000086.Modules.HouseholdExpenses;
using P1XCS000086.Services.Interfaces.Data;
using P1XCS000086.Services.Data;

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
			// # Common Models
			containerRegistry.RegisterSingleton<IDTConveter, DTConverter>();

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
		}
	}
}
