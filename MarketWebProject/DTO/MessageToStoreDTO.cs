
using System;

namespace MarketWebProject.DTO
{
    public class MessageToStoreDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _senderUsername;
        public String SenderUsername => _senderUsername;

        private String _title;
        public String Title => _title;

        private String _message;
        public String Message => _message;

        public MessageToStoreDTO(string storeName, string senderUsername, string title, string message)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }
    }
}
