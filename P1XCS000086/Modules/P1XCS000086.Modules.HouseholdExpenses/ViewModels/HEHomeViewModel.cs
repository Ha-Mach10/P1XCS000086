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
using System.Windows.Media;

namespace P1XCS000086.Modules.HouseholdExpenses.ViewModels
{
	public class HEHomeViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// ****************************************************************************
		// Const Members
		// ****************************************************************************

		private const string c_priceItemKey = "PriceItemRegist";



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		private IRegionManager _regionManager;
		private int _priceItemsCount;
		private bool alreadyChecked = false;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public bool KeepAlive { get; private set; } = true;
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
		#nullable enable
		public ReactivePropertySlim<bool?> IsHeaderCheckBoxChecked { get; }
		public ReactiveCollection<PriceItem> PriceItems { get; }
		public ReactiveCollection<PriceItem> ClonePI { get; }
		public ReactivePropertySlim<int> SumPrice { get; }
		// public ReactivePropertySlim<CheckBox> CheckBox { get; }



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
			IsHeaderCheckBoxChecked = new ReactivePropertySlim<bool?>(false);
            PriceItems = new ReactiveCollection<PriceItem>().AddTo(_disposables);
			ClonePI = new ReactiveCollection<PriceItem>().AddTo(_disposables);
			SumPrice = new ReactivePropertySlim<int>(0);

			/*
			// UI要素を定義
			TextBlock textBlock = new TextBlock()
			{
				Text = "通し番号",
				FontSize = 14,
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left,
				Foreground = new SolidColorBrush(Colors.White),
				Margin = new Thickness(0,4,0,4),
				
			};
			CheckBox checkBox = new CheckBox()
			{
				IsThreeState = true,
				Content = textBlock,
				Command = HeaderCheckBoxChecked,
				IsChecked = false,

			};

			CheckBox = new ReactivePropertySlim<CheckBox>(checkBox).AddTo(_disposables);
			*/

			PriceItem.AddItem(new PriceItem(regionManager, c_priceItemKey, "", OnPriceItemBoxKeyDown, OnDeleteItem, OnCheckedChangedItem));
			ReFleshPriceItems();


            // 
            ReceiptRegist = new();
			ReceiptRegist.Subscribe(OnRegistReceipt).AddTo(_disposables);

			HeaderCheckBoxChecked = new();
			HeaderCheckBoxChecked.Subscribe(OnHeaderCheckBoxChecked).AddTo(_disposables);

			Delete = new();
			Delete.Subscribe(OnDelete).AddTo(_disposables);

			Reload = new();
			Reload.Subscribe(OnReload).AddTo(_disposables);

			FixPrises = new();
			FixPrises.Subscribe(OnFixPrises).AddTo(_disposables);

