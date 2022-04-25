using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class MessageToRegistered
    {
        private String _username;
        private String _storeName;

        public MessageToRegistered(String _Username, String _storeName)
        {
            _username = _Username;      
            _storeName = _storeName;
        }

        public String Username => _username;
        public String StoreName => _storeName;
    }
}
