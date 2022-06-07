using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasedCartDAL
    {
        [Key]
        public DateTime _purchaseDate { get; set; }
        [Key]
        public ShoppingCartDAL _PurchasedCart { get; set; }

        public PurchasedCartDAL(DateTime purchaseDate, ShoppingCartDAL purchasedCart)
        {
            _purchaseDate = purchaseDate;
            _PurchasedCart = purchasedCart;
        }

        public PurchasedCartDAL()
        {
            // Empty constructor for some reason?
        }
    }
}
