using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public abstract class SystemRole
    {
        protected ISet<Operation> _operations;
        private string _storeName = null;
        public string StoreName
        {
            get { return _storeName; }
            protected set {
                if (value == null || value.Equals(""))
                    throw new ArgumentNullException("a role must get a unique store name.");
                _storeName = value; 
            }
        }

        public ISet<Operation> operations => _operations;

        public SystemRole(ISet<Operation> operations)
        {
            _operations = operations;
        }

        public abstract bool grantPermission(Operation op, string store, string grantor);

        public abstract bool denyPermission(Operation op, string store, string denier);

        //internal bool hasAccess(Operation op)
        //{
        //    return _operations.Contains(op);
        //}
        public bool hasAccess(string storeName, Operation op)
        {
            return storeName.Equals(_storeName) && _operations.Contains(op);
        }
    }
}
