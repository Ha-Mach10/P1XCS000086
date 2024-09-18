using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using P1XCS000086.Services.Interfaces.Models.Automation;
using P1XCS000086.Services.Sql;
using P1XCS000086.Services.Sql.MySql;

namespace P1XCS000086.Services.Models.Automation
{
	public class WebAutoMationBaseModel : IWebAutoMationBaseModel
	{
		// *****************************************************************************
		// Fields
		// *****************************************************************************





		// *****************************************************************************
		// Properties
		// *****************************************************************************





		// *****************************************************************************
		// Constructor
		// *****************************************************************************





		// *****************************************************************************
		// Public Methods
		// *****************************************************************************

		public void GetServiceTable()
		{
			SqlSelect select = new SqlSelect(SqlConnectionStrings.ConnectionStrings["account_manager"]);

			string query = $"SELECT * FROM `service`;";
			DataTable dt = select.Select(query);
		}



		// *****************************************************************************
		// Private Methods
		// *****************************************************************************


	}
}
