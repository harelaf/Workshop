
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class StockDTO
    {
        private Dictionary<ItemDTO, int> _itemAndAmount;
        public Dictionary<ItemDTO, int> Items => _itemAndAmount;


    }
}
