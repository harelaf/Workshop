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
        private ICollection<AdminMessageToRegisteredDTO> _adminMessages;
        private ICollection<MessageToStoreDTO> _repliedMessages;
        private ICollection<NotifyMessageDTO> _notifications;

        public RegisteredDTO(Registered registered)
        {
            _username = registered.Username;
            _shoppingCart = new ShoppingCartDTO(registered.ShoppingCart);
            _adminMessages = new List<AdminMessageToRegisteredDTO>();
            _repliedMessages = new List<MessageToStoreDTO>();
            _notifications = new List<NotifyMessageDTO>();
            foreach (AdminMessageToRegistered msg in registered.AdminMessages)
                _adminMessages.Add(new AdminMessageToRegisteredDTO(msg));
            foreach (MessageToStore msg in registered.messageToStores)
                _repliedMessages.Add(new MessageToStoreDTO(msg));
            foreach (NotifyMessage msg in registered.Notifcations)
                _notifications.Add(new NotifyMessageDTO(msg));
        }
        public String ToString()
        {
            String result = $"Visitor Name: {_username}\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
        public int NotificationsCount()
        {
            return _notifications.Count;
        }
    }
}
