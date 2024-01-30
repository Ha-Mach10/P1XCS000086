using P1XCS000086.Modules.ModuleName;
using P1XCS000086.Modules.ViewControls;
using P1XCS000086.Modules.CodeManageRegister;
using P1XCS000086.Modules.CodeManageViewer;
using P1XCS000086.Modules.CodeManageEditor;
using P1XCS000086.Modules.CodeManageMaster;
using P1XCS000086.Services;
using P1XCS000086.Services.Interfaces;
using P1XCS000086.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Models;
using P1XCS000086.Services.Models.CodeManageRegister;
using P1XCS000086.Services.Interfaces.Sql;

namespace P1XCS000086
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
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

			// Sqls
			// # MySql
			containerRegistry.RegisterSingleton<IMySqlConnectionString, SqlConnectionString>();
			containerRegistry.RegisterSingleton<ISqlSelect, SqlSelect>();
			containerRegistry.RegisterSingleton<ISqlInsert, SqlInsert>();
			containerRegistry.RegisterSingleton<ISqlShowTables, SqlShowTables>();
			// # SqlServer


			// Models
			// # CodeManageRegister
			containerRegistry.RegisterSingleton<ICodeManagerRegisterModel, CodeManagerRegisterModel>();
			containerRegistry.RegisterSingleton<IDevelopNumberRegisterModel, DevelopNumberRegisterModel>();
			containerRegistry.RegisterSingleton<IDevelopTypeSelectorModel, DevelopTypeSelectorModel>();

			// 
			containerRegistry.RegisterSingleton<ISqlInsert, SqlInsert>();
			containerRegistry.RegisterSingleton<IJsonConnectionStrings, JsonConnectionStrings>();

		}
		// モジュールカタログの設定
		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<ModuleNameModule>();
			moduleCatalog.AddModule<ViewControlsModule>();

			// コード台帳参照用のモジュール
			moduleCatalog.AddModule<CodeManageViewerModule>();
			// コード台帳登録用のモジュール
			moduleCatalog.AddModule<CodeManageRegisterModule>();
			// コード台帳編集用のモジュール
			moduleCatalog.AddModule<CodeManageEditorModule>();
			// コード台帳マスタ用のモジュール
			moduleCatalog.AddModule<CodeManageMasterModule>();
		}
	}
}
