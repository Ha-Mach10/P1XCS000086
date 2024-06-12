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
	public class CodeRegisterViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// Fields
		private IRegionManager _regionManager;
		private ICodeRegisterModel _model;


		// Properties
		public ReactivePropertySlim<bool> IsPaneOc { get; }
		public ReactivePropertySlim<List<string>> LangTypes { get; }



		public CodeRegisterViewModel(IRegionManager regionManager, ICodeRegisterModel model)
			: base(regionManager)
		{
			// インジェクション
			_regionManager = regionManager;
			_model = model;


			// Properties
			IsPaneOc = new ReactivePropertySlim<bool>(true);
			LangTypes = new ReactivePropertySlim<List<string>>(_model.LangTypes).AddTo(_disposables);

		}
	}
}
