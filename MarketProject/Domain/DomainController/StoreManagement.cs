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
            if (CheckStoreNameExists(storeName))
                return false;
            Store newStore = new Store(storeName, founder, purchasePolicy, discountPolicy);
            _stores[storeName] = newStore;
            return true;
        }

        public bool CheckStoreNameExists(String storeName)
        {
            return _stores.ContainsKey(storeName);
        }

        public String GetStoreInformation(String storeName)
        {
            if (!CheckStoreNameExists(storeName))
                return "Invalid Input: Unknown store name.\n";
            Store store = _stores[storeName];
            return store.GetInformation();
        }

        public bool RateStore(String username, String storeName, int rating, String review)
        {
            if (!CheckStoreNameExists(storeName))
                return false;
            Store store = _stores[storeName];
            return store.RateStore(username, rating, review);
        }
    }
}
