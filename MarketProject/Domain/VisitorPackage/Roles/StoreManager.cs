using System;
using System.Collections.Generic;
using System.Text;
using MarketProject.Domain;

namespace MarketProject.Domain
{
    public class StoreManager : SystemRole
    {
        private string _appointer;
        public string Appointer => _appointer;

        public StoreManager(string Username, string storeName, string appointer) : base(getOps(), Username)
        {
            StoreName = storeName;
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
            if (!base.StoreName.Equals(storeName))
                throw new Exception("the store name is incorrect.");
            if (!_appointer.Equals(grantor))
                throw new Exception("store-manager permissions can be changed by appointer only.");

            List<Operation> optOps = new List<Operation>(){  //optional operations
                Operation.MANAGE_INVENTORY,
                Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY,
                Operation.DEFINE_CONCISTENCY_CONSTRAINT,
                Operation.STORE_WORKERS_INFO
            };
            if (!optOps.Contains(op))
                throw new Exception("the operation you wish to permit is illegal for store manager.");
            operations.Add(op); 
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
            if (!base.StoreName.Equals(storeName))
                throw new Exception("the store name is incorrect.");
            if (!_appointer.Equals(denier))
                throw new Exception("store-manager permissions can be changed by appointer only."); 
            operations.Remove(op);
            return true;
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
    }
}
