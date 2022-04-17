using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class StoreManagement
    {
        private Dictionary<String, Store> _stores; //<storeName:String, Store>

        public StoreManagement()
        {
            _stores = new Dictionary<String, Store>();
        }

        public bool OpenNewStore(StoreFounder founder, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (_stores.ContainsKey(storeName))
                return false;
            Store newStore = new Store(storeName, founder, purchasePolicy, discountPolicy);
            _stores[storeName] = newStore;
            return true;
        }

        public String GetStoreInformation(String storeName)
        {
            if (!_stores.ContainsKey(storeName))
                return "Invalid Input: Unknown store name.\n";
            Store store = _stores[storeName];
            return store.GetInformation();
        }
    }
}
