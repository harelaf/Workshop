using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Guest : User
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
