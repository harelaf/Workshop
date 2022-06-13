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
        public String _password { get; set; }
        [Required]
        public String _salt { get; set; }
        [Required]
        public ShoppingCartDAL _cart { get; set; }
        [Required]
        public DateTime _birthDate { get; set; }
        public ICollection<ComplaintDAL> _filedComplaints { get; set; }
        public ICollection<SystemRoleDAL> _roles { get; set; }
        public ICollection<AdminMessageToRegisteredDAL> _adminMessages { get; set; }
        public ICollection<NotifyMessageDAL> _notifications { get; set; }
        public ICollection<MessageToStoreDAL> _repliedMessages  { get; set; }

        public RegisteredDAL()
        {
            // Empty constructor for some reason?
        }

        public RegisteredDAL(string username, string password, string salt, ShoppingCartDAL cart, DateTime birthDate, ICollection<ComplaintDAL> filedComplaints, ICollection<SystemRoleDAL> roles, ICollection<AdminMessageToRegisteredDAL> adminMessages, ICollection<NotifyMessageDAL> notifications, ICollection<MessageToStoreDAL> repliedMessages)
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
            _filedComplaints = new List<ComplaintDAL>();
            _roles = new List<SystemRoleDAL>();
            _adminMessages = new List<AdminMessageToRegisteredDAL>();
            _notifications = new List<NotifyMessageDAL>();
            _repliedMessages = new List<MessageToStoreDAL>(); 
        }
    }
}
