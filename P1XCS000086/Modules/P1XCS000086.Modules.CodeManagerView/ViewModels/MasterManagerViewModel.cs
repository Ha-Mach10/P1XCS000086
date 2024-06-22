using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using P1XCS000086.Core.Mvvm;
using System.ComponentModel;
using P1XCS000086.Services.Interfaces.Models.CodeManager;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class MasterManagerViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// Fields
		private IRegionManager _regionManager;
		private IMasterManagerModel _model;


		// Properties
		public ReactivePropertySlim<List<string>> LangTypes { get; }
		public ReactivePropertySlim<List<string>> DevTypes { get; }
		public ReactivePropertySlim<string> SelectedLangType { get; }
		public ReactivePropertySlim<string> SelectedDevType { get; }



		public MasterManagerViewModel(IRegionManager regionManager, IMasterManagerModel model)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;


			// Properties
			LangTypes = new ReactivePropertySlim<List<string>>(_model.LangTypes).AddTo(_disposables);
			DevTypes = new ReactivePropertySlim<List<string>>(_model.DevTypes).AddTo(_disposables);
			SelectedLangType = new ReactivePropertySlim<string>(string.Empty);
			SelectedDevType = new ReactivePropertySlim<string>(string.Empty);


			// Commands
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);
			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);
		}



		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{

		}
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{

		}
	}
}
