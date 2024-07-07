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
	public class MasterManagerViewModel : RegionViewModelBase, INotifyPropertyChanged, IRegionMemberLifetime
	{
		// Fields
		private IRegionManager _regionManager;
		private IMasterManagerModel _model;


		// Properties

		public bool KeepAlive { get; private set; }
		public ReactivePropertySlim<List<string>> DatabaseNames { get; }
		public ReactivePropertySlim<string> SelectedDatabaseName { get; }
		public ReactivePropertySlim<List<TableNameListItem>> TableNames { get; }
		public ReactivePropertySlim<TableNameListItem> SelectedTableName { get; }

		public ReactivePropertySlim<DataTable> Table { get; }



		public MasterManagerViewModel(IRegionManager regionManager, IMasterManagerModel model)
			: base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;

			// このビューモデルの生存
			KeepAlive = true;

			// Properties
			DatabaseNames = new ReactivePropertySlim<List<string>>(GenerateDatabaseNames().ToList()).AddTo(_disposables);
			SelectedDatabaseName = new ReactivePropertySlim<string>(string.Empty);
			TableNames = new ReactivePropertySlim<List<TableNameListItem>>().AddTo(_disposables);
			SelectedTableName = new ReactivePropertySlim<TableNameListItem>().AddTo(_disposables);

			Table = new ReactivePropertySlim<DataTable>().AddTo(_disposables);

			// Commands
			DatabaseNameSelectionChanged = new ReactiveCommandSlim();
			DatabaseNameSelectionChanged.Subscribe(OnDatabaseNameSelectionChanged).AddTo(_disposables);
			ListViewSelectionChanged = new ReactiveCommandSlim();
			ListViewSelectionChanged.Subscribe(OnListViewSelectionChanged).AddTo(_disposables);
			LangTypeSelectionChanged = new ReactiveCommandSlim();
			LangTypeSelectionChanged.Subscribe(OnLangTypeSelectionChanged).AddTo(_disposables);
			DevTypeSelectionChanged = new ReactiveCommandSlim();
			DevTypeSelectionChanged.Subscribe(OnDevTypeSelectionChanged).AddTo(_disposables);
		}



		// Commands

		public ReactiveCommandSlim DatabaseNameSelectionChanged { get; }
		private void OnDatabaseNameSelectionChanged()
		{
			TableNames.Value = GenerateTableNameListItems().ToList();
		}
		public ReactiveCommandSlim ListViewSelectionChanged { get; }
		private void OnListViewSelectionChanged()
		{
			if (SelectedTableName.Value is null)
			{
				Table.Value = null;
				return;
			}
			Table.Value = _model.SearchTable(SelectedDatabaseName.Value, SelectedTableName.Value.TableName);
		}
		public ReactiveCommandSlim LangTypeSelectionChanged { get; }
		private void OnLangTypeSelectionChanged()
		{

		}
		public ReactiveCommandSlim DevTypeSelectionChanged { get; }
		private void OnDevTypeSelectionChanged()
		{

		}



		// Private Methods
		
		private IEnumerable<string> GenerateDatabaseNames()
		{
			foreach(var item in _model.DatabaseNames)
			{
				yield return item;
			}
		}
		private IEnumerable<TableNameListItem> GenerateTableNameListItems()
		{
			foreach (var item in _model.GetTableNameSets(SelectedDatabaseName.Value))
			{
				yield return new TableNameListItem(item.Item1, item.Item2);
			}
		}
	}
}
