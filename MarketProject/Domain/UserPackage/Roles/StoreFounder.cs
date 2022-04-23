using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class StoreFounder : SystemRole
    {
        private String _storeName;
        public StoreFounder(string userName, string storeName) : base(getOps(), userName)
        {
            base.StoreName = storeName;
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
}
