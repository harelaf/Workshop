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

        private string _reply;
        public String Reply => _reply;

        private string _replier;
        public String Replier => _replier;

        private int _id;
        public int Id => _id;

        public MessageToStoreDTO(MessageToStore messageToStore)
        {
            _storeName = messageToStore.StoreName;
            _senderUsername = messageToStore.SenderUsername;
            _title = messageToStore.Title;
            _message = messageToStore.Message;
            _reply = messageToStore.Reply;
            _replier = messageToStore.Replier;
            _id = messageToStore.Id;
        }
    }
}
