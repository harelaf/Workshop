using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasedCartDAL
    {
        [Key]
        public DateTime _purchaseDate { get; set; }
        [Key]
        [ForeignKey("PurchasedCartDAL")]
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
