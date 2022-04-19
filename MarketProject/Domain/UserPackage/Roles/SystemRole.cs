using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public abstract class SystemRole
    {
        private Registered _appointer;
        private Store _store;

        public bool hasAccess(Operation op)
        {
            throw new NotImplementedException();
        }
    }
}
