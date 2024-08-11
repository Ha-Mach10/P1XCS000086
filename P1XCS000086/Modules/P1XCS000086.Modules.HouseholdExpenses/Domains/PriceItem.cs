using P1XCS000086.Core.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.HouseholdExpenses.Domains
{
    public class PriceItem : RegionViewModelBase
    {
        public static int tabIndex = 0;

        public string ItemText { get; set; }
        public string ItemPrice { get; set; }
        public int ItemCount { get; set; }

        public string ItemTextTabIndex { get; set; }
		public string ItemPriceTabIndex { get; set; }
		public string ItemCountTabIndex { get; set; }



		public PriceItem(IRegionManager regionManager, string itemText, string itemPrice, int itemCount, Action action)
            :base(regionManager)
        {
            ItemText = itemText;
            ItemPrice = itemPrice;
            ItemCount = itemCount;
            
            AddCommand = new ReactiveCommandSlim();
            AddCommand.Subscribe(action);


            TabIndexIncriment();
        }



        private void TabIndexIncriment()
        {
            ItemTextTabIndex = (tabIndex + 1).ToString();
            ItemPriceTabIndex = (tabIndex + 2).ToString();
            ItemCountTabIndex = (tabIndex + 3).ToString();

            // 
            tabIndex = tabIndex + 3;
        }



        public ReactiveCommandSlim AddCommand { get; }
    }
}
