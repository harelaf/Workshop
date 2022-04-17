using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class MessageToStore
    {
        private int _storeId;
        private int _userSenderId;
        private String _title;
        private String _message;

        public MessageToStore(int storeId, int userSenderId, string title, string message)
        {
            _storeId = storeId;
            _userSenderId = userSenderId;
            _title = title;
            _message = message;
        }
    }
}
