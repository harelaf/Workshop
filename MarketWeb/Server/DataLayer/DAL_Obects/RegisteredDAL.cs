using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{
    public class RegisteredDAL
    {
        [Key]
        public String _username { get; set; }
        [Required]
        public String _password;
        [Required]
        public String _salt;
        [Required]
        public ShoppingCartDAL _cart;
        [Required]
        public DateTime _birthDate;
        public IDictionary<int, ComplaintDAL> _filedComplaints;
        public ICollection<SystemRoleDAL> _roles;
        public ICollection<AdminMessageToRegisteredDAL> _adminMessages;
        public ICollection<NotifyMessageDAL> _notifications;
        public ICollection<MessageToStoreDAL> _repliedMessages;

        public RegisteredDAL()
        {
            // Empty constructor for some reason?
        }

        public RegisteredDAL(string username, string password, string salt, ShoppingCartDAL cart, DateTime birthDate, IDictionary<int, ComplaintDAL> filedComplaints, ICollection<SystemRoleDAL> roles, ICollection<AdminMessageToRegisteredDAL> adminMessages, ICollection<NotifyMessageDAL> notifications, ICollection<MessageToStoreDAL> repliedMessages)
        {
            _username = username;
            _password = password;
            _salt = salt;
            _cart = cart;
            _birthDate = birthDate;
            _filedComplaints = filedComplaints;
            _roles = roles;
            _adminMessages = adminMessages;
            _notifications = notifications;
            _repliedMessages = repliedMessages;
        }

        public RegisteredDAL(string username, string password, string salt, DateTime birthDate)
        {
            _username = username;
            _password = password;
            _salt = salt;
            _birthDate = birthDate;
            _cart = new ShoppingCartDAL();
            _filedComplaints = new Dictionary<int, ComplaintDAL>();
            _roles = new List<SystemRoleDAL>();
            _adminMessages = new List<AdminMessageToRegisteredDAL>();
            _notifications = new List<NotifyMessageDAL>();
            _repliedMessages = new List<MessageToStoreDAL>(); 
        }
    }
}
