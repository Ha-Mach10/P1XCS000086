using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces.Sql
{
    public interface ISqlSelect
    {
        public DataTable Select();
        public DataTable Select(string command);
        public DataTable Select(string whereColumn, string whereValue);
        public DataTable Select(List<string> columns, List<string> whereValues);
    }
}
