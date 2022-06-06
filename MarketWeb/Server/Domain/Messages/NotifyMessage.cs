using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class NotifyMessage
    {
        private int _id;
        private String _storeName;
        public String StoreName => _storeName;

        private String _title;
        public String Title => _title;

        private String _message;
        public String Message => _message;

        private String _receiverUsername;
        public String ReceiverUsername => _receiverUsername;

        
        public int Id => _id;

        public NotifyMessage(int id,string storeName, string title, string message, string receiverUsername) : this(storeName, title, message, receiverUsername)
        {
            _id = id;
        }

        public NotifyMessage(string storeName, string title, string message, string receiverUsername)
        {
            _storeName = storeName;
            _title = title;
            _message = message;
            _receiverUsername = receiverUsername;
        }
    }
}
