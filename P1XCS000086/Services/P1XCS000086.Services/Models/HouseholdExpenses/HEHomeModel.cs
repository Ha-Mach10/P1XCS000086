using System;
using System.Collections.Generic;
using System.Text;

using P1XCS000086.Services.Interfaces.Models.HouseholdExpenses;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.HouseholdExpenses
{
	public class HEHomeModel : IHEHomeModel
	{
		// Fields
		private SqlSelect _select;
		private SqlInsert _insert;
		private SqlUpdate _update;
		private SqlDelete _delete;

		private CommonModel _common = new CommonModel();



		// Properties
		public List<string> ShopTypeNames { get; private set; }
			= new();
		public List<string> ShopNames { get; private set; }
			= new();



		// Constructor
		public HEHomeModel()
		{
			_select = new SqlSelect(_common.ConnStrHouseholdExpensesManager);

			// 
			ShopTypeNames = _select.SelectedColumnToList("shop_type_name", "SELECT `shop_type_name` FROM `master_shop_type`");
		}



		// Public Methods
		public List<string> GetShopNames(string shopTypeName)
		{
			// 店舗形態を取得
			string shopType = _select.GetJustOneSelectedItem("shop_type", $"SELECT `shop_type` FROM `master_shop_type` WHERE `shop_type_name` = '{shopTypeName}';");

			// クエリを生成
			string query = $"SELECT `shop_name` FROM `master_shop` WHERE `shop_type` = '{shopType}';";

			// 生成したクエリから、指定のカラムを取得
			ShopNames = _select.SelectedColumnToList("shop_name", query);

			return ShopNames;
		}
	}
}
