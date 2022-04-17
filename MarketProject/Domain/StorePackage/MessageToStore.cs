using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class MessageToStore
    {
        private String _storeName;
        private String _senderUsername;
        private String _title;
        private String _message;

        public MessageToStore(String storeName, String senderUsername, string title, string message)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }
    }
}
