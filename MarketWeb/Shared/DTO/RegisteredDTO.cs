using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class RegisteredDTO
    {
        private String _username;
        public String Username => _username;

        private ShoppingCartDTO _shoppingCart;
        public ShoppingCartDTO ShoppingCart => _shoppingCart;

        private ICollection<AdminMessageToRegisteredDTO> _adminMessages;
        public ICollection<AdminMessageToRegisteredDTO> AdminMessages => _adminMessages;

        private ICollection<MessageToStoreDTO> _repliedMessages;
        public ICollection<MessageToStoreDTO> RepliedMessages => _repliedMessages;

        private ICollection<NotifyMessageDTO> _notifications;
        public ICollection<NotifyMessageDTO> NotifyMessages => _notifications;

        public RegisteredDTO()
        {
            _username = "user1";
            _shoppingCart = new ShoppingCartDTO();

            _adminMessages = new List<AdminMessageToRegisteredDTO>();
            _repliedMessages = new List<MessageToStoreDTO>();
            _notifications = new List<NotifyMessageDTO>();
            AdminMessageToRegisteredDTO adminMessage = new AdminMessageToRegisteredDTO();
            MessageToStoreDTO storeMessage = new MessageToStoreDTO();
            MessageToStoreDTO message = new MessageToStoreDTO();
            NotifyMessageDTO notify = new NotifyMessageDTO();
            _adminMessages.Add(adminMessage);
            _repliedMessages.Add(message);
            _notifications.Add(notify);
        }
    }
}
