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
using P1XCS000086.Modules.HouseholdExpenses.Domains;

namespace P1XCS000086.Modules.HouseholdExpenses.ViewModels
{
	public class HEHomeViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// ****************************************************************************
		// Const Members
		// ****************************************************************************

		



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		private IRegionManager _regionManager;

		private List<PriceItem> priceItems;



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

		public ReactivePropertySlim<bool> IsPaneOpen { get; }
		
		public ReactivePropertySlim<int> PaneLength { get; }
		public ReactivePropertySlim<List<PriceItem>> PriceItems { get; }



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public HEHomeViewModel(IRegionManager regionManager) : base(regionManager)
		{
			_regionManager = regionManager;


			// 初期化
			priceItems = new List<PriceItem>();


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
			PriceItems = new ReactivePropertySlim<List<PriceItem>>(new());


			// priceItems.Add(new PriceItem(regionManager, "", "", 0, OnPriceItemBoxKeyDown));
			PriceItems.Value.Add(new PriceItem(regionManager, "", "", 0, OnPriceItemBoxKeyDown));


			// 
			RegistReceipt = new();
			RegistReceipt.Subscribe(OnRegistReceipt).AddTo(_disposables);
			FixPrises = new();
			FixPrises.Subscribe(OnFixPrises).AddTo(_disposables);
			PriceItemBoxKeyDown = new ReactiveCommand();
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
		public ReactiveCommand PriceItemBoxKeyDown { get; }
		private void OnPriceItemBoxKeyDown()
		{
			var priceItem = PriceItems.Value.Last();

			bool itemTextIsNullOrEmpty = string.IsNullOrEmpty(priceItem.ItemText) is false;
			bool itemPriceIsNullOrEmpty = string.IsNullOrEmpty(priceItem.ItemPrice) is false;
			bool isLargerThanZero = priceItem.ItemCount > 0;

			if (itemTextIsNullOrEmpty || itemPriceIsNullOrEmpty || isLargerThanZero)
			{
				// riceItems.Add(new PriceItem(_regionManager, "", "", 0, OnPriceItemBoxKeyDown));
				// PriceItems.Value = priceItems;
				PriceItems.Value.Add(new PriceItem(_regionManager, "", "", 0, OnPriceItemBoxKeyDown));
			}
		}
	}
}
