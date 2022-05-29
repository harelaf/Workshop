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
        public MessageToStoreDTO(String storeName, String senderUsername, string title, string message, String reply, String replier, int id)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
            _reply = reply;
            _replier = replier;
            _id = id;
        }
    }
}
