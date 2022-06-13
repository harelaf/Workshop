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
        [ForeignKey("RegisterdDAL")]
        public string _appointer { get; set; }
        public List<OperationWrapper> _operationsWrappers { get; set; }
        [NotMapped]
        public ICollection<Operation> _operations { get; set; }

        protected SystemRoleDAL(ISet<Operation> operations)
        {
            _operations = operations;
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
        }
        protected SystemRoleDAL(ISet<Operation> operations, string appointer)
        {
            _appointer = appointer;
            _operations = operations;
            _operationsWrappers = new List<OperationWrapper>();
            foreach (Operation op in _operations)
            {
                _operationsWrappers.Add(new OperationWrapper(op));
            }
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

        public SystemAdminDAL() : base(getOps())
        { }

        public SystemAdminDAL(ISet<Operation> operations) : base(operations)
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

        public StoreFounderDAL() : base(getOps())
        {
        }

        public StoreFounderDAL(ISet<Operation> operations) : base(operations)
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
        public StoreOwnerDAL(string appointer) : base(getOps(),appointer)
        {
            _appointer = appointer;
        }

        public StoreOwnerDAL(ISet<Operation> operations, string appointer) : base(operations, appointer)
        {
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
        public StoreManagerDAL(string appointer) : base(getOps(), appointer)
        {        }

        public StoreManagerDAL(ISet<Operation> operations, string appointer) : base(operations, appointer)
        {
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
