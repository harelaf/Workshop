using System;
using System.Collections.Generic;
using MarketWeb.Shared;

namespace MarketWeb.Server.Domain
{
    public class StoreManager : SystemRole
    {
        private string _appointer;
        public string Appointer => _appointer;

        public StoreManager(string Username, string storeName, string appointer) : base(getOps(), Username, storeName)
        {
            _appointer = appointer;
        }

        public StoreManager(string appointer, ISet<Operation> operations, string username, string storeName) : base(operations, username, storeName)
        {
            _appointer = appointer;
        }

        /// <summary>
        /// checks weather the grantor is the appointer of this manager and then grant permission
        /// </summary>
        /// <param name="op"></param>
        /// <param name="grantor">the user </param>
        /// <returns>true when the permission has been granted to this user, even if he was already permitted. false otherwise.</returns>
        public override bool grantPermission(Operation op, string storeName, string grantor)
        {
            if (!StoreName.Equals(storeName))
                throw new Exception("the store name is incorrect.");
            if (!_appointer.Equals(grantor))
                throw new Exception("store-manager permissions can be changed by appointer only.");
            if (op.Equals(Operation.PERMENENT_CLOSE_STORE) || 
                op.Equals(Operation.CANCEL_SUBSCRIPTION) ||
                op.Equals(Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE) ||
                op.Equals(Operation.SYSTEM_STATISTICS) ||
                op.Equals(Operation.APPOINT_SYSTEM_ADMIN))
                throw new Exception("the operation you wish to permit is illegal for store manager.");
            lock (operations)
            {
                if (!operations.Contains(op))
                    operations.Add(op);
            }
            return true;
        }

        /// <summary>
        /// checks weather the denier is the appointer of this manager
        /// </summary>
        /// <param name="op"></param>
        /// <param name="denier"></param>
        /// <returns>true when the permission has been denied from this user, even if he was never permitted. false otherwise.</returns>
        public override bool denyPermission(Operation op, string storeName, string denier)
        {
            if (!_appointer.Equals(denier))
                throw new Exception("store-manager permissions can be changed by appointer only.");
            if (!(op.Equals(Operation.STORE_HISTORY_INFO) || op.Equals(Operation.RECEIVE_AND_REPLY_STORE_MESSAGE)))
                 throw new Exception("manager doesn't have this permission go be removed!");
            bool res = false;
            lock (_operations)
            {
                if (_operations.Contains(op))
                     res = _operations.Remove(op);
            }
            return res;
        }

        private static ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            //roles.Add(Operation.MANAGE_INVENTORY);
            //roles.Add(Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY);
            //roles.Add(Operation.DEFINE_CONCISTENCY_CONSTRAINT);
            //roles.Add(Operation.STORE_WORKERS_INFO);
            roles.Add(Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            roles.Add(Operation.STORE_HISTORY_INFO);
            return roles;
        }

        internal bool isAppointer(string appointerUsername)
        {
            return Appointer.Equals(appointerUsername);
        }
    }
}
