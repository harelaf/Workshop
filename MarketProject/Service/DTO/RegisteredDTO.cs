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
        private ICollection<MessageToRegisteredDTO> _messagesToUsers;

        public RegisteredDTO(Registered registered)
        {
            _username = registered.Username;
            _shoppingCart = new ShoppingCartDTO(registered.ShoppingCart);
            _messagesToUsers = new List<MessageToRegisteredDTO>();
            foreach (AdminMessageToRegistered msg in registered.AdminMessages)
                _messagesToUsers.Add(new MessageToRegisteredDTO(msg));
        }
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
