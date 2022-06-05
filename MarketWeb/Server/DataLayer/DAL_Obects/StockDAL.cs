using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class StockDAL
    {
        internal Dictionary<ItemDAL, int> _itemAndAmount { get; set; }

        public StockDAL(Dictionary<ItemDAL, int> itemAndAmount)
        {
            _itemAndAmount = itemAndAmount;
        }
    }
}
