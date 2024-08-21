using P1XCS000086.Services.Interfaces.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Modules.HouseholdExpenses.Domains
{
    internal class ReceiptItem
    {
        // Fields
        
        private ISqlSelect _select;



        // Properties

        public string ShopName { get; private set; }
        public string ReciptId { get; private set; }



        // Constructor
        
        ReceiptItem(ISqlSelect select)
        {
            _select = select;


        }
    }
}