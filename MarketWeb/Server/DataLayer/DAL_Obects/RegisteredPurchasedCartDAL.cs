using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class RegisteredPurchasedCartDAL
    {
        [Key]
        public string userName { get; set; }

        [Required]
        public ICollection<PurchasedCartDAL> _PurchasedCarts { get; set; }

        public RegisteredPurchasedCartDAL(string userName, ICollection<PurchasedCartDAL> purchasedCarts) : this(userName)
        {
            _PurchasedCarts = purchasedCarts;
        }

        public RegisteredPurchasedCartDAL(string username)
        {
            this.userName = username;
            _PurchasedCarts = new List<PurchasedCartDAL>();
        }

        public RegisteredPurchasedCartDAL()
        {
            _PurchasedCarts = new List<PurchasedCartDAL>();
        }
    }
}
