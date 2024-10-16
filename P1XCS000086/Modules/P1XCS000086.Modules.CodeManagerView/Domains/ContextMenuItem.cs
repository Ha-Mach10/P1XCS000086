using P1XCS000086.Core.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P1XCS000086.Modules.CodeManagerView.Domains
{
    public class ContextMenuItem : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
    {
        public bool KeepAlive { get; private set; } = false;

        public string Header { get; set; }
        public ReactiveCommandSlim ItemCommand { get; }
        public bool IsDefaultItem { get; }

        public static List<ContextMenuItem> Items { get; private set; } = new();



        public ContextMenuItem(IRegionManager regionManager, string header, Action action, bool isDefaultItem = false)
            : base(regionManager)
        {
            Header = header;

            ItemCommand = new();
            ItemCommand.Subscribe(action).AddTo(_disposables);

            IsDefaultItem = isDefaultItem;
		}


        public static void AddItem(ContextMenuItem item)
        {
            Items.Add(item);
        }
        public static void AddRangeItems(IEnumerable<ContextMenuItem> items)
        {
            Items.AddRange(items);
        }

        public static void ClearParticalItem()
        {
			var deletedItems = Items.Where(x => x.IsDefaultItem is false).ToList();

            if (Items.Count < 1) return;

			foreach (var item in deletedItems)
			{
				Items.Remove(item);
			}
		}
        public static void ItemsClear()
        {
            Items.Clear();
        }
    }
}
