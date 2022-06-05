using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class RegisteredPurchasedCartDAL
    {
        [Key]
        internal string userName { get; set; }

        [Required]
        internal ICollection<PurchasedCartDAL> _PurchasedCarts { get; set; }

        public RegisteredPurchasedCartDAL(string userName, ICollection<ShoppingCartDAL> purchasedCarts) : this(userName)
        {
            _PurchasedCarts = purchasedCarts;
        }

        public RegisteredPurchasedCartDAL(string userName)
        {
            this.userName = userName;
            _PurchasedCarts = new List<ShoppingCartDAL>();
        }
    }
}
