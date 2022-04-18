using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class StoreManagement
    {
        private Dictionary<String, Store> _stores; //<storeName:String, Store>

        public StoreManagement()
        {
            _stores = new Dictionary<String, Store>();
        }

        public Store GetStore(String storeName)
        {
            if (_stores.ContainsKey(storeName))
                return _stores[storeName];
            return null;
        }

        public bool IsStoreExist(String storeName)
        {
           return GetStore(storeName) != null;
        }

        public Item ReserveItemFromStore(String storeName, int itemID, int amount)
        {
            if (amount <= 0)
                throw new Exception("can't reserve non-positive amount of item");
            Store store = GetStore(storeName);
            return store.ReserveItem(itemID, amount);
        }

        public Item GetItem(String storeName, int itemID)
        {
            Store store = GetStore(storeName);
            if (store == null)
                throw new Exception("there is no store in system with the givn storeid");
            Item item= store.GetItem(itemID);
            if (item == null)
                throw new Exception("there is no item: "+itemID+" in the given store");
            return item;    
        }

        public void UnreserveItemInStore(String storeName, Item item, int amount_to_add)
        {
            if (amount_to_add <= 0)
                throw new Exception("can't unreserve non-positive amount of item");
            Store store = GetStore(storeName);
            store.UnReserveItem(item, amountToAdd);
        }

        public void OpenNewStore(StoreFounder founder, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (CheckStoreNameExists(storeName))
                throw new Exception($"A store with the name {storeName} already exists in the system.");
            Store newStore = new Store(storeName, founder, purchasePolicy, discountPolicy);
            _stores[storeName] = newStore;
        }

        public bool CheckStoreNameExists(String storeName)
        {
            return _stores.ContainsKey(storeName);
        }

        public String GetStoreInformation(String storeName)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            return store.GetInformation();
        }

        public void RateStore(String username, String storeName, int rating, String review)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            store.RateStore(username, rating, review);
        }

        public void UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            store.UpdateStockQuantityOfItem(itemID, newQuantity);
        }

        public bool isStoreActive(String storeName)
        {
            if (!CheckStoreNameExists(storeName))
                return false;
            Store store = _stores[storeName];
            return store.isActive();
        }

        public void CloseStore(String storeName)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            store.CloseStore();
        }

        public void ReopenStore(String storeName)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            if (store.State != StoreState.Inactive)
                throw new Exception($"Store {storeName} is not inactive.");
            store.ReopenStore();
        }

        public void CloseStorePermanently(String storeName)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            store.CloseStorePermanently();
        }
    }
}
