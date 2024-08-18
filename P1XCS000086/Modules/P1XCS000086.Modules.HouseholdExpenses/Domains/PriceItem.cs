using P1XCS000086.Core.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P1XCS000086.Modules.HouseholdExpenses.Domains
{
    public class PriceItem : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
    {
        private static int tabIndex = 0;
        public static Dictionary<string, ReactiveCollection<PriceItem>> PriceItemPair { get; set; } = new();



        private int sumPrice = 0;


        // Properties
        public bool KeepAlive { get; } = false;
        public string Key { get; }
        public bool IsChecked { get; set; } = false;
        public Visibility TextBlockVisibility { get; set; } = Visibility.Visible;
		public Visibility CheckBoxVisibility { get; set; } = Visibility.Collapsed;
        public Visibility ButtonVisibility { get; set; } = Visibility.Collapsed;
        public string Index { get; set; }
        public string ItemText { get; set; }
        public int ItemValue { get; set; }
        public List<string> Units { get; set; } = new()
        {
            "個", "g", "ml", "箱", "缶"
        };
        public int ItemPrice { get; set; } = 0;
        public int ItemCount { get; set; } = 0;
        public int SumPrice
        {
            get => PriceItemSum();
        }

        public string ItemTextTabIndex { get; set; }
		public string ItemValueTabIndex { get; set; }
		public string ItemPriceTabIndex { get; set; }
		public string ItemCountTabIndex { get; set; }

        // Command Properties
		public ReactiveCommandSlim AddCommand { get; }
		public ReactiveCommandSlim<string> DeleteCommand { get; }
        public ReactiveCommandSlim<string> CheckedChangedCommand { get; }



		public PriceItem(IRegionManager regionManager, string keyName, string itemText, Action addAction, Action<string> deleteAction, Action<string> checkedChangedAction)
            :base(regionManager)
        {
            Key = keyName;
            ItemText = itemText;
            

            AddCommand = new();
            AddCommand.Subscribe(addAction).AddTo(_disposables);

            DeleteCommand = new();
            DeleteCommand.Subscribe(param => deleteAction(param)).AddTo(_disposables);

            CheckedChangedCommand = new();
            CheckedChangedCommand.Subscribe(param => checkedChangedAction(param)).AddTo(_disposables);

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
                ++count;

                item.Index = count.ToString();

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
            // 引数のitemよりKeyを取得して指定したKeyに対応するListを取得
            ReactiveCollection<PriceItem> items = PriceItemPair[item.Key];
            items.Remove(item);

            // インデックスを振り直す
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Index = (i+1).ToString();
            }
        }
        public static void ClearItems(string key)
        {
            PriceItemPair[key].Clear();
        }
        public static ReactiveCollection<PriceItem> GetPriceItemList(string key)
        {
            return PriceItemPair[key];
        }



        private void TabIndexIncriment()
        {
            ItemTextTabIndex = (tabIndex + 1).ToString();
			ItemValueTabIndex = (tabIndex + 2).ToString();
			ItemPriceTabIndex = (tabIndex + 3).ToString();
            ItemCountTabIndex = (tabIndex + 4).ToString();

            // 
            tabIndex = tabIndex + 4;
        }
        private int PriceItemSum()
        {
            return ItemPrice * ItemCount;
        }
    }
}
