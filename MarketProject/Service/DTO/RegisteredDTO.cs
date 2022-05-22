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
        private DateTime _birthDate;
        private DateTime _birthDate;
        private ICollection<MessageToRegisteredDTO> _messagesToUsers;
>>>>>>>>> Temporary merge branch 2
        private DateTime _birthDate;
        private ICollection<MessageToRegisteredDTO> _messagesToUsers;
>>>>>>>>> Temporary merge branch 2
        private DateTime _birthDate;
        private ICollection<MessageToRegisteredDTO> _messagesToUsers;
>>>>>>>>> Temporary merge branch 2

        public RegisteredDTO(string Username, ShoppingCartDTO scDTO, ICollection<AdminMessageToRegisteredDTO> adminMessages, ICollection<NotifyMessageDTO> notifications, ICollection<MessageToStoreDTO> repliedMessages)
        {
            result += "Birth Date:" + _birthDate.ToString() + "\n";
            _username = Username;
            _shoppingCart = scDTO;
            _adminMessages = adminMessages;
            _repliedMessages = repliedMessages;
            _notifications = notifications;
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
