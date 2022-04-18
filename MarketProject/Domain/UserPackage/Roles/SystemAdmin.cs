using System;
using System.Collections.Generic;
using System.Text;


namespace MarketProject.Domain
{
    class SystemAdmin : SystemRole
    {
        public SystemAdmin() : base(getOps()) {}

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
            roles.Add(Operation.PERMENENT_CLOSE_STORE);
            roles.Add(Operation.CANCEL_SUBSCRIPTION);
            roles.Add(Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE);
            roles.Add(Operation.SYSTEM_STATISTICS);
            roles.Add(Operation.STORE_HISTORY_INFO);
            return roles;
        }

        public override bool hasAccess(Store store, Operation op)
        {
            return hasAccess(op);
        }
    }
}
