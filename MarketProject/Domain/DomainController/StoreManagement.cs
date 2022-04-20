﻿using System;
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
        public void EditItemPrice(String storeName, int itemID, double newPrice)
        {
            Item item = GetItem(storeName, itemID);
            item.SetPrice(newPrice);
        }
        public void EditItemName(String storeName, int itemID, int new_price, String newName)
        { 
            if(newName == null)
            {
                throw new ArgumentNullException("Store name mustn't be empty!");
            }
            Item item = GetItem(storeName, itemID); 
            item.SetName(newName);
        }
        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            Item item = GetItem(storeName, itemID);
            item.SetName(newDescription);
        }

        public void RateItem(String username, Item item, int rating, String review)

        {
            item.RateItem(username, rating, review);
        }

        public List<Item> GetItemInformation(String itemName, String itemCategory, String keyWord)
        {
            List<Item> filteredItems = new List<Item>();
            foreach(String storeName in _stores.Keys)
            {
                List<Item> items = _stores[storeName].getItemsByName(itemName);
                foreach(Item item in items)
                {
                    if(itemCategory == null || item.Category == itemCategory)
                    {
                        if(keyWord == null || item.Description == null || item.Category.Contains(keyWord))
                        {
                            filteredItems.Add(item);
                        }
                                            }
                }
            }
            return filteredItems;
        }

        public void SendMessageToStore(String username, String storeName, String title, String message)
        {
            if (!_stores.ContainsKey(storeName))
            {
                throw new Exception("Store " + storeName + " not found");
            }
            Store store = _stores[storeName];
            MessageToStore messageToStore = new MessageToStore(storeName, username, title, message);
            store.AddMessage(messageToStore);
        }

        public void UnreserveItemInStore(String storeName, Item item, int amount_to_add)
        {
            if (amount_to_add <= 0)
                throw new Exception("can't unreserve non-positive amount of item");
            Store store = GetStore(storeName);
            store.UnReserveItem(item, amount_to_add);
        }

        public void OpenNewStore(StoreFounder founder, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
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

        public void AddItemToStoreStock(String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            store.AddItemToStoreStock(itemID, name, price, description, category, quantity);
        }

        public void RemoveItemFromStore(String storeName, int itemID)
        {
            if (!CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            Store store = _stores[storeName];
            store.RemoveItemFromStore(itemID);
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

        public bool AddStoreManager(StoreManager newManager, string storeName)
        {
            Store store = GetStore(storeName);
            if (store == null)
                throw new AccessViolationException("no store by that name.");
            return store.AddStoreManager(newManager);
        }

        public bool AddStoreOwner(StoreOwner newOwner, string storeName)
        {
            Store store = GetStore(storeName);
            if (store == null)
                throw new AccessViolationException("no store by that name.");
            return store.AddStoreOwner(newOwner);
        }

        public bool RemoveStoreOwner(string ownerUsername, string storeName)
        {
            Store store = GetStore(storeName);
            if (store == null)
                return false;
            return store.RemoveStoreOwner(ownerUsername);
        }

        public bool RemoveStoreManager(string managerUsername, string storeName)
        {
            Store store = GetStore(storeName);
            if (store == null)
                return false;
            return store.RemoveStoreManager(managerUsername);
        }
    }
}
