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
        private static int tabIndex = 0;

        public static Dictionary<string, List<PriceItem>> PriceItemPair = new();

        public string Key { get; }

        public int Index { get; set; }
        public string ItemText { get; set; }
        public string ItemPrice { get; set; }
        public int ItemCount { get; set; }

        public string ItemTextTabIndex { get; set; }
		public string ItemPriceTabIndex { get; set; }
		public string ItemCountTabIndex { get; set; }



		public PriceItem(IRegionManager regionManager, string keyName, string itemText, string itemPrice, int itemCount, Action action)
            :base(regionManager)
        {
            Key = keyName;

            ItemText = itemText;
            ItemPrice = itemPrice;
            ItemCount = itemCount;
            
            AddCommand = new ReactiveCommandSlim();
            AddCommand.Subscribe(action);

            // タブインデックスの追加
            TabIndexIncriment();
        }

        public static void AddItem(PriceItem item)
        {
            // Pairの中身がない場合
            if (PriceItemPair.Count == 0)
            {
                // キーと定義したリストを追加
                PriceItemPair.Add(item.Key, new());
            }

            // 任意のキーが含まれている場合
            if (PriceItemPair.ContainsKey(item.Key))
            {
                int count = PriceItemPair[item.Key].Count();
                item.Index = ++count;

                PriceItemPair[item.Key].Add(item);
            }
            else
            {
                // 引数のPriceItemのKeyに含まれていない場合
                // 新しくKeyの設定とPriceItemのリストを宣言
                PriceItemPair.Add(item.Key, new());
                PriceItemPair[item.Key].Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public static void DeleteItem(PriceItem item)
        {
            List<PriceItem> items = PriceItemPair[item.Key];
            items.Remove(item);

            for (int i = 1; i >= items.Count; i++)
            {
                items[i].Index = i;
            }
        }
        public static List<PriceItem> GetPriceItemList(string key)
        {
            return PriceItemPair[key];
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
