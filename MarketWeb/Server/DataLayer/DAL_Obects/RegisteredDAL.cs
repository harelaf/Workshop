﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{
    public class RegisteredDAL
    {
        [Key]
        public String _username { get; set; }
        public String _password { get; set; }
        public String _salt { get; set; }
        public ShoppingCartDAL _cart { get; set; }
        public DateTime _birthDate { get; set; }
        public ICollection<SystemRoleDAL> _roles { get; set; }
        public ICollection<AdminMessageToRegisteredDAL> _adminMessages { get; set; }
        public ICollection<NotifyMessageDAL> _notifications { get; set; }

        public RegisteredDAL()
        {
            _roles = new List<SystemRoleDAL>();
            _adminMessages = new List<AdminMessageToRegisteredDAL>();
            _notifications = new List<NotifyMessageDAL>();
            _cart = new ShoppingCartDAL();
            // Empty constructor for some reason?
        }

        public RegisteredDAL(string username, string password, string salt, ShoppingCartDAL cart, DateTime birthDate, ICollection<ComplaintDAL> filedComplaints, ICollection<SystemRoleDAL> roles, ICollection<AdminMessageToRegisteredDAL> adminMessages, ICollection<NotifyMessageDAL> notifications, ICollection<MessageToStoreDAL> repliedMessages)
        {
            _username = username;
            _password = password;
            _salt = salt;
            _cart = cart;
            _birthDate = birthDate;
            _roles = roles;
            _adminMessages = adminMessages;
            _notifications = notifications;
        }

        public RegisteredDAL(string username, string password, string salt, DateTime birthDate)
        {
            _username = username;
            _password = password;
            _salt = salt;
            _birthDate = birthDate;
            _cart = new ShoppingCartDAL();
            _roles = new List<SystemRoleDAL>();
            _adminMessages = new List<AdminMessageToRegisteredDAL>();
            _notifications = new List<NotifyMessageDAL>();
        }
    }
}
