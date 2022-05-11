using MarketProject.Domain;
using System;

namespace MarketProject.Service.DTO
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

        public MessageToStoreDTO(MessageToStore messageToStore)
        {
            _storeName = messageToStore.StoreName;
            _senderUsername = messageToStore.SenderUsername;
            _title = messageToStore.Title;
            _message = messageToStore.Message;
        }
    }
}
