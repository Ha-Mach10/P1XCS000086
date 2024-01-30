using P1XCS000086.Modules.CodeManageRegister.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace P1XCS000086.Modules.CodeManageRegister
{
	public class CodeManageRegisterModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{
			// 
			var regionManager = containerProvider.Resolve<IRegionManager>();

			// メインビュー
			regionManager.RegisterViewWithRegion("ContentRegion", typeof(CodeManagerRegister));
			// サブビュー
			regionManager.RegisterViewWithRegion("DevelopNumberRegisterRegion", typeof(DevelopNumberRegister));
			regionManager.RegisterViewWithRegion("DevelopTypeSelectorRegion", typeof(DevelopTypeSelector));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			// 開発タイプ選択ビュー
			containerRegistry.RegisterForNavigation<DevelopTypeSelector>();
			// 開発番号登録ビュー
			containerRegistry.RegisterForNavigation<DevelopNumberRegister>();
			// コード管理台帳登録ビュー
			containerRegistry.RegisterForNavigation<CodeManagerRegister>();
		}
	}
}