using Org.BouncyCastle.Asn1.Kisa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Data
{
	internal class DataTableCompalator
	{
		private DataTable _dt1;
		private DataTable _dt2;



		public List<(List<string>, List<string>)> Lists { get; private set; }



		internal DataTableCompalator(DataTable dt1, DataTable dt2)
		{
			_dt1 = dt1;
			_dt2 = dt2;

			List<string> columnNames = new();
			foreach (var columnName in _dt1.Columns)
			{
				columnNames.Add(columnName.ToString());
			}

			DataTable bigDt = _dt1.Rows.Count > _dt2.Rows.Count ? _dt1 : _dt2;
			DataTable minDt = _dt1.Rows.Count > _dt2.Rows.Count ? _dt2 : _dt1;

			for (int i = 0; i < (_dt1.Rows.Count > _dt2.Rows.Count ? _dt1.Rows.Count : _dt2.Rows.Count); i++)
			{
				List<string> list1 = new(), list2 = new();

				if (i < minDt.Rows.Count)
				{
					foreach (var columnName in columnNames)
					{
						list1.Add(minDt.Rows[i].ToString());
						list2.Add(bigDt.Rows[i].ToString());
					}

					Lists.Add((list1, list2));
				}
				else if (i < bigDt.Rows.Count)
				{
					foreach (var columnName in columnNames)
					{
						list2.Add(bigDt.Rows[i].ToString());
					}

					Lists.Add((null, list2));
				}
            }
		}

		public DataTable ()
		{

		}
	}
}
