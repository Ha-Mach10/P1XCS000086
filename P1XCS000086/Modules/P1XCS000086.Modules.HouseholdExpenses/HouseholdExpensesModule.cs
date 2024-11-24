using P1XCS000086.Modules.HouseholdExpenses.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;

namespace P1XCS000086.Modules.HouseholdExpenses
{
	public class HouseholdExpensesModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<HEHome>();
		}
	}
}