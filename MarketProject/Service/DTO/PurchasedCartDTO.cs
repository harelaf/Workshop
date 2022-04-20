using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    internal class PurchasedCartDTO
    {
        private DateTime _date;
        private ShoppingCartDTO _shoppingCart;

        public PurchasedCartDTO(DateTime date, ShoppingCart shoppingCart)
        {
            _date = date;
            _shoppingCart = new ShoppingCartDTO(shoppingCart);
        }
    }
}
