using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    abstract class SystemRole
    {
        protected ISet<Operation> _operations;
        public ISet<Operation> operations => _operations;
        

        public SystemRole(ISet<Operation> operations)
        {
            _operations = operations;
        }

        public abstract bool grantPermission(Operation op, Store store, Registered grantor);

        public abstract bool denyPermission(Operation op, Store store, Registered denier);
        
        internal bool hasAccess(Operation op)
        {
            return _operations.Contains(op);
        }

        public abstract bool hasAccess(Store store, Operation op);
    }
}
