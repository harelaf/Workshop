using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class MessageToUser
    {
        private String _username;
        private String _storeName;

        public MessageToUser(String _username, String _storeName)
        {
            _username = _username;      
            _storeName = _storeName;
        }

        public String Username => _username;
        public String StoreName => _storeName;
    }
}
