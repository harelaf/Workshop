using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Registered : User
    {
        private ICollection<MessageToUser> _messagesToUser;
    }
}
