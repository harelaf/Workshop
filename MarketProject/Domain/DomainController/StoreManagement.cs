using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class StoreManagement
    {
        private List<Store> _stores;

        public StoreManagement()
        {
            _stores = new List<Store>();
        }

        public bool openNewStore(StoreFounder founder, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (_stores.Exists(store => store.getName().Equals(storeName)))
                return false;
            Store newStore = new Store(storeName, founder, purchasePolicy, discountPolicy);
            _stores.Add(newStore);
            return true;
        }
    }
}
