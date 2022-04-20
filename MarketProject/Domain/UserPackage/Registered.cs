using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class Registered : User
    {
        private ICollection<MessageToUser> _messagesToUser;
        private String _username;
        public String Username=> _username;
        private String _password;
        private ICollection<SystemRole> _roles;

        public Registered(string username, string password)
        {
            _messagesToUser = new List<MessageToUser>();
            _username = username;
            _password = password;
        }
        public void SendMessage(MessageToUser message)
        {
            _messagesToUser.Add(message);
        }
    }
}
