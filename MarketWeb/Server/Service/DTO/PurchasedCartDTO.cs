using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class PurchasedCartDTO
    {
        private DateTime _date;
        private ShoppingCartDTO _shoppingCart;

        public DateTime Date=> _date;
        public ShoppingCartDTO ShoppingCart => _shoppingCart;

        public PurchasedCartDTO(DateTime date, ShoppingCartDTO cartDTO)
        {
            _date = date;
            _shoppingCart = cartDTO;
        }
        public String ToString()
        {
            String result = $"Purchase of: {_date.ToString("MM / dd / yyyy hh: mm tt")}\n";
            result+= "Purchase Cart:\n"+ _shoppingCart.ToString();
            return result;
        }
    }
}
