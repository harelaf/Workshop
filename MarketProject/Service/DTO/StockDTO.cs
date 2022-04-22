using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class StockDTO
    {
        private Dictionary<ItemDTO, int> _itemAndAmount;
        public Dictionary<ItemDTO, int> Items => _itemAndAmount;

        public StockDTO(Stock stock)
        {
            _itemAndAmount = new Dictionary<ItemDTO, int>();
            foreach (KeyValuePair<Item, int> entry in stock.Items)
            {
                ItemDTO dto = new ItemDTO(entry.Key);
                _itemAndAmount[dto] = entry.Value;
            }
        }
    }
}
