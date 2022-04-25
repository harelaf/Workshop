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
        private ICollection<MessageToUserDTO> _messagesToUsers;

        public RegisteredDTO(Registered registered)
        {
            _userName = registered.Username;
            _shoppingCart = new ShoppingCartDTO(registered.ShoppingCart);
            _messagesToUsers = new List<MessageToUserDTO>();
            foreach (MessageToUser msg in registered.MessagesToUser)
                _messagesToUsers.Add(new MessageToUserDTO(msg));
        }
        public String ToString()
        {
            String result = $"User Name: {_userName}\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
        public int MessagesCount()
        {
            return _messagesToUsers.Count;
        }
    }
}
