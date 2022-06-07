using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class AdminMessageToRegistered
    {
        internal int id { get; set; }
        internal String _receiverUsername { get; set; }
        internal String _senderUsername { get; set; }
        internal String _title { get; set; }
        internal String _message { get; set; }

        public AdminMessageToRegistered(String _Username, String senderUsername, String title, String message)
        {
            _receiverUsername = _Username;
            _senderUsername = senderUsername;
            _title = title;
            _message = message; 
        }

        public AdminMessageToRegistered(int id, string receiverUsername, string senderUsername, string title, string message)
        {
            this.id = id;
            _receiverUsername = receiverUsername;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }

        public String ReceiverUsername => _receiverUsername;
        public String SenderUsername => _senderUsername;
        public String Title => _title;
        public String Message => _message;
    }
}
