using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class Guest : Visitor
    {
        private String _token;
        public String Token
        {
            get
            {
                return _token;
            }
        }

        public Guest(string token)
        {
            _token = token;
        }
    }
}
