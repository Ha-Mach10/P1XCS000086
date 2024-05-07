using P1XCS000086.Modules.HomeView.Views;

using Prism.Ioc;
using Prism.Modularity;

namespace P1XCS000086.Modules.HomeView
{
	public class HomeViewModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		/// <summary>
		/// ビューの型を登録
		/// </summary>
		/// <param name="containerRegistry"></param>
		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			// ビューを登録
			containerRegistry.RegisterForNavigation<Home>();
		}
	}
}