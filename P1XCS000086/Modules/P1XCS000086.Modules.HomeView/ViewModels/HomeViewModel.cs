using P1XCS000086.Core.Mvvm;
using P1XCS000086.Modules.CodeManagerView.Views;
using P1XCS000086.Modules.HouseholdExpenses.Views;
using P1XCS000086.Modules.HomeView.Domains;
using P1XCS000086.Services.Interfaces.Domains;
using P1XCS000086.Services.Interfaces.Models;

using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using P1XCS000086.Core;


namespace P1XCS000086.Modules.HomeView.ViewModels
{
	public class HomeViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IRegionManager _regionManager;
		private IMergeModel _mergeModel;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReadOnlyReactiveCollection<TileContainer<TransitionButton>> ExpanderPanels { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public HomeViewModel(IRegionManager regionManager, IMergeModel mergeModel) : base(regionManager)
		{
			_regionManager = regionManager;
			_mergeModel = mergeModel;

			// Proeperties
			ObservableCollection<TileContainer<TransitionButton>> itemsContainers = [.. GenerateItemsContainers().ToList()];
			ExpanderPanels = itemsContainers.ToReadOnlyReactiveCollection().AddTo(_disposables);
		}



		// ****************************************************************************
		// ReactiveCommand
		// ****************************************************************************

		/// <summary>
		/// 画面遷移
		/// </summary>
		/// <param name="regionName"></param>
		/// <param name="viewName"></param>
		/// <param name="content"></param>
		private void OnViewTransiton(string regionName, string viewName, string content)
		{
			_mergeModel.AddTabButtons(new TabButton(content, regionName, viewName, OnViewClose));
			_regionManager.RequestNavigate(regionName, viewName);
		}
		/// <summary>
		/// タブクローズ
		/// </summary>
		/// <param name="viewName"></param>
		private void OnViewClose(string viewName)
		{
			if (_mergeModel.TabButtons.Value.Count == 1)
			{
				_regionManager.RequestNavigate(RegionNames.ContentRegion, "Home");
			}

			_mergeModel.RemoveTabButtons(viewName);
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// 各種画面遷移用ボタン格納コンテナ列挙用
		/// </summary>
		/// <returns>コンテナを返す</returns>
		private IEnumerable<TileContainer<TransitionButton>> GenerateItemsContainers()
		{
			yield return new TileContainer<TransitionButton>("コード管理", true, GenerateCodeManagerButtons());
			yield return new TileContainer<TransitionButton>("家計管理", true, GenerateHouseholdExpensesButtons());
		}
		/// <summary>
		/// コード管理用の画面遷移用のボタン列挙用
		/// </summary>
		/// <returns>列挙された遷移用ボタン</returns>
		private IEnumerable<TransitionButton> GenerateCodeManagerButtons()
		{
			yield return new TransitionButton(nameof(CodeRegister), "コード登録", "TextBoxPlus", OnViewTransiton);
			yield return new TransitionButton(nameof(CodeManager), "登録コード一覧", "Table", OnViewTransiton);
			yield return new TransitionButton(nameof(CodeEditor), "登録コード編集", "TableEdit", OnViewTransiton);
		}
		/// <summary>
		/// 家計管理用の画面遷移用のボタン列挙用
		/// </summary>
		/// <returns>列挙された遷移用ボタン</returns>
		private IEnumerable<TransitionButton> GenerateHouseholdExpensesButtons()
		{
			yield return new TransitionButton(nameof(HEHome), "購入品目登録", "TextBoxPlus", OnViewTransiton);
		}
	}
}