			CancelPrices = new();
			CancelPrices.Subscribe(OnCancelPrices).AddTo(_disposables);
		}



		// ****************************************************************************
		// Commands
		// ****************************************************************************

		public ReactiveCommandSlim ShopNameSelectionChanged { get; }
		private void OnShopNameSelectionChanged()
		{
			ReFleshPriceItems();
		}

		public ReactiveCommandSlim ReceiptRegist { get; }
		private void OnRegistReceipt()
		{
			IsPaneOpen.Value = true;
			PaneLength.Value = 1200;
		}
		public ReactiveCommand HeaderCheckBoxChecked { get; }
		private void OnHeaderCheckBoxChecked()
		{
			if (alreadyChecked is true)
			{
				IsHeaderCheckBoxChecked.Value = false;
			}

			foreach (var item in PriceItems)
			{
				item.IsChecked = (bool)IsHeaderCheckBoxChecked.Value;
				if (item == PriceItems.Last())
				{
					item.IsChecked = false;
				}
			}

			ReFleshPriceItems();

			if (IsHeaderCheckBoxChecked.Value == true &&
				alreadyChecked is true)
			{
				alreadyChecked = false;
			}
			else if (IsHeaderCheckBoxChecked.Value == false &&
				alreadyChecked is true)
			{
				alreadyChecked = false;
			}
			else
			{
				alreadyChecked = true;
			}
		}
		public ReactiveCommandSlim Reload { get; }
		private void OnReload()
		{
			ReFleshPriceItems();
		}
		public ReactiveCommandSlim Delete { get; }
		private void OnDelete()
		{
			foreach (var item in PriceItems)
			{
				if (item.IsChecked is true)
				{
					PriceItem.DeleteItem(item);
				}
			}

			ReFleshPriceItems();
		}
		public ReactiveCommandSlim FixPrises { get; }
		private void OnFixPrises()
		{
			// Paneを閉じる
			ChangeClosePane();
		}
		public ReactiveCommandSlim CancelPrices { get; }
		private void OnCancelPrices()
		{
			// Paneを閉じる
			ChangeClosePane();

			PriceItem.ClearItems(c_priceItemKey);
			PriceItem.AddItem(new PriceItem(_regionManager, c_priceItemKey, "", OnPriceItemBoxKeyDown, OnDeleteItem, OnCheckedChangedItem));
			ReFleshPriceItems();
		}
		/// <summary>
		/// Paneを閉じる動作を表現
		/// </summary>
		private void ChangeClosePane()
		{
			IsPaneOpen.Value = false;
			PaneLength.Value = 300;
		}
		public ReactiveCommandSlim PriceItemBoxKeyDown { get; }
		private void OnPriceItemBoxKeyDown()
		{
			int sameCount = 0;

			foreach (var item in ClonePI.Zip(PriceItems, (a, b) => new { A = a, B = b }))
			{
				bool sameIP = item.A.ItemPrice == item.B.ItemPrice;
				bool sameIC = item.A.ItemCount == item.B.ItemCount;

				if (sameIP || sameIC)
				{
					sameCount++;
				}

				if (sameCount > 0)
				{
					// クローン用コレクションをクリア
					ClonePI.Clear();
					return;
				}
			}

			ClonePI.AddRangeOnScheduler(PriceItems);


			bool isHit = false;

			foreach (PriceItem item in PriceItems)
			{
				// すべてのフィールドが空でないかつ、数量が0より大きいとき
				if (item == PriceItems.Last() &&
					(string.IsNullOrEmpty(item.ItemText) is false || item.ItemPrice > 0 || item.ItemCount > 0))
				{
					// 新しいアイテムを生成し追加
					PriceItem.AddItem(new PriceItem(_regionManager, c_priceItemKey, "", OnPriceItemBoxKeyDown, OnDeleteItem, OnCheckedChangedItem));

					// コレクションをリフレッシュ
					// ReFleshPriceItems();
					isHit = true;

					break;
				}
				else if (item != PriceItems.Last() &&
					(string.IsNullOrEmpty(item.ItemText) && item.ItemPrice == 0 && item.ItemCount == 0))
				{
					// 指定したアイテムを削除
					PriceItem.DeleteItem(item);

					// コレクションをリフレッシュ
					// ReFleshPriceItems();
					isHit = true;

					break;
				}
				else if (item.ItemPrice > 0 && item.ItemCount > 0)
				{
					isHit = true;
				}
			}


			if (isHit)
			{
				// コレクションをリフレッシュ
				ReFleshPriceItems();
			}
		}
		public ReactiveCommandSlim<string> DeleteItem { get; }
		private void OnDeleteItem(string paramIndex)
		{
			int index = int.Parse(paramIndex);

			if (index >= 1)
			{
				PriceItem item = PriceItem.GetPriceItemList(c_priceItemKey).ElementAt(index - 1);
				PriceItem.DeleteItem(item);
				ReFleshPriceItems();
			}
		}
		private void OnCheckedChangedItem(string paramIndex)
		{
			int index = int.Parse(paramIndex);

			int trueCount = 0;
			int falseCount = 0;

			foreach (var item in PriceItem.GetPriceItemList(c_priceItemKey))
			{
				if (item != PriceItems.Last())
				{
					if (item.IsChecked is true)
					{
						trueCount++;
					}
					else if (item.IsChecked is false)
					{
						falseCount++;
					}
				}
			}

			// 全てのチェックボックスがTrueの場合
			if (trueCount > 0 && falseCount == 0)
			{
				IsHeaderCheckBoxChecked.Value = true;
			}
			// 全てのチェックボックスがFalseの場合
			else if (trueCount == 0 && falseCount > 0)
			{
				IsHeaderCheckBoxChecked.Value = false;
			}
			else if (trueCount > 0 && falseCount > 0)
			{
				// CheckBox.Value.IsChecked = null;
				IsHeaderCheckBoxChecked.Value = null;
			}
		}

		private void ReFleshPriceItems()
		{
			// ReactiveCollectionをクリア
			PriceItems.Clear();
			// SumPriceの値を0にする
			SumPrice.Value = 0;

			foreach (var item in PriceItem.GetPriceItemList(c_priceItemKey))
			{
				PriceItems.AddOnScheduler(item);
				SumPrice.Value = SumPrice.Value + item.SumPrice;

				item.TextBlockVisibility = Visibility.Collapsed;
				item.CheckBoxVisibility = Visibility.Visible;
				item.ButtonVisibility = Visibility.Visible;
				if (item == PriceItem.GetPriceItemList(c_priceItemKey).Last())
				{
					item.TextBlockVisibility = Visibility.Visible;
					item.CheckBoxVisibility |= Visibility.Collapsed;
					item.ButtonVisibility = Visibility.Collapsed;
				}
			}
		}
	}
}
