using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class RegisteredDTO
    {
        private String _username;
        private ShoppingCartDTO _shoppingCart;
        private ICollection<MessageToRegisteredDTO> _messagesToUsers;

       
        public String ToString()
        {
            String result = $"Visitor Name: {_username}\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
        public int MessagesCount()
        {
            return _messagesToUsers.Count;
        }
    }
}
