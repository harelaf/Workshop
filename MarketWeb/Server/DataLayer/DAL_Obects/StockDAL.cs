﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class StockDAL
    {
        [Key]
        public int id { get; set; }
        [Required]
        public ICollection<StockItemDAL> _itemAndAmount { get; set; }

        public StockDAL(ICollection<StockItemDAL> itemAndAmount)
        {
            _itemAndAmount = itemAndAmount;
        }

        public StockDAL()
        {
            _itemAndAmount = new List<StockItemDAL>();
        }
    }
}