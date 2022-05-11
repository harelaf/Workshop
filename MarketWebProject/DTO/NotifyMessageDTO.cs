using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class NotifyMessageDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _title;
        public String Title => _title;

        private String _message;
        public String Message => _message;

        private String _receiverUsername;
        public String ReceiverUsername => _receiverUsername;

        private int _id;
        public int Id => _id;

        public NotifyMessageDTO()
        {
            _storeName = "testStoreName";
            _title = "testTitle";
            _message = "TestMessage";
            _receiverUsername = "testReceiverUsername";
            _id = 1;
        }
    }
}
