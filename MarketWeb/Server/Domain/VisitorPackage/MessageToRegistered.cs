using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class MessageToRegistered
    {
        private String _username;
        private String _storeName;
        private String _title;
        private String _message;

        public MessageToRegistered(String _Username, String _StoreName, String title, String message)
        {
            _username = _Username;
            _storeName = _StoreName;
            _title = title;
            _message = message; 
        }

        public String Username => _username;
        public String StoreName => _storeName;
        public String Title => _title;
        public String Message => _message;
    }
}
