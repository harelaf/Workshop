
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class PurchasedCartDTO
    {
        private DateTime _date;
        private ShoppingCartDTO _shoppingCart;

        public DateTime Date=> _date;
        public ShoppingCartDTO ShoppingCart => _shoppingCart;

        public PurchasedCartDTO()
        {
            _date = DateTime.Now;
            _shoppingCart = new ShoppingCartDTO();
        }

        
        public String ToString()
        {
            String result = $"Purchase of: {_date.ToString("MM / dd / yyyy hh: mm tt")}\n";
            result+= "Purchase Cart:\n"+ _shoppingCart.ToString();
            return result;
        }
    }
}
