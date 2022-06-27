using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Shared;


namespace MarketWeb.Server.Domain
{
    public class SystemAdmin : SystemRole
    {
        public SystemAdmin(string Username) : base(getOps(), Username, null) {}
        public SystemAdmin(string Username, ISet<Operation> op) : base(op, Username, null) 
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
            roles.Add(Operation.APPOINT_SYSTEM_ADMIN);
            return roles;
        }

    
    }
}
