
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class PurchasedCartDTO
    {
        private DateTime _date;
        private ShoppingCartDTO _shoppingCart;

        public DateTime Date=> _date;
        public ShoppingCartDTO ShoppingCart => _shoppingCart;

        
        public String ToString()
        {
            String result = $"Purchase of: {_date.ToString("MM / dd / yyyy hh: mm tt")}\n";
            result+= "Purchase Cart:\n"+ _shoppingCart.ToString();
            return result;
        }
    }
}
