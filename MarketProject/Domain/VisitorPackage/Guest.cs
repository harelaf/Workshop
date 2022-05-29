using System;

namespace MarketProject.Domain
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
