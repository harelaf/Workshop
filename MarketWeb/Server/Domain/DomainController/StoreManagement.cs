using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Shared;
using MarketWeb.Server.DataLayer;
using Newtonsoft.Json;
using MarketWeb.Shared.DTO;

namespace MarketWeb.Server.Domain
{
    public class StoreManagement
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<String, Store> _stores; //<storeName:String, Store>
        private DalTRranslator _translator;
        private DalController _dalController = DalController.GetInstance();
        private static bool hasInitialized = false;
        public StoreManagement()
        {
            _stores = new Dictionary<String, Store>();
            _translator = new DalTRranslator();
        }
        public void load()
        {
            if (!hasInitialized)
            {
                _stores = _translator.StoreListDalToDomain(_dalController.GetAllActiveStores());
            }
            hasInitialized = true;
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
                _dalController.EditItemPrice(storeName, itemID, newPrice);
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
            _dalController.EditItemName(storeName, itemID, newName);    
        }

        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            Item item = GetItem(storeName, itemID);
            item.SetDescription(newDescription);
            _dalController.EditItemDescription(storeName, itemID, newDescription);
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

        public void SendMessageToStore(String Username, String storeName, String title, String message)
        {
            Store store = GetActiveStore(storeName);
            int id = _dalController.SendMessageToStore(storeName, title, message, Username);
            MessageToStore messageToStore = new MessageToStore(storeName, Username, title, message, id);
            store.AddMessage(messageToStore);
            
        }
        public void SendMessageToStoreTest(String Username, String storeName, String title, String message, int id)
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

        public void markAcceptedBidAsUsed(string bidder, string storeName, int itemID)
        {
            GetStore(storeName).markAcceptedBidAsUsed(bidder, itemID);
            //_dalController.markB
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
            _dalController.OpenNewStore(storeName, founder.Username);
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
            _stores.Remove(storeName);
            _dalController.CloseStore(storeName);
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
            _dalController.ReopenStore(storeName);
            _stores.Add(storeName,store);
        }

        public void CloseStorePermanently(String storeName)
        {
            Store store = GetStore(storeName);
            store.CloseStorePermanently();
            _stores.Remove(storeName);
            _dalController.CloseStorePermanently(storeName);
        }

        public void AddStoreDiscount(String storeName, Discount discount)
        {
            Store store = GetActiveStore(storeName);
            store.AddDiscount(discount);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            _dalController.EditStoreDiscountPolicy(storeName, JsonConvert.SerializeObject(store.GetDiscountPolicy(), settings));
        }

        public void AddStorePurchasePolicy(string storeName, Condition condition)
        {
            Store store = GetActiveStore(storeName);
            store.AddConditionToPurchasePolicy(condition);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            _dalController.EditStorePurchasePolicy(storeName, JsonConvert.SerializeObject(store.GetPurchasePolicy(), settings));
        }

        public bool AddStoreManager(StoreManager newManager, string storeName)
        {
            Store store = GetActiveStore(storeName);
            return store.AddStoreManager(newManager);
        }

        //public bool AddStoreOwner(StoreOwner newOwner, string storeName)
        //{
        //    Store store = GetStore(storeName);
        //    return store.AddStoreOwner(newOwner);
        //}
        public Tuple<List<string>, List<string>> RemoveStoreOwner(string ownerUsername, string storeName, String appointerUsername)
        {
            Store store = GetActiveStore(storeName);
            Tuple<List<string>, List<string>> tuple= store.RemoveStoreOwner(ownerUsername, appointerUsername);
            _dalController.RemoveStoreOwner(ownerUsername, storeName);
            return tuple;
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
            /*foreach (KeyValuePair<string, Store> pair in _stores)
            {
                bool isActive = (pair.Value.State == StoreState.Active);
                if (isAdmin || isActive)
                {
                    storeList.Add(pair.Value);
                }
            }
            return storeList;*/
            foreach(Store store in _stores.Values)
                storeList.Add(store);
            return storeList;
        }

        public List<StoreManager> getStoreManagers(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetManagers();
        }

        public List<StoreOwner> getStoreOwners(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetOwners();
        }

