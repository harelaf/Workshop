using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class StoreManagement
    {
        private ICollection<Store> _stores;

        public Store GetStore(String storeName)
        {
            foreach (Store store in _stores)
            {
                if (store.StoreName == storeName)
                    return store;
            }
            return null;
        }
        public bool IsStoreExist(String storeName)
        {
           return GetStore(storeName) != null;
        }
        public Item ReserveItemFromStore(String storeName, int itemID, int amount)
        {
            Store store = GetStore(storeName);
            return store.ReserveItem(itemID, amount);
        }
        
        
    }
}
