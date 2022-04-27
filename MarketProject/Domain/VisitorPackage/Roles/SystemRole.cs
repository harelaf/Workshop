using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public abstract class SystemRole
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ISet<Operation> _operations;
        private string _storeName = null;
        public string StoreName
        {
            get { return _storeName; }
            protected set {
                if (value == null || value.Equals(""))
                {
                    String errorMessage = "a role must get a unique store name.";
                    LogErrorMessage("StoreName", errorMessage);
                    throw new ArgumentNullException(errorMessage);
                }
                _storeName = value; 
            }
        }
        private string _Username;
        public string Username
        {
            get { return _Username; }
            protected set
            {
                if (value == null || value.Equals(""))
                {
                    String errorMessage = "a role must get a unique user name.";
                    LogErrorMessage("Username", errorMessage);
                    throw new ArgumentNullException(errorMessage);
                }
                _Username = value;
            }
        }

        public ISet<Operation> operations => _operations;

        public SystemRole(ISet<Operation> operations, string Username)
        {
            _operations = operations;
            this.Username = Username;
        }

        public virtual bool grantPermission(Operation op, string store, string grantor)
        {
            String errorMessage = "only Store Manager can be granted additional permisions.";
            LogErrorMessage("grantPermission", errorMessage);
            throw new Exception(errorMessage);
        }

        public virtual bool denyPermission(Operation op, string store, string denier)
        {
            String errorMessage = "only Store Manager can be denied some permisions.";
            LogErrorMessage("denyPermission", errorMessage);
            throw new Exception(errorMessage);
        }

        public bool hasAccess(string storeName, Operation op)
        {
            String errorMessage;
            if(storeName == _storeName && _operations.Contains(op))
                return true;
            errorMessage = "no permission for this operation.";
            LogErrorMessage("hasAccess", errorMessage);
            throw new Exception(errorMessage);
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in SystemRole.{functionName}. Cause: {message}.");
        }
    }
}
