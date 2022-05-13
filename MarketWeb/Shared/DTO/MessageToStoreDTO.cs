
using System;

namespace MarketWeb.Shared.DTO
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

        public MessageToStoreDTO()
        {
            _storeName = "testStoreName";
            _senderUsername = "testSenderUsername";
            _title = "testTitle";
            _message = "testMessage";
            _reply = "testReply";
            _replier = "testReplier";
            _id = 1;
        }
    }
}
