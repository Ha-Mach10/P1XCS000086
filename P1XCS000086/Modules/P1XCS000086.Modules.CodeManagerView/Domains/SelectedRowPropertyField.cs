using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.CodeManagerView.Domains
{
	internal class SelectedRowPropertyField
	{
		// 
		private ISqlSelect _select;
		private Dictionary<string, string> _connectionStrings;
		private string _commonManager;
		private string _manager;



		public string DevNumText { get; private set; } = "開発番号";
		public string DevNameText { get; private set; } = "開発名称";
		public string FrameworkText { get; private set; } = "UIフレームワーク";
		public string CodeNameText { get; private set; } = "コードネーム";
		public string CreatedOnText { get; private set; } = "作成日";
		public string RereasedOnText { get; private set; }
		public string UseApplicationText { get; private set; }
		public string VersionText { get; private set; }
		public string DiversionNumberText { get; private set; }
		public string NewNumberText { get; private set; }
		public string ExplanationText { get; private set; }
		public string SummaryText { get; private set; }
		public string DirFileNameText { get; private set; }
		
		
		public string DevelopNumber { get; }
		public string DevelopName { get; }
		public string Framework { get; }
		public string CodeName { get; }
		public string CreatedOn { get; }
		public string ReleasedOn { get; }
		public string UseApplication { get; }
		public string Version { get; }
		public string DiversionNumber { get; }
		public string NewNumber { get; }
		public string Explanation { get; }
		public string Summary { get; }
		public string DirFileName { get; }



		SelectedRowPropertyField(ICodeRegisterModel model, ISqlSelect select, DataRowView dataRowView)
		{
			// 接続文字列を取得
			_commonManager = model.ConnStrings["common_manager"];
			_manager = model.ConnStrings["manager"];

			_select = select;

			string query = "SELECT * FROM `database_structure` WHERE `type` = 'column' AND `table_name` = 'manager_register_code';";
			List<string> columnNames = _select.SelectedColumnToList(_commonManager, "database_structure", query);

			List<dynamic> dynamics = new List<dynamic>()
			{
				DevNumText,
				DevNameText,
				FrameworkText,
				CodeNameText,
				CreatedOnText,
				RereasedOnText,
				UseApplicationText,
				VersionText,
				DiversionNumberText,
				NewNumberText,
				ExplanationText,
				SummaryText,
				DirFileNameText
			};

			var zipItems = columnNames.Zip(dynamics, (x, y) => new { ColumnName = x, Property = y });
			foreach (dynamic item in zipItems)
			{
				item.Property = item.ColumnName;
			}


			List<dynamic> properties = new List<dynamic>()
			{
				DevelopNumber,
				DevelopName,
				Framework,
				CodeName,
				CreatedOn,
				ReleasedOn,
				UseApplication,
				Version,
				DiversionNumber,
				NewNumber,
				Explanation,
				Summary,
				DirFileName
			};
			var zipProperties = properties.Zip(columnNames, (prop, colName) => new { Property = prop, ColumnName = colName });
			foreach (dynamic item in zipProperties)
			{
				item.Property = ;
			}
		}


		private void SetColumnNames(DataRowView dataRowView)
		{
			string query = "SELECT * FROM `database_structure` WHERE `type` = 'column' AND `table_name` = 'manager_register_code';";
			List<string> columnNames = _select.SelectedColumnToList(_commonManager, "database_structure", query);

			List<dynamic> dynamics = new List<dynamic>()
			{
				DevNumText,
				DevNameText,
				FrameworkText,
				CodeNameText,
				CreatedOnText,
				RereasedOnText,
				UseApplicationText,
				VersionText,
				DiversionNumberText,
				NewNumberText,
				ExplanationText,
				SummaryText,
				DirFileNameText
			};

			var zipItems = columnNames.Zip(dynamics, (x, y) => new { ColumnName = x, Property = y });
			foreach (dynamic item in zipItems)
			{
				item.Property = item.ColumnName;
			}
		}
	}
}
