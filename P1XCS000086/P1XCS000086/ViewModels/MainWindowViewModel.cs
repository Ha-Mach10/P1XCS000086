using P1XCS000086.Core;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Domains;
using P1XCS000086.Modules.HomeView.Views;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.Domains;

using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using P1XCS000086.Modules.CodeManagerView.Views;


namespace P1XCS000086.ViewModels
{
	public class MainWindowViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IRegionManager _regionManager;
		private IMainWindowModel _model;
		private IMergeModel _mergeModel;

		private static string m_regionName = RegionNames.ContentRegion;



		// ****************************************************************************
		// Properties
		// ****************************************************************************
		public string Title { get; } = "Multi Tool";
		/*
		public ReactivePropertySlim<List<ITabButton>> TabButtons
		{
			get => _mergeModel.TabButtons;
		}
		*/
		public ReactivePropertySlim<List<ITabButton>> TabButtons { get; }
		public ReactivePropertySlim<ITabButton> SelectedButton { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		/// <param name="regionManager"></param>
		public MainWindowViewModel(IRegionManager regionManager, IMainWindowModel model, IMergeModel mergeModel) : base(regionManager)
		{
			// インジェクションされたモデルを設定
			_regionManager = regionManager;
			_model = model;
			_mergeModel = mergeModel;

			/*
			// 統合モデルのRegionNameの値を設定（※例外が発生するため）
			_mergeModel.ChangeRegionName(m_regionName);
			*/
			/*
			List<ITabButton> a = new List<ITabButton>()
			{
				new TabButton(nameof(CodeRegister), "コード登録", "TextBoxPlus"),
				new TabButton(nameof(CodeManager), "登録コード一覧", "Table"),
				new TabButton(nameof(CodeEditor), "登録コード編集", "TableEdit")
			};*/
			TabButtons = new ReactivePropertySlim<List<ITabButton>>(_mergeModel.TabButtons.Value);
			// TabButtons.Value = a;

			// Properties
			SelectedButton = new ReactivePropertySlim<ITabButton>();

			// Commands
			GoHome = new ReactiveCommandSlim();
			GoHome.Subscribe(() => OnGoHome()).AddTo(_disposables);
			SelectionChanged = new ReactiveCommandSlim();
			SelectionChanged.Subscribe(() => OnSelectionChanged());


			// Transition
			_regionManager.RegisterViewWithRegion<Home>(m_regionName);
		}



		// ****************************************************************************
		// Command
		// ****************************************************************************

		public ReactiveCommandSlim GoHome { get; }
		private void OnGoHome()
		{
			_regionManager.RequestNavigate(m_regionName, nameof(Home));
		}
		public ReactiveCommandSlim SelectionChanged { get; }
		private void OnSelectionChanged()
		{
			_regionManager.RequestNavigate(m_regionName, SelectedButton.Value.ViewName);
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/*
		private List<TabButton> GenerateTabButtons()
		{
			if (TabButtons.Value is not null)
			{
				TabButtons.Value.Clear();
			}

			// List<ITabButton> ifTabButtons = _mergeModel.TabButtons;

			var tabButton = new ReactivePropertySlim<List<TabButton>>();

			return ifTabButtons.Select(x => new TabButton(_regionManager, x.Header, x.RegionName, x.ViewName)).ToList();
		}
		*/
	}
}
