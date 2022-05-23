using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Shared;

namespace MarketProject.Domain
{
    public class StoreManagement
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<String, Store> _stores; //<storeName:String, Store>

        public StoreManagement()
        {
            _stores = new Dictionary<String, Store>();
        }

        public Store GetStore(String storeName)
        {
            String errorMessage;
            if (_stores.ContainsKey(storeName))
                return _stores[storeName];
            errorMessage = "Store '" + storeName + "' does not exists.";
            LogErrorMessage("GetStore", errorMessage);
            throw new Exception(errorMessage);
        }

        public bool IsStoreExist(String storeName)
        {
           return _stores.ContainsKey(storeName);
        }

        public Item ReserveItemFromStore(String storeName, int itemID, int amount)
        {
            String errorMessage;
            if (amount <= 0)
            {
                errorMessage = "can't reserve non-positive amount of item";
                LogErrorMessage("ReserveItemFromStore", errorMessage);
                throw new Exception(errorMessage);
            }
            Store store = GetStore(storeName);
            return store.ReserveItem(itemID, amount);
        }

        public Item GetItem(String storeName, int itemID)
        {
            String errorMessage;
            Store store = GetStore(storeName);
            if (store == null)
            {
                errorMessage = "there is no store in system with the givn storeid";
                LogErrorMessage("GetItem", errorMessage);
                throw new Exception(errorMessage);
            }
            Item item= store.GetItem(itemID);
            if (item == null)
            {
                errorMessage = "there is no item: " + itemID + " in the given store";
                LogErrorMessage("GetItem", errorMessage);
                throw new Exception(errorMessage);
            }
            return item;    
        }
        public void EditItemPrice(String storeName, int itemID, double newPrice)
        {
            Item item = GetItem(storeName, itemID);
            lock (item)
            {
                item.SetPrice(newPrice);
            }  
        }
        public void EditItemName(String storeName, int itemID, String newName)
        {
            String errorMessage;
            if (newName == null)
            {
                errorMessage = "Store name mustn't be empty!";
                LogErrorMessage("EditItemName", errorMessage);
                throw new ArgumentNullException(errorMessage);
            }
            Item item = GetItem(storeName, itemID); 
            item.SetName(newName);
        }
        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            Item item = GetItem(storeName, itemID);
            item.SetDescription(newDescription);
        }

        public void RateItem(String Username, Item item, int rating, String review)

        {
            item.RateItem(Username, rating, review);
        }

        public IDictionary<string, List<Item>> GetItemInformation(String itemName, String itemCategory, String keyWord)
        {
            Dictionary<string, List<Item>> filteredItems = new Dictionary<string, List<Item>>();
            foreach (String storeName in _stores.Keys)
            {
                List<Item> items = _stores[storeName].getItemsByName(itemName);
                foreach (Item item in items)
                {
                    if (itemCategory != null && itemCategory.Length > 0 && !item.Category.Contains(itemCategory))
                    {
                        if (keyWord != null && keyWord.Length > 0 && item.Description != null && item.Description.Length > 0 && !item.Description.Contains(keyWord))
                        {
                            items.Remove(item);
                        }
                    }
                }
                if (items.Count > 0)
                    filteredItems.Add(storeName, items);
            }
            return filteredItems;
        }

        public void SendMessageToStore(String Username, String storeName, String title, String message, int id)
        {
            Store store = GetStore(storeName);
            MessageToStore messageToStore = new MessageToStore(storeName, Username, title, message, id);
            store.AddMessage(messageToStore);
        }

        public void UnreserveItemInStore(String storeName, Item item, int amount_to_add)
        {
            String errorMessage;
            if (amount_to_add <= 0)
            {
                errorMessage = "can't unreserve non-positive amount of item";
                LogErrorMessage("UnreserveItemInStore", errorMessage);
                throw new Exception(errorMessage);
            }
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

        public Store GetStoreInformation(String storeName)
        {
            String errorMessage;
            if (!CheckStoreNameExists(storeName))
            {
                errorMessage = $"Store {storeName} does not exist.";
                LogErrorMessage("GetStoreInformation", errorMessage);
                throw new Exception(errorMessage);
            }
            Store store = _stores[storeName];
            return store;
        }

        public List<String> GetStoreRolesByName(String storeName)
        {
            Store store = GetStore(storeName);
            return store.GetStoreRolesByName();
        }

        public void RateStore(String Username, String storeName, int rating, String review)
        {
            Store store = GetStore(storeName);
            store.RateStore(Username, rating, review);
        }

        public void UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity)
        {
            Store store = GetStore(storeName);
            store.UpdateStockQuantityOfItem(itemID, newQuantity);
        }

        public bool isStoreActive(String storeName)
        {
            if (!IsStoreExist(storeName))
                return false;
            Store store = GetStore(storeName);
            return store.isActive();
        }

        public void AddItemToStoreStock(String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            Store store = GetStore(storeName);
            store.AddItemToStoreStock(itemID, name, price, description, category, quantity);
        }

        public void RemoveItemFromStore(String storeName, int itemID)
        {
            Store store = GetStore(storeName);
            store.RemoveItemFromStore(itemID);
        }

        public void CloseStore(String storeName)
        {
            Store store = GetStore(storeName);
            store.CloseStore();
        }

        public void ReopenStore(String storeName)
        {
            String errorMessage;
            Store store = GetStore(storeName);
            if (store.State != StoreState.Inactive)
            {
                errorMessage = $"Store {storeName} is not inactive.";
                LogErrorMessage("GetStore", errorMessage);
                throw new Exception(errorMessage);
            }
            store.ReopenStore();
        }

        public void CloseStorePermanently(String storeName)
        {
            Store store = GetStore(storeName);
            store.CloseStorePermanently();
            _stores.Remove(storeName);
        }

        public void AddStoreDiscount(String storeName, Discount discount)
        {
            GetStore(storeName).AddDiscount(discount);
        }

        public bool AddStoreManager(StoreManager newManager, string storeName)
        {
            Store store = GetStore(storeName);
            return store.AddStoreManager(newManager);
        }

        public bool AddStoreOwner(StoreOwner newOwner, string storeName)
        {
            Store store = GetStore(storeName);
            return store.AddStoreOwner(newOwner);
        }
        public bool RemoveStoreOwner(string ownerUsername, string storeName, String appointerUsername)
        {
            Store store = GetStore(storeName);
            return store.RemoveStoreOwner(ownerUsername, appointerUsername);
        }

        public bool RemoveStoreManager(string managerUsername, string storeName, String appointerUsername)
        {
            Store store = GetStore(storeName);
            return store.RemoveStoreManager(managerUsername, appointerUsername);
        }

        public List<Store> GetStoresOfUser(String username, bool isAdmin)
        {
            List<Store> storeList = new List<Store>();
            foreach(KeyValuePair<string, Store> pair in _stores)
            {
                bool isActive = (pair.Value.State == StoreState.Active);
                bool isInactiveButOwner = (pair.Value.State != StoreState.Inactive) && ((pair.Value.GetFounder().Username == username) || (pair.Value.GetOwners().Exists(x => x.Username == username)));
                if (isAdmin || isActive || isInactiveButOwner)
                {
                    storeList.Add(pair.Value);
                }
            }
            return storeList;
        }

        public List<Store> GetAllActiveStores(bool isAdmin)
        {
            List<Store> storeList = new List<Store>();
            foreach (KeyValuePair<string, Store> pair in _stores)
            {
                bool isActive = (pair.Value.State == StoreState.Active);
                if (isAdmin || isActive)
                {
                    storeList.Add(pair.Value);
                }
            }
            return storeList;
        }

        internal List<StoreManager> getStoreManagers(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetManagers();
        }

        internal List<StoreOwner> getStoreOwners(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetOwners();
        }

        internal StoreFounder getStoreFounder(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetFounder();
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered Visitor's roles from all relevant stores.</para>
        /// </summary>
        /// <param name="registered"> The Visitor to revoke the roles of.</param>
        internal void RemoveAllRoles(Registered registered)
        {
            ICollection<String> storeNames = registered.StoresWithRoles;
            foreach (String storeName in storeNames)
            {
                Store store = GetStore(storeName);
                store.RemoveRoles(registered.Username);
            }
        }

        public Queue<MessageToStore> GetStoreMessages(string storeName)
        {
            Store store = GetStore(storeName);
            return store.MessagesToStore;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in StoreManagement.{functionName}. Cause: {message}.");
        }

        internal MessageToStore AnswerStoreMessage(string storeName, int msgID)
        {
            Store store = GetStore(storeName);
            return store.AnswerMessage(msgID);
        }

        internal List<Store> GetStoresByName(List<string> stores)
        {
            List<Store> storeList = new List<Store>();
            foreach (string storeName in stores)
            {
                storeList.Add(GetStore(storeName));
            }
            return storeList;
        }
    }
}
