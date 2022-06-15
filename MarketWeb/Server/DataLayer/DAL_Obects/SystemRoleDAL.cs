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
        [Required]
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
        [ForeignKey("RegisterdDAL")]
        public string _username { get; set; }

        [ForeignKey("StoreDAL")]
        public string _storeName { get; set; }

        [ForeignKey("RegisterdDAL")]
        public string _appointer { get; set; }
        [Required]
        public List<OperationWrapper> _operationsWrappers { get; set; }
        [NotMapped]
        public ICollection<Operation> _operations { get; set; }

        protected SystemRoleDAL(string username)
        {
            _username = username;
            _operations = getOps();
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
        }
        protected SystemRoleDAL(string username, string storename)
        {
            _storeName = storename;
            _username = username;
            _operations = getOps();
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
        }
        protected SystemRoleDAL(string appointer, string username, string storename)
        {
            _username=username;
            _appointer = appointer;
            _storeName=storename;
            _operations = getOps();
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
        }
        protected SystemRoleDAL()
        {
            _operations = getOps();
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
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
        protected abstract ISet<Operation> getOps();
    }
    public class SystemAdminDAL : SystemRoleDAL
    {

        public SystemAdminDAL() : base()
        { }

        public SystemAdminDAL(string username) : base(username)
        {
        }

        protected override ISet<Operation> getOps()
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

        public StoreFounderDAL() : base()
        {
        }

        public StoreFounderDAL(string username, string storename) : base(username, storename)
        {
        }

        protected override ISet<Operation> getOps()
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
        public StoreOwnerDAL(string appointer, string username, string storename) : base(appointer, username, storename) { }

        public StoreOwnerDAL() : base()
        {
        }

        protected override ISet<Operation> getOps()
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
        public StoreManagerDAL(string appointer, string username, string storename) : base(appointer, username, storename) { }

        public StoreManagerDAL() : base()
        {
        }
        protected override ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            roles.Add(Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            roles.Add(Operation.STORE_HISTORY_INFO);
            return roles;
        }
    }
}
