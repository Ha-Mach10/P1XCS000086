using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using P1XCS000086.Services.Interfaces.Data;
using Reactive.Bindings.Notifiers;

namespace P1XCS000086.Services.Data
{
	public class DTConverter : IDTConveter
	{
		public DTConverter()
		{

		}


		// Public Methods
		public DataTable Convert(List<string> columnNames, object[,] gridObjects)
		{
			DataTable dt = new();

			columnNames.Select(x => dt.Columns.Add(x)).ToArray();

			int secondDimension = gridObjects.Length / columnNames.Count;
			for (int i = 0; i < secondDimension; i++)
			{
				DataRow dr = dt.NewRow();

				int count = 0;
				foreach (var columnName in columnNames)
				{
					dr[columnName] = gridObjects[i, count].ToString();
					count++;
				}

				dt.Rows.Add(dr);
			}

			return dt;
		}
	}
}
