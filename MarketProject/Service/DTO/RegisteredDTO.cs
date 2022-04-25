using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class RegisteredDTO
    {
        private String _username;
        private ShoppingCartDTO _shoppingCart;

        public RegisteredDTO(Registered registered)
        {
            _username = registered.Username;
            _shoppingCart = new ShoppingCartDTO(registered.ShoppingCart);
        }
        public String ToString()
        {
            String result = $"Visitor Name: {_username}\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
    }
}
