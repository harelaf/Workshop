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
        internal virtual StoreDAL _store { get; set; }
        internal virtual IDictionary<ItemDAL, int> _items { get; set; }

        public ShoppingBasketDAL(StoreDAL store, IDictionary<ItemDAL, int> items)
        {
            _store = store;
            _items = items;
        }
    }
}
