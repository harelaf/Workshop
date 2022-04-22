using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class MessageToStore
    {
        private String _storeName;
        public String StoreName => _storeName;
        private String _senderUsername;
        public String SenderUsername => _senderUsername;
        private String _title;
        public String Title => _title;
        private String _message;
        public String Message => _message;

        public MessageToStore(String storeName, String senderUsername, string title, string message)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }
    }
}
