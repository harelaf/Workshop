using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{
    public abstract class SystemRoleDAL
    {

        internal ISet<Operation> _operations { get; set; }
        [Key]
        internal string _username { get; set; }
        [Key]
        internal string _storeName { get; set; } = "";
        protected SystemRoleDAL(ISet<Operation> operations, string username, string storeName)
        {
            _operations = operations;
            _username = username;
            _storeName = storeName; 
        }
    }
    public class SystemAdminDAL : SystemRoleDAL
    {
        internal IDictionary<int, ComplaintDAL> _receivedComplaints { get; set; }
        public SystemAdminDAL(string username, IDictionary<int, ComplaintDAL> receivedComplaints) : base(getOps(), username, "")
        {
            _receivedComplaints = receivedComplaints;   
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
        
        [Required]
        internal string _appointer { get; set; }

        public StoreFounderDAL(string storeName, string username) : base(getOps(), username, storeName)
        {
            _storeName = storeName;
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
        internal string _appointer { get; set; }

        public StoreOwnerDAL(string storeName, string appointer, string username) : base(getOps(), username, storeName)
        {
            _storeName = storeName;
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
        internal string _appointer { get; set; }

        public StoreManagerDAL(string storeName, string appointer, string username) : base(getOps(), username, storeName)
        {
            _storeName = storeName;
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
