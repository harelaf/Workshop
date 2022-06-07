using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class MessageToStore
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
        
        public MessageToStore(String storeName, String senderUsername, string title, string message, int id)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
            _reply = null;
            _replier = null;
            _id = id;
        }

        public void AnswerMsg(String reply, string replier)
        {
            _replier = replier;
            _reply = reply; 
        }

        public MessageToStore(string storeName, string senderUsername, string title, string message, string reply, string replier, int id)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
            _reply = reply;
            _replier = replier;
            _id = id;
        }

        public bool isClosed()
        {
            return Replier != null;
        }
    }
}
