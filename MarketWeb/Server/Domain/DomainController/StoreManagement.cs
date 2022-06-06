using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Shared;
using MarketWeb.Server.DataLayer;

namespace MarketWeb.Server.Domain
{
    public class StoreManagement
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<String, Store> _stores; //<storeName:String, Store>
        private DalTRranslator _translator;
        private DalController _dalController = DalController.GetInstance();
        public StoreManagement()
        {
            _stores = new Dictionary<String, Store>();
            _translator = new DalTRranslator();
        }

        public Store GetActiveStore(String storeName)
        {
            String errorMessage;
            if (_stores.ContainsKey(storeName))
                return _stores[storeName];
            errorMessage = "Store '" + storeName + "' does not exists Or inActive.";
            LogErrorMessage("GetStore", errorMessage);
            throw new Exception(errorMessage);
        }
        public Store GetStore(string storeName)
        {
            if (_stores.ContainsKey(storeName))
                return _stores[storeName];
            return _translator.StoreDalToDomain(_dalController.GetStoreInformation(storeName));
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
            Store store = GetActiveStore(storeName);
            return store.ReserveItem(itemID, amount);
        }

        public Item GetItem(String storeName, int itemID)
        {
            String errorMessage;
            Store store = GetActiveStore(storeName);
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
                Stock stock = _stores[storeName].Stock;
                List<Item> cur_items = new List<Item>();
                foreach (KeyValuePair<Item, int> pair in stock.Items)
                {
                    Item item = pair.Key;
                    if (itemName != null && itemName.Length > 0 && item.Name.Contains(itemName))
                    {
                        cur_items.Add(item);
                    }
                    else if (itemCategory != null && itemCategory.Length > 0 && item.Category.Contains(itemCategory))
                    {
                        cur_items.Add(item);
                    }
                    else if (keyWord != null && keyWord.Length > 0 && item.Description != null && item.Description.Length > 0 && item.Description.Contains(keyWord))
                    {
                        cur_items.Add(item);
                    }
                }
                if (cur_items.Count > 0)
                    filteredItems.Add(storeName, cur_items);
            }
            return filteredItems;
        }

        public void SendMessageToStore(String Username, String storeName, String title, String message, int id)
        {
            Store store = GetActiveStore(storeName);
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
            Store store = GetActiveStore(storeName);
            store.UnReserveItem(item, amount_to_add);
        }

        public void OpenNewStore(StoreFounder founder, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (_dalController.StoreExists(storeName))
            {
                string errorMessage = $"system allready has store with name: {storeName}";
                LogErrorMessage("UnreserveItemInStore", errorMessage);
                throw new Exception(errorMessage);
            }
            Store newStore = new Store(storeName, founder, purchasePolicy, discountPolicy);
            _stores[storeName] = newStore;
        }

        public bool CheckStoreNameExists(String storeName)
        {
            return _dalController.StoreExists(storeName);
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
            Store store = GetActiveStore(storeName);
            return store.GetStoreRolesByName();
        }

        public void RateStore(String Username, String storeName, int rating, String review)
        {
            Store store = GetActiveStore(storeName);
            store.RateStore(Username, rating, review);
            _dalController.RateStore(storeName, rating, review, Username);
        }

        public void UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity)
        {
            Store store = GetActiveStore(storeName);
            store.UpdateStockQuantityOfItem(itemID, newQuantity);
            _dalController.UpdateStockQuantityOfItem(storeName, itemID, newQuantity);
        }

        public bool isStoreActive(String storeName)
        {
            if (!IsStoreExist(storeName))
                return false;
            Store store = GetActiveStore(storeName);
            return store.isActive();
        }

        public void AddItemToStoreStock(String storeName, String name, double price, String description, String category, int quantity)
        {
            Store store = GetActiveStore(storeName);
            int id = _dalController.AddItemToStoreStock(storeName, name, price, description, category, quantity);
            store.AddItemToStoreStock(id, name, price, description, category, quantity);
        }
        public void AddItemToStoreStockTest(String storeName, int id, String name,  double price, String description, String category, int quantity)
        {
            Store store = GetActiveStore(storeName);
            //int id = _dalController.AddItemToStoreStock(storeName, name, price, description, category, quantity);
            store.AddItemToStoreStock(id, name, price, description, category, quantity);
        }

        public void RemoveItemFromStore(String storeName, int itemID)
        {
            Store store = GetActiveStore(storeName);
            store.RemoveItemFromStore(itemID);
            _dalController.RemoveItemFromStore(storeName, itemID);
        }

        public void CloseStore(String storeName)
        {
            Store store = GetActiveStore(storeName);
            store.CloseStore();
        }

        public void ReopenStore(String storeName)
        {
            String errorMessage;
            Store store = GetActiveStore(storeName);
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
            Store store = GetActiveStore(storeName);
            store.CloseStorePermanently();
            _stores.Remove(storeName);
        }

        public void AddStoreDiscount(String storeName, Discount discount)
        {
            GetActiveStore(storeName).AddDiscount(discount);
        }

        public bool AddStoreManager(StoreManager newManager, string storeName)
        {
            Store store = GetActiveStore(storeName);
            return store.AddStoreManager(newManager);
        }

        public bool AddStoreOwner(StoreOwner newOwner, string storeName)
        {
            Store store = GetActiveStore(storeName);
            return store.AddStoreOwner(newOwner);
        }
        public Tuple<List<string>, List<string>> RemoveStoreOwner(string ownerUsername, string storeName, String appointerUsername)
        {
            Store store = GetActiveStore(storeName);
            return store.RemoveStoreOwner(ownerUsername, appointerUsername);
        }

        public bool RemoveStoreManager(string managerUsername, string storeName, String appointerUsername)
        {
            Store store = GetActiveStore(storeName);
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
            Store store = GetActiveStore(storeName);
            return store.GetManagers();
        }

        internal List<StoreOwner> getStoreOwners(string storeName)
        {
            Store store = GetActiveStore(storeName);
            return store.GetOwners();
        }

        internal StoreFounder getStoreFounder(string storeName)
        {
            Store store = GetActiveStore(storeName);
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
                Store store = GetActiveStore(storeName);
                store.RemoveRoles(registered.Username);
            }
        }

        public List<MessageToStore> GetStoreMessages(string storeName)
        {
            Store store = GetActiveStore(storeName);
            return store.MessagesToStore;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in StoreManagement.{functionName}. Cause: {message}.");
        }

        internal MessageToStore AnswerStoreMessage(string storeName, int msgID)
        {
            Store store = GetActiveStore(storeName);
            return store.AnswerMessage(msgID);
        }

        internal List<Store> GetStoresByName(List<string> stores)
        {
            List<Store> storeList = new List<Store>();
            foreach (string storeName in stores)
            {
                storeList.Add(GetActiveStore(storeName));
            }
            return storeList;
        }
    }
}
