using P1XCS000086.Core;
using P1XCS000086.Core.Mvvm;
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
using System;


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
		public ReactivePropertySlim<List<ITabButton>> TabButtons
		{
			get => _mergeModel.TabButtons;
		}
		public ReactivePropertySlim<ITabButton> SelectedButton { get; }




		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		/// <param name="regionManager"></param>
		public MainWindowViewModel(IRegionManager regionManager, IMainWindowModel model, IMergeModel mergeModel)
			: base(regionManager)
		{
			// インジェクションされたモデルを設定
			_regionManager = regionManager;
			_model = model;
			_mergeModel = mergeModel;


			// Properties
			SelectedButton = new ReactivePropertySlim<ITabButton>();

			// Commands
			GoHome = new ReactiveCommandSlim();
			GoHome.Subscribe(OnGoHome).AddTo(_disposables);
			SelectionChanged = new ReactiveCommandSlim();
			SelectionChanged.Subscribe(OnSelectionChanged).AddTo(_disposables);


			// Transition
			_regionManager.RegisterViewWithRegion<Home>(m_regionName);
		}



		// ****************************************************************************
		// Command
		// ****************************************************************************

		public ReactiveCommandSlim GoHome { get; }
		private void OnGoHome()
		{
			SelectedButton.Value = null;

            _regionManager.RequestNavigate(m_regionName, nameof(Home));
		}
		public ReactiveCommandSlim SelectionChanged { get; }
		private void OnSelectionChanged()
		{
			if (TabButtons.Value is null || TabButtons is null)
			{
				SelectedButton.Value = null;
			}
			if (SelectedButton.Value is null)
			{
				_regionManager.RequestNavigate(m_regionName, "Home");
				return;
			}
			_regionManager.RequestNavigate(m_regionName, SelectedButton.Value.ViewName);
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************


	}
}
