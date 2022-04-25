using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class MessageToVisitor
    {
        private String _Username;
        private String _storeName;

        public MessageToVisitor(String _Username, String _storeName)
        {
            _Username = _Username;      
            _storeName = _storeName;
        }

        public String Username => _Username;
        public String StoreName => _storeName;
    }
}
