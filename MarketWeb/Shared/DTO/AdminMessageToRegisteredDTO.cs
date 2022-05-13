using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class AdminMessageToRegisteredDTO
    {
        private String _receiverUsername;
        public String ReceiverUsername => _receiverUsername;

        private String _senderUsername;
        public String senderUsername => _senderUsername;

        private String _title;
        public String Title => _title;

        private String _message;
        public String Message => _message;

        public AdminMessageToRegisteredDTO()
        {
            _receiverUsername = "testReceiverUsername";
            _senderUsername = "testSenderUsername";
            _title = "testTitle";
            _message = "testMessage";
        }
    }
}
