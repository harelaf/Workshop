using System.Collections.Generic;
using MarketWeb.Shared;

namespace MarketWeb.Server.Domain
{
    public class StoreOwner : SystemRole
    {
        private string _appointer;
        public string Appointer => _appointer;
        public StoreOwner(string Username, string storeName, string appointer) : base(getOps(), Username, storeName)
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

        internal bool isAppointer(string appointerUsername)
        {
            return Appointer.Equals(appointerUsername);
        }
    }
}
