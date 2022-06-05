using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ShoppingBasketDAL
    {
        [Key]
        internal int sbId { get; set; }
        [Required]
        internal virtual string _storeName { get; set; }
        internal virtual IDictionary<ItemDAL, int> _items { get; set; }

        public ShoppingBasketDAL(string storeName, IDictionary<ItemDAL, int> items)
        {
            _storeName = storeName;
            _items = items;
        }
    }
}
