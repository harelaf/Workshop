using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class AdminMessageToRegistered
    {
        private int id { get; set; }
        private String _receiverUsername { get; set; }
        private String _senderUsername { get; set; }
        private String _title { get; set; }
        private String _message { get; set; }

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
