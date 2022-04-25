using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class MessageToUser
    {
        private String _username;
        private String _storeName;
        private String _title;
        private String _message;

        public MessageToUser(String _username, String _storeName, String title, String message)
        {
            _username = _username;
            _storeName = _storeName;
            _title = title;
            _message = message; 
        }

        public String Username => _username;
        public String StoreName => _storeName;
    }
}
