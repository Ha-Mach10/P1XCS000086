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
using System.Data;
using P1XCS000086.Modules.CodeManagerView.Domains;

namespace P1XCS000086.Modules.CodeManagerView.ViewModels
{
	public class MasterManagerViewModel : RegionViewModelBase, INotifyPropertyChanged
	{
		// Fields
		private IRegionManager _regionManager;
		private IMasterManagerModel _model;


		// Properties
		public ReactivePropertySlim<List<string>> TableNames { get; }

		public ReactivePropertySlim<DataTable> Table { get; }



		public MasterManagerViewModel(IRegionManager regionManager, IMasterManagerModel model)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;


			// Properties
			TableNames = new ReactivePropertySlim<List<string>>(_model.TableNames).AddTo(_disposables);


			// Commands
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);
			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);
		}



		// Commands
		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{

		}
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{

		}



		// Private Methods
		private IEnumerable<TableNameListItem> GenerateTableNameListItems()
		{
			_model.TableNames.Select(x =>
			{



				return x;
			}).ToArray();
			yield return new TableNameListItem()
		}
	}
}
