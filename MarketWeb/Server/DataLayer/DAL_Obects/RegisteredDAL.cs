using System;
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
        [Required]
        [ForeignKey("CartDAL")]
        public ShoppingCartDAL _cart { get; set; }
        public DateTime _birthDate { get; set; }
        [Required]
        public ICollection<AdminMessageToRegisteredDAL> _adminMessages { get; set; }
        [Required]
        public ICollection<NotifyMessageDAL> _notifications { get; set; }

        public PopulationSection _populationSection { get; set; }

        public RegisteredDAL()
        {
            _adminMessages = new List<AdminMessageToRegisteredDAL>();
            _notifications = new List<NotifyMessageDAL>();
            _cart = new ShoppingCartDAL();
            // Empty constructor for some reason?
        }

        public RegisteredDAL(string username, string password, string salt, ShoppingCartDAL cart, DateTime birthDate,
            ICollection<ComplaintDAL> filedComplaints, ICollection<SystemRoleDAL> roles, ICollection<AdminMessageToRegisteredDAL> adminMessages,
            ICollection<NotifyMessageDAL> notifications, ICollection<MessageToStoreDAL> repliedMessages, PopulationSection populationSection)
        {
            _username = username;
            _password = password;
            _salt = salt;
            _cart = cart;
            _birthDate = birthDate;
            _adminMessages = adminMessages;
            _notifications = notifications;
            _populationSection = populationSection;
        }

        public RegisteredDAL(string username, string password, string salt, DateTime birthDate)
        {
            _username = username;
            _password = password;
            _salt = salt;
            _birthDate = birthDate;
            _cart = new ShoppingCartDAL();
            _adminMessages = new List<AdminMessageToRegisteredDAL>();
            _notifications = new List<NotifyMessageDAL>();
            _populationSection = PopulationSection.REGISTERED_NO_ROLES;
        }
    }
}
