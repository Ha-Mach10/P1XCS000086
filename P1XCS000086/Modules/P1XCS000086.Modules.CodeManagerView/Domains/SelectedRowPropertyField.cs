using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;
using Prism.Navigation.Regions;
using System;	
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.CodeManagerView.Domains
{
	public class SelectedRowPropertyField : RegionViewModelBase, IRegionMemberLifetime
	{
		public bool KeepAlive { get; } = false;

		public string TextBlockValue { get; private set; }
		public string TextBoxValue {  get; private set; }
		public static List<SelectedRowPropertyField> Fields { get; private set; } = new();


		public SelectedRowPropertyField(IRegionManager regionManager, string textBlockValue, string textBoxValue)
			: base(regionManager)
		{
			TextBlockValue = textBlockValue;
			TextBoxValue = textBoxValue;
		}



		public static void AddRangeItem(List<SelectedRowPropertyField> selectedRowPropertyFields)
		{
			Fields.Clear();
			Fields.AddRange(selectedRowPropertyFields);
		}
	}
}
