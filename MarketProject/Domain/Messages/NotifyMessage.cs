using System;

namespace MarketProject.Domain
{
    public class NotifyMessage
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

        public NotifyMessage(string storeName, string title, string message, string receiverUsername)
        {
            _storeName = storeName;
            _title = title;
            _message = message;
            _receiverUsername = receiverUsername;
        }
    }
}
