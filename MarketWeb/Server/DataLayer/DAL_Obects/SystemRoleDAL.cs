using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{
    public class OperationWrapper
    {
        [Key]
        public int id { get; set; }
        public Operation op { get; set; }

        public OperationWrapper(Operation op)
        {
            this.op = op;
        }

        public OperationWrapper()
        {
            // ???
        }
    }

    public abstract class SystemRoleDAL
    {
        [Key]
        public int id { get; set; }
        public string _username { get; set; }
        public string _storeName { get; set; } = "";
        public List<OperationWrapper> _operationsWrappers { get; set; }
        [NotMapped]
        public ICollection<Operation> _operations { get; set; }
        protected SystemRoleDAL(ISet<Operation> operations, string username, string storeName)
        {
            _operations = operations;
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
            _username = username;
            _storeName = storeName;
        }
        protected SystemRoleDAL()
        {
            _operationsWrappers = new List<OperationWrapper>();
            _operations = new List<Operation>();
        }
        public ISet<Operation> ConvertToSet()
        {
            ISet<Operation> operations = new HashSet<Operation>();
            foreach (Operation op in _operations)
            {
                operations.Add(op);
            }
            return operations;
        }
    }
    public class SystemAdminDAL : SystemRoleDAL
    {

        public SystemAdminDAL(string username) : base(getOps(), username, "")
        { }

        public SystemAdminDAL(ISet<Operation> operations, string username, string storeName) : base(operations, username, storeName)
        {
        }

        private static ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            roles.Add(Operation.PERMENENT_CLOSE_STORE);
            roles.Add(Operation.CANCEL_SUBSCRIPTION);
            roles.Add(Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE);
            roles.Add(Operation.SYSTEM_STATISTICS);
            roles.Add(Operation.STORE_HISTORY_INFO);
            roles.Add(Operation.STORE_INFORMATION);
            return roles;
        }
    }
    public class StoreFounderDAL : SystemRoleDAL
    {

        public StoreFounderDAL(string storeName, string username) : base(getOps(), username, storeName)
        {
            _storeName = storeName;
        }

        public StoreFounderDAL(ISet<Operation> operations, string username, string storeName) : base(operations, username, storeName)
        {
        }

        private static ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            roles.Add(Operation.MANAGE_INVENTORY);
            roles.Add(Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY);
            roles.Add(Operation.DEFINE_CONCISTENCY_CONSTRAINT);
            roles.Add(Operation.APPOINT_OWNER);
            roles.Add(Operation.REMOVE_OWNER);
            roles.Add(Operation.APPOINT_MANAGER);
            roles.Add(Operation.REMOVE_MANAGER);
            roles.Add(Operation.CHANGE_MANAGER_PREMISSIONS);
            roles.Add(Operation.CLOSE_STORE);
            roles.Add(Operation.REOPEN_STORE);
            roles.Add(Operation.STORE_WORKERS_INFO);
            roles.Add(Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            roles.Add(Operation.STORE_HISTORY_INFO);
            roles.Add(Operation.STORE_INFORMATION);
            roles.Add(Operation.STOCK_EDITOR);
            return roles;
        }
    }
    public class StoreOwnerDAL : SystemRoleDAL
    {

        [Required]
        public string _appointer { get; set; }

        public StoreOwnerDAL(string storeName, string appointer, string username) : base(getOps(), username, storeName)
        {
            _appointer = appointer;
        }

        public StoreOwnerDAL(ISet<Operation> operations, string username, string storeName, string appointer) : base(operations, username, storeName)
        {
            _appointer = appointer;
        }

        private static ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            roles.Add(Operation.MANAGE_INVENTORY);
            roles.Add(Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY);
            roles.Add(Operation.APPOINT_OWNER);
            roles.Add(Operation.REMOVE_OWNER);
            roles.Add(Operation.APPOINT_MANAGER);
            roles.Add(Operation.REMOVE_MANAGER);
            roles.Add(Operation.CHANGE_MANAGER_PREMISSIONS);
            roles.Add(Operation.STORE_WORKERS_INFO);
            roles.Add(Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            roles.Add(Operation.STORE_HISTORY_INFO);
            roles.Add(Operation.STORE_INFORMATION);
            roles.Add(Operation.STOCK_EDITOR);
            return roles;
        }
    }
    public class StoreManagerDAL : SystemRoleDAL
    {
        [Required]
        public string _appointer { get; set; }

        public StoreManagerDAL(string storeName, string appointer, string username) : base(getOps(), username, storeName)
        {
            _appointer = appointer;
        }

        public StoreManagerDAL(ISet<Operation> operations, string username, string storeName, string appointer) : base(operations, username, storeName)
        {
            _appointer = appointer;
        }
        private static ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            roles.Add(Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            roles.Add(Operation.STORE_HISTORY_INFO);
            return roles;
        }
    }
}
