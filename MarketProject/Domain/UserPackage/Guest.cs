using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Guest : User
    {
        private string system_username;
        public string System_username=> system_username;

        public Guest(string system_username)
        {
            this.system_username = system_username;
        }
    }
}
