﻿using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class StockDTO
    {
        private Dictionary<ItemDTO, int> _itemAndAmount;
        public Dictionary<ItemDTO, int> Items => _itemAndAmount;

        public StockDTO(Dictionary<ItemDTO, int> itemAndAmount)
        {
            _itemAndAmount = itemAndAmount;
        }
    }
}
