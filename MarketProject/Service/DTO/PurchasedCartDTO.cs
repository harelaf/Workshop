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
        public String ToString()
        {
            String result = $"Purchase of: {_date.ToString("MM / dd / yyyy hh: mm tt")}\n";
            result+= "Purchase Cart:\n"+ _shoppingCart.ToString();
            return result;
        }
    }
}
