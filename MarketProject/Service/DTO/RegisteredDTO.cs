using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class RegisteredDTO
    {
        private String _userName;
        private ShoppingCartDTO _shoppingCart;

        public RegisteredDTO(Registered registered)
        {
            _userName = registered.Username;
            _shoppingCart = new ShoppingCartDTO(registered.ShoppingCart);
        }
        public String ToString()
        {
            String result = $"User Name: {_userName}\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
    }
}
