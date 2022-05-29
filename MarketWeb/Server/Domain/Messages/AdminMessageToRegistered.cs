using System;

namespace MarketWeb.Server.Domain
{
    public class AdminMessageToRegistered
    {
        //private int id { get; set; }
        private String _receiverUsername;
        private String _senderUsername;
        private String _title;
        private String _message;

        public AdminMessageToRegistered(String _Username, String senderUsername, String title, String message)
        {
            _receiverUsername = _Username;
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
