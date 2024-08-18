using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.HouseholdExpenses
{
	public interface IHEHomeModel
	{
		// Properties
		public List<string> ShopTypeNames { get; }
		public List<string> ShopNames { get; }



		// Public Methods
		public List<string> GetShopNames(string shopTypeName);
	}
}
