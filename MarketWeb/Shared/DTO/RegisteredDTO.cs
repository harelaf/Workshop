
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class RegisteredDTO
    {
        private String _username;
        private ShoppingCartDTO _shoppingCart;
        private ICollection<AdminMessageToRegisteredDTO> _adminMessages;
        private ICollection<MessageToStoreDTO> _repliedMessages;
        private ICollection<NotifyMessageDTO> _notifications;
        private IDictionary<int, ComplaintDTO> _filedComplaints;
        private DateTime _birthDate;
        public String Username { get { return _username; } set { _username = value; } }
        public ShoppingCartDTO ShoppingCart { get { return _shoppingCart; } set { _shoppingCart = value; } }
        public ICollection<AdminMessageToRegisteredDTO> AdminMessages { get { return _adminMessages; } set { _adminMessages = value; } }
        public ICollection <MessageToStoreDTO> RepliedMessages { get { return _repliedMessages; } set { _repliedMessages = value; } }
        public ICollection<NotifyMessageDTO> NotifyMessages { get { return _notifications; } set { _notifications = value; } }
        public IDictionary<int, ComplaintDTO> FiledComplaints { get { return _filedComplaints; } set { _filedComplaints = value; } }
        public DateTime BirthDate { get { return _birthDate; } set { _birthDate = value; } }

        public RegisteredDTO(string Username, ShoppingCartDTO scDTO, ICollection<AdminMessageToRegisteredDTO> adminMessages, ICollection<NotifyMessageDTO> notifications, ICollection<MessageToStoreDTO> repliedMessages, IDictionary<int, ComplaintDTO> filedComplaints, DateTime dob)
        {
            _username = Username;
            _shoppingCart = scDTO;
            _adminMessages = adminMessages;
            _repliedMessages = repliedMessages;
            _notifications = notifications;
            _filedComplaints = filedComplaints;
            _birthDate = dob;
        }
        public String ToString()
        {
            String result = $"Visitor Name: {_username}\n";
            result += "Birth Date:" + _birthDate.ToString() + "\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
        public int NotificationsCount()
        {
            return _notifications.Count;
        }
    }
}