        public StoreFounder getStoreFounder(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetFounder();
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered Visitor's roles from all relevant stores.</para>
        /// </summary>
        /// <param name="registered"> The Visitor to revoke the roles of.</param>
        public void RemoveAllRoles(Registered registered)
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
            Store store = GetStore(storeName);
            return store.MessagesToStore;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in StoreManagement.{functionName}. Cause: {message}.");
        }

        public MessageToStore AnswerStoreMessage(string storeName, int msgID)
        {
            Store store = GetActiveStore(storeName);
            return store.AnswerMessage(msgID);
        }

        public List<Store> GetStoresByName(List<string> stores)
        {
            List<Store> storeList = new List<Store>();
            foreach (string storeName in stores)
            {
                storeList.Add(GetStore(storeName));
            }
            return storeList;
        }

        public void ResetStoreDiscountPolicy(string storeName)
        {
            GetStore(storeName).ResetDiscountPolicy();
            _dalController.ResetStoreDiscountPolicy(storeName);
        }

        public void ResetStorePurchasePolicy(string storeName)
        {
            GetStore(storeName).ResetPurchasePolicy();
            _dalController.ResetStorePurchasePolicy(storeName);
        }

        public List<string> GetDiscountPolicyStrings(string storeName)
        {
            return GetStore(storeName).GetDiscountPolicyStrings();
        }

        public List<string> GetPurchasePolicyStrings(string storeName)
        {
            return GetStore(storeName).GetPurchasePolicyStrings();
        }

        public void BidItemInStore(string storeName, int itemId, int amount, double newPrice, string bidder)
        {
            GetStore(storeName).BidItem(itemId, amount, newPrice, bidder);
        }

        public bool AcceptBid(string storeName, string acceptor, int itemId, string bidder)
        {
            return GetStore(storeName).AcceptBid(acceptor, itemId, bidder);
        }

        public bool CounterOfferBid(string storeName, string acceptor, int itemId, string bidder, double counterOffer)
        {
            return GetStore(storeName).CounterOfferBid(acceptor, itemId, bidder, counterOffer);
        }

        public void RejectBid(string storeName, string rejector, int itemId, string bidder)
        {
            GetStore(storeName).RejectBid(rejector, itemId, bidder);
        }

        public double GetBidAcceptedPrice(string bidder, string storeName, int itemID, int amount)
        {
            return GetStore(storeName).GetBidAcceptedPrice(bidder, itemID, amount);
        }

        public List<string> GetUsernamesWithPermissionInStore(string storeName, Operation op)
        {
            return GetStore(storeName).GetUsernamesWithPermission(op);
        }

        public List<Bid> GetBidsForStore(string storeName)
        {
            return GetStore(storeName).GetBids();
        }

        public List<Bid> GetVisitorBidsAtStore(string storeName, string bidder)
        {
            String errorMessage = "";
            List<Bid> bids = GetStore(storeName).GetVisitorBids(bidder);
            if (bids != null && bids.Count > 0)
                return bids;
            errorMessage = "this visitor has no bids at store '" + storeName + "'.";
            LogErrorMessage("GetVisitorBidsAtStore", errorMessage);
            throw new Exception(errorMessage);
        }

        internal List<string> GetStoreRolesByName(string storeName)
        {
            Store store = GetStore(storeName);
            return store.GetStoreRolesByName();
        }

        public StoreOwner AddStoreOwnerForTestPurposes(StoreOwner newOwner, string storeName)
        {
            Store store = GetStore(storeName);
            return store.AddStoreOwner(newOwner);
        }

        public StoreOwner AcceptOwnerAppointment(string storeName, string acceptor, string newOwner)
        {
            return GetStore(storeName).AcceptOwnerAppointment(acceptor, newOwner);
        }

        public void RejectOwnerAppointment(string storeName, string rejector, string newOwner)
        {
            GetStore(storeName).RejectOwnerAppointment(rejector, newOwner);
        }

        public Dictionary<string, List<string>> GetStandbyOwnersInStore(string storeName)
        {
            return GetStore(storeName).GetStandbyOwnersInStore();
        }
    }
}
