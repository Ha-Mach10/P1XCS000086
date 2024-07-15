using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Data
{
	public interface IDTConveter
	{
		// Public Methods
		public DataTable Convert(List<string> columnNames, object[,] gridObjects);
	}
}
