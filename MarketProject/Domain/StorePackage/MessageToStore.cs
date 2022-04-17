using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class MessageToStore
    {
        private int _storeId;
        private String _senderUsername;
        private String _title;
        private String _message;

        public MessageToStore(int storeId, int senderUsername, string title, string message)
        {
            _storeId = storeId;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }
    }
}
