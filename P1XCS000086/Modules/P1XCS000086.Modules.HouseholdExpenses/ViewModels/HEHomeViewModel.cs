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

namespace P1XCS000086.Modules.HouseholdExpenses.ViewModels
{
	public class HEHomeViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		private IRegionManager _regionManager;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<List<string>> ShopNames { get; }
		public ReactivePropertySlim<string> SelectedShopName { get; }

		public ReactivePropertySlim<string> AttendantName { get; }
		public ReactivePropertySlim<string> RegistorNumber { get; }
		public ReactivePropertySlim<string> ReceiptNumber { get; }
		public ReactivePropertySlim<DateTime> IssuedDate { get; }
		public ReactivePropertySlim<string> TotalPrice { get; }
		public ReactivePropertySlim<int> PurchasedCount { get; }



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public HEHomeViewModel(IRegionManager regionManager) : base(regionManager)
		{
			_regionManager = regionManager;


			ShopNames = new ReactivePropertySlim<List<string>>().AddTo(_disposables);
			SelectedShopName = new ReactivePropertySlim<string>(string.Empty);

			AttendantName = new ReactivePropertySlim<string>(string.Empty);
			RegistorNumber = new ReactivePropertySlim<string>(string.Empty);
			ReceiptNumber = new ReactivePropertySlim<string>(string.Empty);
			IssuedDate = new ReactivePropertySlim<DateTime>(DateTime.Now).AddTo(_disposables);
			TotalPrice = new ReactivePropertySlim<string>(string.Empty);
			PurchasedCount = new ReactivePropertySlim<int>(0);
		}



		// ****************************************************************************
		// Commands
		// ****************************************************************************

		public ReactiveCommandSlim ShopNameSelectionChanged { get; }
		private void OnShopNameSelectionChanged()
		{

		}
	}
}
