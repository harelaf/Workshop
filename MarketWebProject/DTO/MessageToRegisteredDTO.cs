using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class MessageToRegisteredDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _username;
        public String Username => _username;

        private String _title;
        public String Title => _title;
        
        private String _message;
        public String Message => _message;

        public MessageToRegisteredDTO(string storeName, string username, string title, string message)
        {
            _storeName = storeName;
            _username = username;
            _title = title;
            _message = message;
        }
    }
}
