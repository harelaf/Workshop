using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasedCartDAL
    {
        [Key]
        internal DateTime _purchaseDate { get; set; }
        [Key]
        internal ShoppingCartDAL _PurchasedCart { get; set; }

        public PurchasedCartDAL(DateTime purchaseDate, ShoppingCartDAL purchasedCart)
        {
            _purchaseDate = purchaseDate;
            _PurchasedCart = purchasedCart;
        }
    }
}
