
using System;
using System.Collections.Generic;

namespace MarketWeb.Shared.DTO
{
    public class StockDTO
    {
        private Dictionary<int, Tuple<ItemDTO, int>> _itemAndAmount;
        public Dictionary<int, Tuple<ItemDTO, int>> Items => _itemAndAmount;

        public StockDTO(Dictionary<int, Tuple<ItemDTO, int>> itemAndAmount)
        {
            _itemAndAmount = itemAndAmount == null ? new Dictionary<int, Tuple<ItemDTO, int>>() : itemAndAmount; 
        }
    }
}
