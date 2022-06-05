using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class StorePurchasedBasketDAL
    {
        [Key]
        internal string _storeName { get; set; }
        [Required]
        internal ICollection<PurchasedBasketDAL> _PurchasedBaskets { get; set; }

        public StorePurchasedBasketDAL(string storeName)
        {
            _storeName = storeName;
            _PurchasedBaskets = new List<ShoppingBasketDAL>();
        }

        public StorePurchasedBasketDAL(string storeName, ICollection<ShoppingBasketDAL> purchasedBaskets) : this(storeName)
        {
            _PurchasedBaskets = purchasedBaskets;
        }
    }
}
