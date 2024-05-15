using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P1XCS000086.Services.Domains;
using P1XCS000086.Services.Interfaces.Domains;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Models;
using Reactive.Bindings;

namespace P1XCS000086.Services.Models
{
	public class MergeModel : IMergeModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public ReactivePropertySlim<string> RegionName { get; }
		public ReactivePropertySlim<List<ITabButton>> TabButtons { get; }
		// public List<ITabButton> TabButtons { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public MergeModel()
		{
			RegionName = new ReactivePropertySlim<string>(string.Empty);
			TabButtons = new ReactivePropertySlim<List<ITabButton>>();
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		public void ChangeRegionName(string newRegionName)
		{
			RegionName.Value = newRegionName;
		}
		public void AddTabButtons(ITabButton tabButton)
		{
			// 
			if (TabButtons is null) { return; }
			if (TabButtons.Value is null)
			{
				TabButtons.Value = new List<ITabButton>() { tabButton };
				return;
			}

			var pickTabItem = TabButtons.Value.Where(x => x.Header == tabButton.Header);
			if (TabButtons.Value.Where(x => x.Header == tabButton.Header).Any())
			{
				return;
			}

			// List<ITabButton>を新たに生成し、一度TabButtonsプロパティの値を移す
			List<ITabButton> buttons = new List<ITabButton>();
			buttons.AddRange(TabButtons.Value.Select(x => x).ToList());

			// 追加されたタブボタンをリストに追加
			buttons.Add(tabButton);

			// 追加済みのList<ITabButton>の値を再度セット
			TabButtons.Value = buttons;
		}
		public void RemoveTabButtons(string viewName)
		{
			if (TabButtons.Value.Where(x => x.ViewName == viewName).Any())
			{
				// List<ITabButton>を新たに生成し、一度TabButtonsプロパティの値を移す
				List<ITabButton> buttons = new List<ITabButton>();
				buttons.AddRange(TabButtons.Value);
				
				// 削除コマンドを押されたタブボタンをリストから削除
				ITabButton tabButton = TabButtons.Value.Where(x => x.ViewName == viewName).First();
				buttons.Remove(tabButton);

				// 削除済みのList<ITabButton>の値を再度セット
				TabButtons.Value = buttons;
			}
		}
	}
}
