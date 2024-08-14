using P1XCS000086.Core.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using P1XCS000086.Modules.HouseholdExpenses.Domains;
using System.Windows;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace P1XCS000086.Modules.HouseholdExpenses.ViewModels
{
	public class HEHomeViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// ****************************************************************************
		// Const Members
		// ****************************************************************************

		private const string c_priceItemKey = "PriceItemRegist";



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		private IRegionManager _regionManager;

		private List<PriceItem> priceItems = new();



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<int> PaneMaxLength { get; }

		public ReactivePropertySlim<List<string>> ShopNames { get; }
		public ReactivePropertySlim<string> SelectedShopName { get; }

		public ReactivePropertySlim<string> AttendantName { get; }
		public ReactivePropertySlim<string> RegistorNumber { get; }
		public ReactivePropertySlim<string> ReceiptNumber { get; }
		public ReactivePropertySlim<DateTime> IssuedDate { get; }
		public ReactivePropertySlim<string> TotalPrice { get; }
		public ReactivePropertySlim<int> PurchasedCount { get; }

		// 
		public ReactivePropertySlim<bool> IsPaneOpen { get; }
		public ReactivePropertySlim<int> PaneLength { get; }
		public ReactiveCollection<PriceItem> PriceItems { get; }
		/*
		public ObservableCollection<PriceItem> ObPriceItems
		{
			get
			{
				if (PriceItem.PriceItemPair.Count() > 0)
				{
                    return new ObservableCollection<PriceItem>(PriceItem.PriceItemPair[c_priceItemKey]);
                }

				return null;
			}
		}
		*/



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public HEHomeViewModel(IRegionManager regionManager) : base(regionManager)
		{
			_regionManager = regionManager;


			PaneMaxLength = new ReactivePropertySlim<int>(int.MaxValue);

			ShopNames = new ReactivePropertySlim<List<string>>().AddTo(_disposables);
			SelectedShopName = new ReactivePropertySlim<string>(string.Empty);

			AttendantName = new ReactivePropertySlim<string>(string.Empty);
			RegistorNumber = new ReactivePropertySlim<string>(string.Empty);
			ReceiptNumber = new ReactivePropertySlim<string>(string.Empty);
			IssuedDate = new ReactivePropertySlim<DateTime>(DateTime.Now).AddTo(_disposables);
			TotalPrice = new ReactivePropertySlim<string>(string.Empty);
			PurchasedCount = new ReactivePropertySlim<int>(0);

			IsPaneOpen = new ReactivePropertySlim<bool>(false);

			PaneLength = new ReactivePropertySlim<int>(300);
            PriceItems = new ReactiveCollection<PriceItem>();


            priceItems.Add(new PriceItem(regionManager, c_priceItemKey, "", "", 0, OnPriceItemBoxKeyDown));
			PriceItem priceItem = new PriceItem(regionManager, c_priceItemKey, "", "", 0, OnPriceItemBoxKeyDown);
            PriceItem.AddItem(priceItem);
			PriceItems.AddOnScheduler(PriceItem.GetPriceItemList(c_priceItemKey).Last());
            /*
			for (int i = 0; i < 10; i++)
			{
				priceItems.Add(new PriceItem(regionManager, c_priceItemKey, "", "", 0, OnPriceItemBoxKeyDown));
				PriceItem.PriceItemPair[c_priceItemKey].Add(new PriceItem(regionManager, c_priceItemKey, "", "", 0, OnPriceItemBoxKeyDown));
			}
			*/
            // PriceItems.Value = PriceItem.GetPriceItemList(c_priceItemKey);


            // 
            RegistReceipt = new();
			RegistReceipt.Subscribe(OnRegistReceipt).AddTo(_disposables);
			FixPrises = new();
			FixPrises.Subscribe(OnFixPrises).AddTo(_disposables);
			PriceItemBoxKeyDown = new();
			PriceItemBoxKeyDown.Subscribe(OnPriceItemBoxKeyDown).AddTo(_disposables);
		}



		// ****************************************************************************
		// Commands
		// ****************************************************************************

		public ReactiveCommandSlim ShopNameSelectionChanged { get; }
		private void OnShopNameSelectionChanged()
		{

		}

		public ReactiveCommandSlim RegistReceipt { get; }
		private void OnRegistReceipt()
		{
			IsPaneOpen.Value = true;
			PaneLength.Value = 1200;
		}
		public ReactiveCommandSlim FixPrises { get; }
		private void OnFixPrises()
		{
			IsPaneOpen.Value = false;
			PaneLength.Value = 300;
		}
		public ReactiveCommandSlim PriceItemBoxKeyDown { get; }
		private void OnPriceItemBoxKeyDown()
		{
			var priceFieldItem = PriceItems.Last();

			

			int itemsCount = PriceItems.Count;
			PriceItem lastPriceItem = PriceItems.Last();

			// カウンタ変数
			int count = 0;

			foreach (PriceItem item in PriceItems)
			{
				// すべてのフィールドが空でないかつ、数量が0より大きいとき
                if (item == PriceItems.Last() &&
					(string.IsNullOrEmpty(item.ItemText) is false || string.IsNullOrEmpty(item.ItemPrice) is false || item.ItemCount > 0))
                {
                    // 新しいPriceItemを生成して、PriceItemの内部ディクショナリに追加
                    PriceItem priceItem = new PriceItem(_regionManager, c_priceItemKey, "", "", 0, OnPriceItemBoxKeyDown);
                    PriceItem.AddItem(priceItem);

                    // ReactiveCollectionに格納
                    PriceItems.AddOnScheduler(PriceItem.GetPriceItemList(c_priceItemKey).Last());

                    int aa = 0;
					return;
                }

				

				// カウンタインクリメント
				count++;
            }
		}
	}
}
