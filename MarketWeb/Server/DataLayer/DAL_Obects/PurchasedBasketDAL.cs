using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasedBasketDAL
    {
        [Key]
        public DateTime _purchaseDate { get; set; }
        [Key]
        public ShoppingBasketDAL _PurchasedBasket { get; set; }

        public PurchasedBasketDAL(DateTime purchaseDate, ShoppingBasketDAL purchasedBasket)
        {
            _purchaseDate = purchaseDate;
            _PurchasedBasket = purchasedBasket;
        }

        public PurchasedBasketDAL()
        {
            // ???
        }
    }
}
