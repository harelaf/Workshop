﻿using System;
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
        private string _Username;
        public string Username
        {
            get { return _Username; }
            private set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentNullException("a role must get a unique user name.");
                _Username = value;
            }
        }

        public ISet<Operation> operations => _operations;

        public SystemRole(ISet<Operation> operations, string Username)
        {
            _operations = operations;
            Username = Username;
        }

        public virtual bool grantPermission(Operation op, string store, string grantor)
        {
            throw new Exception("only Store Manager can be granted additional permisions.");
        }

        public virtual bool denyPermission(Operation op, string store, string denier)
        {
            throw new Exception("only Store Manager can be denied some permisions.");
        }

        //internal bool hasAccess(Operation op)
        //{
        //    return _operations.Contains(op);
        //}
        public bool hasAccess(string storeName, Operation op)
        {
            if(storeName == _storeName && _operations.Contains(op))
                return true;
            throw new Exception("no permission for this operation.");
        }
    }
}