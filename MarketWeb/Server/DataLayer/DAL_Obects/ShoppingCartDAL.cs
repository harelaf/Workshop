﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ShoppingCartDAL
    {
        [Key]
        internal  int scId { get; set; }
        internal virtual ICollection<ShoppingBasketDAL> _shoppingBaskets { get; set; }

        public ShoppingCartDAL()
        {
            _shoppingBaskets = new List<ShoppingBasketDAL>();
        }

        public ShoppingCartDAL(ICollection<ShoppingBasketDAL> shoppingBaskets)
        {
            _shoppingBaskets = shoppingBaskets;
        }
    }
}
