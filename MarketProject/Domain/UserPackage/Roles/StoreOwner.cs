using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class StoreOwner : SystemRole
    {
        private Registered _appointer;
        private Store _store;
        public StoreOwner(Store store, Registered appointer) : base(getOps())
        {
            _store = store;
            _appointer = appointer;
        }

        public override bool hasAccess(Store store, Operation op)
        {
            return store.Equals(_store) && hasAccess(op);
        }

        public override bool grantPermission(Operation op, Store store, Registered grantor)
        {
            return false;
        }

        public override bool denyPermission(Operation op, Store store, Registered denier)
        {
            return false;
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
            return roles;
        }
    }
}
