using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{
    public class RegisteredDAL
    {
        
        [Key]
        internal String _username;
        [Required]
        internal String _password;
        [Required]
        internal String _salt;
        [Required]
        internal ShoppingCartDAL _cart;
        [Required]
        public DateTime _birthDate;
        internal IDictionary<int, ComplaintDAL> _filedComplaints;
        internal ICollection<SystemRoleDAL> _roles;
        internal ICollection<AdminMessageToRegisteredDAL> _adminMessages;
        internal ICollection<NotifyMessageDAL> _notifications;
        internal ICollection<MessageToStoreDAL> _repliedMessages;

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
