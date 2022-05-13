
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class StockDTO
    {
        private Dictionary<ItemDTO, int> _itemAndAmount;
        public Dictionary<ItemDTO, int> Items => _itemAndAmount;

        public StockDTO()
        {
            _itemAndAmount = new Dictionary<ItemDTO, int>();
            _itemAndAmount.Add(new ItemDTO("item1", 10.5, "store1"), 20);
            _itemAndAmount.Add(new ItemDTO("item2", 9.5, "store1"), 30);
            _itemAndAmount.Add(new ItemDTO("item3", 15.5, "store1"), 25);
        }
    }
}
