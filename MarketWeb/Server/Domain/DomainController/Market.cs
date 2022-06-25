using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Service;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Server.Domain.PurchasePackage.DiscountPolicyPackage;
using MarketWeb.Server.Domain.PurchasePackage.PolicyPackage;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using Microsoft.AspNetCore.SignalR;
using MarketWeb.Server.Domain.PurchasePackage;
using System.Threading.Tasks;
using MarketWeb.Server.DataLayer;

namespace MarketWeb.Server.Domain
{
    public class Market
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public StoreManagement _storeManagement;
        private VisitorManagement _VisitorManagement;
        private History _history;
        private IDictionary<string, Operation> _opNameToOp;
        private DalTRranslator _dalTRranslator;
        private DalController _dalController;
        protected NotificationHub _notificationHub;

        public Market(NotificationHub notificationHub = null)
        {
            _opNameToOp = new Dictionary<string, Operation>();
            setOPerationDictionary();
            _notificationHub = notificationHub;
            // Calling RestartSystem after creating this object is important.
        }

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the Visitor as the current admin.</para>
        /// </summary>
        public async Task RestartSystem(String adminUsername, String adminPassword, String ipShippingService, String ipPaymentService, string datasource, string initialcatalog, string userid, string password)
        {//I.1
            // Do starting system stuff with IPs
            WSIEPaymentHandler paymentHandler = new WSIEPaymentHandler(ipPaymentService);
            try
            {
                if (!(await paymentHandler.Handshake()))
                    throw new Exception();
            } 
            catch(Exception)
            {
                throw new Exception("CONFIG: Payment method wasn't available.");
            }
            PurchaseProcess.GetInstance().AddPaymentMethod("WSIE", paymentHandler);
            WSEPShippingHandler shippingHandler = new WSEPShippingHandler(ipShippingService);
            try
            {
                if (!(await shippingHandler.Handshake()))
                    throw new Exception();
            }
            catch(Exception)
            {
                throw new Exception("CONFIG: Shipping method wasn't available.");
            }
            PurchaseProcess.GetInstance().AddShipmentMethod("WSEP", shippingHandler);

           
            // Initialize DB
            DalController.InitializeContext(datasource, initialcatalog, userid, password);
            this._dalController = DalController.GetInstance();

            _storeManagement = new StoreManagement();
            _VisitorManagement = new VisitorManagement();
            _VisitorManagement.SetNotificationHub(_notificationHub);

            // Initialize admin
            _VisitorManagement.InitializeAdmin(adminUsername, adminPassword);
            _VisitorManagement.AdminStart(adminUsername, adminPassword);

            //translator:
            _dalTRranslator = new DalTRranslator();
            DalTRranslator.StoreManagement = _storeManagement;
            _storeManagement.load();

            _history = new History(this);
            
        }


        /// add\update basket eof store with item and amount.
        /// update store stock: itemAmount- amount

        //--VisitorToken should be a visitor in system.
        //--item itemID should be an item of storeName
        //--storeName should be a store in system
        //--storeName should have at least amount of itemID
        public void AddItemToCart(String VisitorToken, int itemID, String storeName, int amount)
        {//II.2.3
            String errorMessage = null;
            CheckIsVisitorAVisitor(VisitorToken, "AddItemToCart");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the given store name";
            if (errorMessage != null)
            {
                LogErrorMessage("AddItemToCart", errorMessage);
                throw new Exception(errorMessage);
            }
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                lock (store.Stock)
                {
                    Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
                    _VisitorManagement.AddItemToVisitorCart(VisitorToken, store, item, amount);
                    
                }
            }

        }

        public void AddAcceptedBidToCart(String VisitorToken, int itemID, String storeName, int amount)
        {//II.2.3
            String errorMessage = null;
            CheckIsVisitorAVisitor(VisitorToken, "AddAcceptedBidToCart");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the given store name";
            if (errorMessage != null)
            {
                LogErrorMessage("AddAcceptedBidToCart", errorMessage);
                throw new Exception(errorMessage);
            }
            Store store = _storeManagement.GetStore(storeName);
            bool isAGuest = _VisitorManagement.IsVisitorAGuest(VisitorToken);
            String bidder = isAGuest ? VisitorToken : _VisitorManagement.GetRegisteredUsernameByToken(VisitorToken);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    errorMessage = $"Store {storeName} is currently inactive.";
                if (errorMessage != null)
                {
                    LogErrorMessage("AddAcceptedBidToCart", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
                    Item copy = new Item(item.ItemID, item.Name, item._price, item.Description, item.Category);
                    try
                    {
                        double price = _storeManagement.GetBidAcceptedPrice(bidder, storeName, itemID, amount);
                        copy.SetPrice(price);
                        _VisitorManagement.AddAcceptedBidToCart(VisitorToken, store, itemID, amount);
                        _storeManagement.markAcceptedBidAsUsed(bidder, storeName, itemID);
                        if(!isAGuest)
                            _dalController.AddAcceptedBidToCart(bidder, _dalTRranslator.StoreDomainToDal(store), itemID, amount, price);
                    }
                    catch (Exception ex)
                    {
                        _storeManagement.UnreserveItemInStore(storeName, item, amount);
                        LogErrorMessage("AddAcceptedBidToCart", ex.Message);
                        throw ex;
                    }
                }
            }

        }

        public Item RemoveItemFromCart(String VisitorToken, int itemID, String storeName)
        {//II.2.4
            String errorMessage = null;
            CheckIsVisitorAVisitor(VisitorToken, "RemoveItemFromCart");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the givn storeid";
            if (errorMessage != null)
            {
                LogErrorMessage("RemoveItemFromCart", errorMessage);
                throw new Exception(errorMessage);
            }

            Store store = _storeManagement.GetStore(storeName);// active or not, should be able to remove
            lock (store)
            {
                Item item = _storeManagement.GetItem(storeName, itemID);
                int amount_removed = _VisitorManagement.RemoveItemFromCart(VisitorToken, item, _storeManagement.GetActiveStore(storeName));
                // now update store stock
                lock (store.Stock)
                {
                    _storeManagement.UnreserveItemInStore(storeName, item, amount_removed);
                   
                }
                return item;
            }
           
        }

        internal void RemoveAcceptedBidFromCart(string authToken, int itemID, string storeName)
        {
            String errorMessage = null;
            CheckIsVisitorAVisitor(authToken, "RemoveAcceptedBidFromCart");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the givn storeid";
            if (errorMessage != null)
            {
                LogErrorMessage("RemoveAcceptedBidFromCart", errorMessage);
                throw new Exception(errorMessage);
            }
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_removed = _VisitorManagement.RemoveAcceptedBidFromCart(authToken, itemID, storeName);
            // now update store stock
            Store store = _storeManagement.GetStore(storeName);
            lock (store.Stock)
            {
                _storeManagement.UnreserveItemInStore(storeName, item, amount_removed);
            }
        }


        internal Registered getUser(String store_founder_token)
        {
            return _VisitorManagement.GetRegisteredByToken(store_founder_token);
        }

        public void UpdateQuantityOfItemInCart(String VisitorToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            String errorMessage = null;
            CheckIsVisitorAVisitor(VisitorToken, "UpdateQuantityOfItemInCart");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the givn storeid";
            if (errorMessage != null)
            {
                LogErrorMessage("UpdateQuantityOfItemInCart", errorMessage);
                throw new Exception(errorMessage);
            }
            Item item = _storeManagement.GetItem(storeName, itemID);
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                lock (store.Stock)
                {
                    int amount_differnce = _VisitorManagement.GetUpdatingQuantityDifference(VisitorToken, item, _storeManagement.GetActiveStore(storeName), newQuantity);
                    if (amount_differnce == 0)
                        errorMessage = "Update Quantity Of Item In Cart faild: current quantity and new quantity are the same!";
                    if (errorMessage != null)
                    {
                        LogErrorMessage("UpdateQuantityOfItemInCart", errorMessage);
                        throw new Exception(errorMessage);
                    }
                    if (amount_differnce > 0)// add item to cart and remove it from store stock
                        _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
                    else//remove item from cart and add to store stock[
                        _storeManagement.UnreserveItemInStore(storeName, item, -1 * amount_differnce);
                    _VisitorManagement.UpdateItemInVisitorCart(VisitorToken, store, item, newQuantity, -1*amount_differnce);

                }
            }
        }


        public void OpenNewStore(String authToken, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "OpenNewStore");
            if (storeName.Equals(""))
                errorMessage = "Invalid Input: Blank store name.";
            else if (_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = $"A store with the name {storeName} already exists in the system.";
            if (errorMessage != null)
            {
                LogErrorMessage("OpenNewStore", errorMessage);
                throw new Exception(errorMessage);
            }
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            StoreFounder founder = new StoreFounder(Username, storeName);
            _VisitorManagement.AddRole(Username, founder);
            _storeManagement.OpenNewStore(founder, storeName, purchasePolicy, discountPolicy);
        }

        public Store GetStoreInformation(String authToken, String storeName)
        {
            String errorMessage = null;
            CheckIsVisitorAVisitor(authToken, "GetStoreInformation");
            if (storeName.Equals(""))
                errorMessage = "Invalid Input: Blank store name.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetStoreInformation", errorMessage);
                throw new Exception(errorMessage);
            }
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (_storeManagement.isStoreActive(storeName))
                    return store;

                else
                {
                    String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
                    if (_VisitorManagement.CheckAccess(Username, storeName, Operation.STORE_INFORMATION))
                        return store;
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                    LogErrorMessage("GetStoreInformation", errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }

        public void RateStore(String authToken, String storeName, int rating, String review)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "RateStore");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_history.CheckIfVisitorPurchasedInStore(Username, storeName))
                errorMessage = $"Visitor {Username} has never purchased in {storeName}.";
            else if (storeName.Equals(""))
                errorMessage = "Invalid Input: Store name is blank.";
            else if (rating < 0 || rating > 10)
                errorMessage = "Invalid Input: rating should be in the range [0, 10].";
            if (errorMessage != null)
            {
                LogErrorMessage("RateStore", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.RateStore(Username, storeName, rating, review);
        }

        public void AddItemToStoreStock(String authToken, String storeName, String name, double price, String description, String category, int quantity)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "AddItemToStoreStock");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                else if (price < 0)
                    errorMessage = "Invalid Input: Price has to be at least 0.";
                else if (name.Equals(""))
                    errorMessage = "Invalid Input: Blank item name.";
                else if (quantity < 0)
                    errorMessage = "Invalid Input: Quantity has to be at least 0.";
                if (errorMessage != null)
                {
                    LogErrorMessage("AddItemToStoreStock", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    _storeManagement.AddItemToStoreStock(storeName, name, price, description, category, quantity);
                }
            }
        }
        public void AddItemToStoreStock(String authToken, String storeName, int id,  String name, double price, String description, String category, int quantity)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "AddItemToStoreStock");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                else if (price < 0)
                    errorMessage = "Invalid Input: Price has to be at least 0.";
                else if (name.Equals(""))
                    errorMessage = "Invalid Input: Blank item name.";
                else if (quantity < 0)
                    errorMessage = "Invalid Input: Quantity has to be at least 0.";
                if (errorMessage != null)
                {
                    LogErrorMessage("AddItemToStoreStock", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    _storeManagement.AddItemToStoreStockTest(storeName, id,  name, price, description, category, quantity);
                }
            }
        }


        public void RemoveItemFromStore(String authToken, String storeName, int itemID)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "RemoveItemFromStore");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                if (errorMessage != null)
                {
                    LogErrorMessage("RemoveItemFromStore", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    _storeManagement.RemoveItemFromStore(storeName, itemID);
                }
            }
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchasesHistory(String authToken, String storeName)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "GetStorePurchasesHistory");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (storeName.Equals(""))
                errorMessage = "Invalid Input: Blank store name.";
            else if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = $"Store {storeName} does not exist.";
            else if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.STORE_HISTORY_INFO) &&
                !_VisitorManagement.CheckAccess(Username, null, Operation.STORE_HISTORY_INFO))
                errorMessage = $"This Visitor is not an admin or owner in {storeName}.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetStorePurchasesHistory", errorMessage);
                throw new Exception(errorMessage);
            }
            return _history.GetStorePurchaseHistory(storeName);
        }

        public void UpdateStockQuantityOfItem(String authToken, String storeName, int itemID, int newQuantity)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "UpdateStockQuantityOfItem");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                else if (newQuantity < 0)
                    errorMessage = "Invalid Input: Quantity has to be at least 0.";
                if (errorMessage != null)
                {
                    LogErrorMessage("UpdateStockQuantityOfItem", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    _storeManagement.UpdateStockQuantityOfItem(storeName, itemID, newQuantity);
                }
            }
        }

        public void CloseStore(String authToken, String storeName)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "CloseStore");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetActiveStore(storeName);
            lock (store)
            {
                if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                if (errorMessage != null)
                {
                    LogErrorMessage("CloseStore", errorMessage);
                    throw new Exception(errorMessage);
                }
                HasPermission(authToken, storeName, "CLOSE_STORE");
                _storeManagement.CloseStore(storeName);
            }
            List<String> names = store.GetStoreRolesByName();
            String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am sad to inform you that {storeName} is temporarily closing down. " +
                $"Your roles in the store will remain until we decide permanently close down." +
                $"Yours Truly," +
                $"{Username}.";
            foreach (String name in names)
            {
                SendNotificationToStore(storeName, name, title, message);
            }
        }

        public void CloseStorePermanently(String authToken, String storeName)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "CloseStorePermanently");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_VisitorManagement.CheckAccess(Username, null, Operation.PERMENENT_CLOSE_STORE))
                    errorMessage = $"Visitor is not an admin.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                else if (store.State == StoreState.Closed)
                    errorMessage = $"The store: {storeName} is allready closed!";
                if (errorMessage != null)
                {
                    LogErrorMessage("CloseStorePermanently", errorMessage);
                    throw new Exception(errorMessage);
                }
                List<String> names = store.GetStoreRolesByName();
                String title = $"Store: {storeName} is permanently closing down: [{DateTime.Now.ToString()}].";
                String message = $"I am sad to inform you that {storeName} is closing down. " +
                    $"All of your roles have been revoked." +
                    $"Yours Truly," +
                    $"{Username}.";
                foreach (String name in names)
                {
                    _VisitorManagement.RemoveRole(name, storeName);
                    SendNotificationToStore(storeName, name, title, message);
                }
                _storeManagement.CloseStorePermanently(storeName);
            }
        }

        public void ReopenStore(String authToken, String storeName)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "ReopenStore");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (store.State == StoreState.Active || store.State == StoreState.Closed)
                    errorMessage = $"You can't reopen store: {storeName} because it's in state: {store.State}";
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.REOPEN_STORE))
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                if (errorMessage != null)
                {
                    LogErrorMessage("ReopenStore", errorMessage);
                    throw new Exception(errorMessage);
                }
                _storeManagement.ReopenStore(storeName);
            }
            List<String> names = store.GetStoreRolesByName();
            String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am happy to inform you that {storeName} is reopening. " +
                $"Your roles in the store stayed the same." +
                $"Yours Truly," +
                $"{Username}.";
            foreach (String name in names)
            {
                SendNotificationToStore(storeName, name, title, message);
            }
        }

        public void EditItemPrice(String authToken, String storeName, int itemID, double newPrice)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "EditItemPrice");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
                errorMessage = "the given Visitor is not a stock owner of the given store";
            if (errorMessage != null)
            {
                LogErrorMessage("EditItemPrice", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.EditItemPrice(storeName, itemID, newPrice);
        }
        public void EditItemName(String authToken, String storeName, int itemID, String newName)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "EditItemName");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
                errorMessage = "the given Visitor is not a stock owner of the given store";
            if (errorMessage != null)
            {
                LogErrorMessage("EditItemName", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.EditItemName(storeName, itemID, newName);
        }
        public void EditItemDescription(String authToken, String storeName, int itemID, String newDescription)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "EditItemDescription");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
                errorMessage = "the given Visitor is not a stock owner of the given store";
            if (errorMessage != null)
            {
                LogErrorMessage("EditItemDescription", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.EditItemDescription(storeName, itemID, newDescription);
        }

        public void RateItem(String authToken, int itemID, String storeName, int rating, String review)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "RateItem");
            String username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Item item = _storeManagement.GetItem(storeName, itemID);
            if (rating < 0 || rating > 10)
                errorMessage = "Rate should be beteen 0 to 10";
            else if (!_history.CheckIfVisitorPurchasedItemInStore(username, storeName, item))
                errorMessage = "This Visitor has never bought item with id: " + itemID + " at " + storeName;
            if (errorMessage != null)
            {
                LogErrorMessage("RateItem", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.RateItem(username, item, rating, review);
            _dalController.RateItem(itemID, storeName, rating, review, username);
        }

        public IDictionary<string, List<Item>> GetItemInformation(String authToken, String itemName, String itemCategory, String keyWord)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "GetItemInformation");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.IsRegistered(appointerUsername))
                errorMessage = "Visitor " + appointerUsername + " not found in system";
            if (errorMessage != null)
            {
                LogErrorMessage("GetItemInformation", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetItemInformation(itemName, itemCategory, keyWord);
        }

        public void SendMessageToStore(String authToken, String storeName, String title, String message)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "SendMessageToStore");
            string senderUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (errorMessage != null)
            {
                LogErrorMessage("SendMessageToStore", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.SendMessageToStore(senderUsername, storeName, title, message);
            List<string> names = _storeManagement.GetStoreRolesByName(storeName);
            foreach (string name in names)
			{
                SendNotificationToStore(storeName, name, "Store Message: " + title, message);
			}
        }
        //there is no store in system with the given store name
        public void SendAdminMessageToRegisterd(string userToken, String UsernameReciever, String title, String message)
        {
            String errorMessage = null;
            string senderUsername = null;
            if (!_VisitorManagement.IsVisitorLoggedin(userToken))
                errorMessage = "user have yo be logged in for this operation.";
            else if (!_VisitorManagement.IsRegistered(UsernameReciever))
                errorMessage = "Visitor " + UsernameReciever + " not found in system";
            else
            {
                senderUsername = _VisitorManagement.GetRegisteredUsernameByToken(userToken);
                if (!_VisitorManagement.CheckAccess(senderUsername, null, Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE))
                {
                    errorMessage = "User " + senderUsername + " doesn't have permission to preform this operation.";
                }
            }

            if (errorMessage != null)
            {
                LogErrorMessage("SendMessageToRegisterd", errorMessage);
                throw new Exception(errorMessage);
            }
            int id = _dalController.SendAdminMessage(UsernameReciever, senderUsername, title, message);
            _VisitorManagement.SendAdminMessageToRegistered(UsernameReciever,senderUsername, title, message, id);
            SendNotification("Administration", UsernameReciever, "Admin Message: " + title, message);

        }
        public void SendNotificationToStore(string storeName, string usernameReciever, String title, String message)
        {
            String errorMessage = null;
            if (!_VisitorManagement.IsRegistered(usernameReciever))
                errorMessage = "Visitor " + usernameReciever + " not found in system";
            else if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the given store name";
            if (errorMessage != null)
            {
                LogErrorMessage("SendMessageToRegisterd", errorMessage);
                throw new Exception(errorMessage);
            }
            SendNotification(storeName, usernameReciever, title, message); 
        }
        public void SendNotification(string Sender, string usernameReciever, String title, String message)
        {
            int id = _dalController.SendNotification(Sender, usernameReciever, title, message);
            _VisitorManagement.SendNotificationMessageToRegistered(usernameReciever, Sender, title, message, id);
        }
        //1. admin: sender, reciver, msg, title
        //2. notify:reciver, msg, title, store,
        //4. complaint: send recive
        public void AnswerStoreMesseage(string authToken, string receiverUsername, int msgID, string storeName, string reply)
        {
            String errorMessage = null;
            MessageToStore msg = null;
            CheckIsVisitorLoggedIn(authToken, "AnswerStoreMesseage");
            string replierUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.CheckStoreNameExists(storeName))
                errorMessage = "there is no store in system with the given store name";
            else if (!_VisitorManagement.IsRegistered(receiverUsername))
                errorMessage = "Visitor " + receiverUsername + " is no longer user in system. replyment faild.";
            else
            {
                msg = _storeManagement.AnswerStoreMessage(storeName, msgID);
                if (msg == null)
                    errorMessage = "Coild'nt find message: " + msgID + " in store: " + storeName;
            }
            if (errorMessage != null)
            {
                LogErrorMessage("AnswerStoreMesseage", errorMessage);
                throw new Exception(errorMessage);
            }
            _VisitorManagement.SendStoreMessageReplyment(msg, replierUsername, receiverUsername ,reply);
            _dalController.AnswerStoreMesseage(msgID, storeName, reply, replierUsername);
            SendNotificationToStore(storeName, receiverUsername, "Reply - "+ replierUsername, reply);
        }

        public List<MessageToStore> GetStoreMessages(String authToken, String storeName)
        {
            String username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(username, storeName, Operation.RECEIVE_AND_REPLY_STORE_MESSAGE))
                return _storeManagement.GetStoreMessages(storeName);
            return null;
        }

        public void AddStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.6
            CheckIsVisitorLoggedIn(authToken, "AddStoreManager");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_MANAGER))
            {
                if (!_VisitorManagement.IsRegistered(managerUsername))
                {
                    throw new Exception("there is no register user in system with username: " + managerUsername + ". you can only appoint register user to store role.");
                }
                StoreManager newManager = new StoreManager(managerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreManager(newManager, storeName))
                {
                    _VisitorManagement.AddRole(managerUsername, newManager);
                    _dalController.AddStoreManager(managerUsername, storeName, appointerUsername);
                }
                   
            }
            else
            {
                throw new Exception("you dont have accses to appoint users to this stores");
            }
        }

        public ICollection<AdminMessageToRegistered> GetRegisteredMessagesFromAdmin(string authToken)
        {
            CheckIsVisitorLoggedIn(authToken, "GetRegisteredMessagesFromAdmin");
            return _VisitorManagement.getRegisteredMessages(authToken);
        }
        public ICollection<MessageToStore> GetRegisteredAnswerdStoreMessages(string authToken)
        {
            CheckIsVisitorLoggedIn(authToken, "GetRegisteredAnswerdStoreMessages");
            return _VisitorManagement.GetRegisteredAnswerdStoreMessages(authToken);
        }

        public async Task PurchaseMyCartAsync(String VisitorToken, String address, String city, String country, String zip, String purchaserName, String paymentMethode, String shipmentMethode,  string cardNumber = null, string month = null, string year = null, string holder = null, string ccv = null, string id = null)
        {//II.2.5
            CheckIsVisitorAVisitor(VisitorToken, "PurchaseMyCart");
            ShoppingCart shoppingCartToDocument = await _VisitorManagement.PurchaseMyCart(VisitorToken, address, city, country, zip, purchaserName, paymentMethode, shipmentMethode, cardNumber, month, year, holder, ccv, id);
            //send to history
            _history.AddStoresPurchases(shoppingCartToDocument);
            if (_VisitorManagement.IsVisitorLoggedin(VisitorToken)) 
                _history.AddRegisterPurchases(shoppingCartToDocument, _VisitorManagement.GetRegisteredUsernameByToken(VisitorToken));
        }

        public ShoppingCart ViewMyCart(String authToken)
        {//II.2.4
            CheckIsVisitorAVisitor(authToken, "ViewMyCart");
            return _VisitorManagement.GetVisitorShoppingCart(authToken);
        }

        public void RemoveStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.5
            CheckIsVisitorLoggedIn(authToken, "RemoveStoreOwner");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.REMOVE_OWNER))
            {
                RecRemoveStoreOwner(appointerUsername, ownerUsername, storeName);
            }
        }
        private void RecRemoveStoreOwner(string appointer, string ownerApointee, string storeName)
        {
            Tuple<List<string>, List<string>> owners_managers = _storeManagement.RemoveStoreOwner(ownerApointee, storeName, appointer);
            _VisitorManagement.RemoveRole(ownerApointee, storeName);
            List<string> managers = owners_managers.Item2;
            foreach (string manager in managers)
            {
                if (_storeManagement.RemoveStoreManager(manager, storeName, ownerApointee))
                    _VisitorManagement.RemoveRole(manager, storeName);
            }
            List<string> owners = owners_managers.Item1;
            foreach (string owner in owners)
            {
                RecRemoveStoreOwner(ownerApointee, owner, storeName);
            }
        }
        public void RemoveStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.8
            CheckIsVisitorLoggedIn(authToken, "RemoveStoreManager");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.REMOVE_MANAGER))
            {
                if (_storeManagement.RemoveStoreManager(managerUsername, storeName, appointerUsername))
                {
                    _VisitorManagement.RemoveRole(managerUsername, storeName);
                    _dalController.RemoveStoreManager(managerUsername, storeName);
                }
            }
        }
        public ICollection<Tuple<DateTime, ShoppingCart>> GetMyPurchases(String authToken)
        {//II.3.7
            CheckIsVisitorLoggedIn(authToken, "GetMyPurchases");
            return _history.GetRegistreredPurchaseHistory(_VisitorManagement.GetRegisteredUsernameByToken(authToken));
        }
        public Registered GetVisitorInformation(String authToken)
        {
            CheckIsVisitorLoggedIn(authToken, "GetVisitorInformation");
            return _VisitorManagement.GetLoggedinRegistered(authToken);
        }

        internal void AppointSystemAdmin(String authToken, String adminUsername)
        {
            String registered = _VisitorManagement.GetRegisteredUsernameByToken(_VisitorManagement.GetRegisteredUsernameByToken(authToken));
            if (_VisitorManagement.CheckAccess(registered,null, Operation.APPOINT_SYSTEM_ADMIN))
            {
                _VisitorManagement.AppointSystemAdmin(adminUsername);
                _dalController.AppointSystemAdmin(adminUsername);
            }
                
        }

        /// <summary>
        /// <para> For Req II.3.8. </para>
        /// <para> Updates a Visitor's password if given the correct previous password.</para>
        /// </summary>
        /// <param name="authToken"> The authenticating token of the Visitor changing the password.</param>
        /// <param name="oldPassword"> The Visitor's current password. </param>
        /// <param name="newPassword"> The new updated password. </param>
        public void EditVisitorPassword(String authToken, String oldPassword, String newPassword)
        {
            _VisitorManagement.EditVisitorPassword(authToken, oldPassword, newPassword);
        }

        public List<StoreManager> getStoreManagers(String storeName, String authToken)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "getStoreManagers");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                errorMessage = $"this Visitor does not have permission to permorm this operation";
            if (errorMessage != null)
            {
                LogErrorMessage("getStoreManagers", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.getStoreManagers(storeName);
        }

        public ICollection<NotifyMessage> GetRegisteredMessagesNotofication(string authToken)
        {
            CheckIsVisitorLoggedIn(authToken, "GetRegisteredMessagesNotofication");
            return _VisitorManagement.GetRegisteredMessagesNotofication(authToken);
        }

        public List<StoreOwner> getStoreOwners(String storeName, String authToken)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "getStoreOwners");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                errorMessage = $"this Visitor does not have permission to permorm this operation";
            if (errorMessage != null)
            {
                LogErrorMessage("getStoreOwners", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.getStoreOwners(storeName);
        }

        public StoreFounder getStoreFounder(String storeName, String authToken)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "getStoreFounder");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                errorMessage = "this Visitor does not have permission to permorm this operation";
            if (errorMessage != null)
            {
                LogErrorMessage("getStoreFounder", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.getStoreFounder(storeName);
        }

        public void ExitSystem()
        {

        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in Visitor.</para>
        /// </summary>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the Visitor should use with the system.</returns>
        public String Login(String authToken, String Username, String password)
        {
            // TODO: Transfer cart?
            string token = _VisitorManagement.Login(authToken, Username, password);
            _VisitorManagement.AddRegisteredToPopulationStatistics(Username, DateTime.Now);
            return token;
        }

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out Visitor identified by authToken.</para>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor to log out.</param>
        public String Logout(String authToken)
        {
            return _VisitorManagement.Logout(authToken);
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered Visitor from our system and remove their roles from all relevant stores.</para>
        /// </summary>
        /// <param name="authToken"> The token authenticating the Visitor making the request.</param>
        /// <param name="usr_toremove"> The Visitor to remove and revoke the roles of.</param>
        public void RemoveRegisteredVisitor(String authToken, String usr_toremove)
        {
            String errorMessage = null;
            if (_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), null, Operation.CANCEL_SUBSCRIPTION)) // TODO: fix when checkAccess properly implemented
            {
                Registered registeredToRemove = _VisitorManagement.GetRegisteredVisitor(usr_toremove);

                _VisitorManagement.RemoveRegisteredVisitor(usr_toremove);
                _dalController.RemoveRegisteredVisitor(usr_toremove);
            }
            else
            {
                errorMessage = "Visitor tried to perform an unautherised operation - Cancel Subscription.";
                LogErrorMessage("RemoveRegisteredVisitor", errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public String EnterSystem() // Generating token and returning it
        { //II.1.1
            String token =  _VisitorManagement.EnterSystem();
            _dalController.AddVisitToPopulationStatistics(null, DateTime.Now);
            return token;
        }

        public void ExitSystem(String authToken) // Removing cart and token assigned to guest
        { //II.1.2
            _VisitorManagement.ExitSystem(authToken);
        }

        public void AddManagerPermission(String authToken, String managerUsername, String storeName, string op_name)
        {
            Operation op;
            if (_opNameToOp.ContainsKey(op_name))
            {
                op = _opNameToOp[op_name];
            }
            else
                throw new Exception("Unknown Permission!");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.IsRegistered(managerUsername))
            {
                throw new Exception("there is no such user in system as: " + managerUsername);
            }
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.CHANGE_MANAGER_PREMISSIONS))
                _VisitorManagement.AddManagerPermission(appointerUsername, managerUsername, storeName, op);
            else
            {
                throw new Exception("you don't have permission to modify manager permission...");
            }
        }
        public void HasPermission(string userToken, string storeName, string op_name)
        {
            Operation op;
            if (_opNameToOp.ContainsKey(op_name))
            {
                op = _opNameToOp[op_name];
            }
            else
                throw new Exception("the given operation name isn't exists.");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(userToken), storeName, op))
                throw new Exception("Access denied!");
        }

        public List<Store> GetStoresOfUser(String authToken)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "GetStoresOfUser");
            bool isAdmin = true;
            try
            {
                HasPermission(authToken, null, "PERMENENT_CLOSE_STORE");
            }
            catch (Exception e)
            {
                isAdmin = false;
            }
            string username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            List<string> stores = _VisitorManagement.GetStoresOfUser(username);
            return _storeManagement.GetStoresByName(stores);
        }

        public List<Store> GetAllActiveStores(String authToken)
        {
            //NotifyMessageDTO notification = new NotifyMessageDTO("Store", "Title", "You did GetAllActiveStores", "ReceiverUsername", 0);
            //log.Info($"Sending notification to :{authToken}");
            //this._notificationHub.SendNotification(authToken, notification);
            String errorMessage = null;
            CheckIsVisitorAVisitor(authToken, "GetAllActiveStores");
            bool isAdmin = true;
            try
            {
                HasPermission(authToken, null, "PERMENENT_CLOSE_STORE");
            }
            catch (Exception e)
            {
                isAdmin = false;
            }
            return _storeManagement.GetAllActiveStores(isAdmin);
        }

        public void RemoveManagerPermission(String authToken, String managerUsername, String storeName, string opName)
        {
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_opNameToOp.ContainsKey(opName))
                throw new Exception("the input op_name is'nt exists!!!!");
            Operation op = _opNameToOp[opName];
            if (!_VisitorManagement.IsRegistered(managerUsername))
            {
                throw new Exception("there is no such user in system as: " + managerUsername);
            }
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.CHANGE_MANAGER_PREMISSIONS))
                _VisitorManagement.RemoveManagerPermission(appointerUsername, managerUsername, storeName, op);
            else
            {
                throw new Exception("no permission to remove manager permission.");
            }
        }
        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> If credentials are valid, register new Visitor.</para>
        /// </summary>
        /// <param name="authToken"> The token of the guest currently registering.</param>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        public void Register(String authToken, String Username, String password, DateTime birthDate)
        {//II.1.3
         // TODO: Transfer cart? (Same dillema as login)
            _VisitorManagement.Register(Username, password, birthDate);
        }

        /// <summary>
        /// <para> For Req II.3.6. </para>
        /// <para> Files a complaint to the current system admin.</para>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor filing the complaint. </param>
        /// <param name="cartID"> The cart ID relevant to the complaint. </param>
        /// <param name="message"> The message detailing the complaint. </param>
        public void FileComplaint(String authToken, int cartID, String message)
        {
            _VisitorManagement.FileComplaint(authToken, cartID, message);
        }

        public IDictionary<int, Complaint> GetRegisterdComplaints(string authToken)
        {
            return _VisitorManagement.GetRegisterdComplaints(authToken);
        }

        /// <summary>
        /// <para> For Req II.6.3. </para>
        /// <para> System admin replies to a complaint he received.</para>
        /// </summary>
        /// <param name="authToken"> The authorisation token of the system admin.</param>
        /// <param name="complaintID"> The ID of the complaint. </param>
        /// <param name="reply"> The response to the complaint. </param>
        public void ReplyToComplaint(String authToken, int complaintID, String reply)
        {
            _VisitorManagement.ReplyToComplaint(authToken, complaintID, reply);
        }

        private void CheckIsVisitorLoggedIn(String authToken, String functionName)
        {
            String errorMessage;
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
            {
                errorMessage = $"This user is no longer logged in.";
                LogErrorMessage(functionName, errorMessage);
                throw new Exception(errorMessage);
            }
        }

        internal double CalcCartActualPrice(String authToken)
        {
            CheckIsVisitorAVisitor(authToken, "CalcCartActualPrice");
            return _VisitorManagement.GetVisitorShoppingCart(authToken).getActualPrice();
        }

        private void CheckIsVisitorAVisitor(String authToken, String functionName)
        {
            String errorMessage;
            if (!_VisitorManagement.IsVisitorAVisitor(authToken))
            {
                errorMessage = $"This user is no longer a visitor in the system.";
                LogErrorMessage(functionName, errorMessage);
                throw new Exception(errorMessage);
            }
        }

        internal String GetCartReceipt(String authToken)
        {
            CheckIsVisitorAVisitor(authToken, "GetCartReceipt");
            return _VisitorManagement.GetVisitorShoppingCart(authToken).GetCartReceipt();
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in Market.{functionName}. Cause: {message}.");
        }


        private void setOPerationDictionary()
        {
            _opNameToOp.Add("MANAGE_STOCK", Operation.MANAGE_INVENTORY);
            _opNameToOp.Add("CHANGE_SHOP_AND_DISCOUNT_POLICY", Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY);
            _opNameToOp.Add("DEFINE_CONCISTENCY_CONSTRAINT", Operation.DEFINE_CONCISTENCY_CONSTRAINT);
            _opNameToOp.Add("APPOINT_OWNER", Operation.APPOINT_OWNER);
            _opNameToOp.Add("REMOVE_OWNER", Operation.REMOVE_OWNER);
            _opNameToOp.Add("APPOINT_MANAGER", Operation.APPOINT_MANAGER);
            _opNameToOp.Add("REMOVE_MANAGER", Operation.REMOVE_MANAGER);
            _opNameToOp.Add("CHANGE_MANAGER_PREMISSIONS", Operation.CHANGE_MANAGER_PREMISSIONS);
            _opNameToOp.Add("CLOSE_STORE", Operation.CLOSE_STORE);
            _opNameToOp.Add("REOPEN_STORE", Operation.REOPEN_STORE);
            _opNameToOp.Add("STORE_WORKERS_INFO", Operation.STORE_WORKERS_INFO);
            _opNameToOp.Add("RECEIVE_AND_REPLY_STORE_MESSAGE", Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            _opNameToOp.Add("STORE_HISTORY_INFO", Operation.STORE_HISTORY_INFO);
            _opNameToOp.Add("STORE_INFORMATION", Operation.STORE_INFORMATION);
            _opNameToOp.Add("PERMENENT_CLOSE_STORE", Operation.PERMENENT_CLOSE_STORE);
            _opNameToOp.Add("CANCEL_SUBSCRIPTION", Operation.CANCEL_SUBSCRIPTION);
            _opNameToOp.Add("RECEIVE_AND_REPLY_ADMIN_MESSAGE", Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE);
            _opNameToOp.Add("SYSTEM_STATISTICS", Operation.SYSTEM_STATISTICS);
            _opNameToOp.Add("APPOINT_SYSTEM_ADMIN", Operation.APPOINT_SYSTEM_ADMIN);
            _opNameToOp.Add("STOCK_EDITOR", Operation.STOCK_EDITOR);
        }

        public Item GetItem(string token, string storeName, int itemId)
        {
            string errorMessage = null;
            CheckIsVisitorAVisitor(token, "GetItem");
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetItem", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetItem(storeName, itemId);
        }

        public void AddStoreDiscount(String authToken, String storeName, String conditionString, String discountString)
        {
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY))
                errorMessage = "Visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("AddStoreDiscount", errorMessage);
                throw new Exception(errorMessage);
            }
            Discount discount = new DiscountParser(discountString, conditionString).Parse();
            _storeManagement.AddStoreDiscount(storeName, discount);
        }

        public void AddStorePurchasePolicy(string authToken, string storeName, string conditionString)
        {
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY))
                errorMessage = "Visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("AddStoreDiscount", errorMessage);
                throw new Exception(errorMessage);
            }
            Condition condition = new ConditionParser(conditionString).Parse();
            _storeManagement.AddStorePurchasePolicy(storeName, condition);
        }


        internal List<string> GetPaymentMethods()
        {
            return PurchaseProcess.GetInstance()._paymentHandlerProxy.GetPaymentMethods();
        }

        internal List<string> GetShipmentMethods()
        {
            return PurchaseProcess.GetInstance()._shippingHandlerProxy.GetShipmentMethods();
        }

        internal void ResetStoreDiscountPolicy(string authToken, string storeName)
        {
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY))
                errorMessage = "Visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("ResetStoreDiscountPolicy", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.ResetStoreDiscountPolicy(storeName);
        }
        internal void ResetStorePurchasePolicy(string authToken, string storeName)
        {
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY))
                errorMessage = "Visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("ResetStorePurchasePolicy", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.ResetStorePurchasePolicy(storeName);
        }

        internal List<string> GetDiscountPolicyStrings(string authToken, string storeName)
        {
            String errorMessage = null;
            if (!_VisitorManagement.IsVisitorAVisitor(authToken))
                errorMessage = "only visitors are entitled to execute this operation. enter system properly.";
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetDiscountPolicyStrings", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetDiscountPolicyStrings(storeName);
        }

        internal List<string> GetPurchasePolicyStrings(string authToken, string storeName)
        {
            String errorMessage = null;
            if (!_VisitorManagement.IsVisitorAVisitor(authToken))
                errorMessage = "only visitors are entitled to execute this operation. enter system properly.";
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetPurchasePolicyStrings", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetPurchasePolicyStrings(storeName);
        }

        internal void BidItemInStore(string authToken, string storeName, int itemId, int amount, double newPrice)
        {
            String errorMessage = null;
            if (!_VisitorManagement.IsVisitorAVisitor(authToken))
                errorMessage = "only visitors are entitled to execute this operation. enter system properly.";
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (errorMessage != null)
            {
                LogErrorMessage("BidItemInStore", errorMessage);
                throw new Exception(errorMessage);
            }
            String bidder = _VisitorManagement.IsVisitorLoggedin(authToken) ? _VisitorManagement.GetRegisteredUsernameByToken(authToken) : authToken;
            _storeManagement.BidItemInStore(storeName, itemId, amount, newPrice, bidder);
            _dalController.BidItemInStore(bidder, storeName, itemId, amount, newPrice);
            String title = $"A New Bid In '{storeName}'!";
            String message = $"a visitor entered a new bid for item id '{itemId}' at the '{storeName}' store.";
            List<String> involvedUsernames = _storeManagement.GetUsernamesWithPermissionInStore(storeName, Operation.STOCK_EDITOR);
            foreach (String username in involvedUsernames)
            {
                int id = _dalController.SendNotification(storeName, username, title, message);

                _VisitorManagement.SendNotificationMessageToRegistered(username, storeName, title, message, id);

            }
        }

        internal bool AcceptBid(string authToken, string storeName, int itemId, string bidder)
        {
            String errorMessage = null;
            String acceptor = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(acceptor, storeName, Operation.STOCK_EDITOR))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("AcceptBid", errorMessage);
                throw new Exception(errorMessage);
            }
            bool success = _storeManagement.AcceptBid(storeName, acceptor, itemId, bidder);
            _dalController.AcceptBid(storeName, acceptor, itemId, bidder, success);
            if (success)
            {
                String title = $"Your Bid Got Respond!";
                String message = $"your bid for item id '{itemId}' at the '{storeName}' store has an answer.";
                if (_VisitorManagement.IsRegistered(bidder))
                {
                    int id = _dalController.SendNotification(storeName, bidder, title, message);
                    _VisitorManagement.SendNotificationMessageToRegistered(bidder, storeName, title, message, id);

                }
                else _VisitorManagement.SendNotificationMessageToVisitor(bidder, storeName, title, message);
                return true;
            }
            else return false;
        }

        internal bool CounterOfferBid(string authToken, string storeName, int itemId, string bidder, double counterOffer)
        {
            String errorMessage = null;
            String acceptor = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(acceptor, storeName, Operation.STOCK_EDITOR))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("CounterOfferBid", errorMessage);
                throw new Exception(errorMessage);
            }
            bool success = _storeManagement.CounterOfferBid(storeName, acceptor, itemId, bidder, counterOffer);
            _dalController.CounterOffer(storeName, acceptor, itemId, bidder, counterOffer, success);
            if (success)
            {
                String title = $"Your Bid Got Respond!";
                String message = $"your bid for item id '{itemId}' at the '{storeName}' store has an answer.";
                if (_VisitorManagement.IsRegistered(bidder))
                {
                    int id = _dalController.SendNotification(storeName, bidder, title, message);
                    _VisitorManagement.SendNotificationMessageToRegistered(bidder, storeName, title, message, id);
                }
                else _VisitorManagement.SendNotificationMessageToVisitor(bidder, storeName, title, message);
                return true;
            }
            else return false;
        }

        internal void RejectBid(string authToken, string storeName, int itemId, string bidder)
        {
            String errorMessage = null;
            String rejector = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(rejector, storeName, Operation.STOCK_EDITOR))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("RejectBid", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.RejectBid(storeName, rejector, itemId, bidder);
            _dalController.RejectBid(storeName, rejector, itemId, bidder);
            String title = $"Your Bid Got Rejected!";
            String message = $"your bid for item id '{itemId}' at the '{storeName}' store is not approved.";
            if (_VisitorManagement.IsRegistered(bidder))
            {
                int id = _dalController.SendNotification(storeName, bidder, title, message);
                _VisitorManagement.SendNotificationMessageToRegistered(bidder, storeName, title, message, id);

            }
            else _VisitorManagement.SendNotificationMessageToVisitor(bidder, storeName, title, message);
        }

        internal List<String> GetUsernamesWithPermissionInStore(string authToken, string storeName, Operation op)
        {
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.STORE_WORKERS_INFO))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetUsernameWithPermissionInStore", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetUsernamesWithPermissionInStore(storeName, op);
        }

        internal List<Bid> GetBidsForStore(string authToken, string storeName)
        {
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.STOCK_EDITOR))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetBidsForStore", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetBidsForStore(storeName);
        }

        internal List<Bid> GetVisitorBidsAtStore(string authToken, string storeName)
        {
            String errorMessage = null;
            String bidder = _VisitorManagement.IsVisitorLoggedin(authToken) ? _VisitorManagement.GetRegisteredUsernameByToken(authToken) : authToken;
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetVisitorBidsAtStore", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetVisitorBidsAtStore(storeName, bidder);
        }
        public void AddStoreOwnerForTestPurposes(String authToken, String ownerUsername, String storeName)
        {//II.4.4
            CheckIsVisitorLoggedIn(authToken, "AddStoreOwner");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_OWNER))
            {
                if (!_VisitorManagement.IsRegistered(ownerUsername))
                {
                    throw new Exception("there is no register user in system with username: " + ownerUsername + ". you can only appoint register user to store role.");
                }
                StoreOwner newOwner = new StoreOwner(ownerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreOwnerForTestPurposes(newOwner, storeName) != null)
                {
                    _VisitorManagement.AddRole(ownerUsername, newOwner);
                    //_dalController.AddStoreOwner(ownerUsername, storeName, appointerUsername);
                }
            }
            else
            {
                throw new Exception("you don't have access to fire owner in store.");
            }
        }
        internal bool AcceptOwnerAppointment(string authToken, string storeName, string newOwner)
        {//II.4.4
            CheckIsVisitorLoggedIn(authToken, "AcceptOwnerAppointment");
            String errorMessage = null;
            String acceptor = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(acceptor, storeName, Operation.APPOINT_OWNER))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (!_VisitorManagement.IsRegistered(newOwner))
                errorMessage = $"there is no registered user in system with username: {newOwner}. you can only appoint registered user to store role.";
            if (errorMessage != null)
            {
                LogErrorMessage("AcceptOwnerAppointment", errorMessage);
                throw new Exception(errorMessage);
            }
            StoreOwner ownerRole = _storeManagement.AcceptOwnerAppointment(storeName, acceptor, newOwner);
            if (ownerRole != null)
            {
                _VisitorManagement.AddRole(newOwner, ownerRole);
                _dalController.AddStoreOwner(ownerRole.Username, ownerRole.StoreName, ownerRole.Appointer);
                return true;
            }
            else
            {
                _dalController.AcceptOwnerAppointment(newOwner, storeName, acceptor);
            }
            return false;
                
            //{
            //    String title = $"You're a new owner at store '{storeName}'!";
            //    String message = $"your appointment got approved. you now have owwner's permission at this store.";
            //    _VisitorManagement.SendNotificationMessageToRegistered(newOwner, storeName, title, message);
            //}
        }
        internal void RejectOwnerAppointment(string authToken, string storeName, string newOwner)
        {
            CheckIsVisitorLoggedIn(authToken, "RejectOwnerAppointment");
            String errorMessage = null;
            String rejector = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(rejector, storeName, Operation.APPOINT_OWNER))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("RejectOwnerAppointment", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.RejectOwnerAppointment(storeName, rejector, newOwner);
            _dalController.RejectOwnerAppointment(storeName, newOwner);
        }
        internal Dictionary<string, List<string>> GetStandbyOwnersInStore(string authToken, string storeName)
        {
            CheckIsVisitorLoggedIn(authToken, "GetStandbyOwnersInStore");
            String errorMessage = null;
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_storeManagement.isStoreActive(storeName))
                errorMessage = $"Store '{storeName}' is currently inactive.";
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.APPOINT_OWNER))
                errorMessage = "this visitor is not the entitled to execute this operation.";
            if (errorMessage != null)
            {
                LogErrorMessage("GetStandbyOwnersInStore", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storeManagement.GetStandbyOwnersInStore(storeName); 
        }

        public ICollection<PopulationStatistics> GetDailyPopulationStatistics(string authToken, DateTime dateTime)
        {
            CheckIsVisitorLoggedIn(authToken, "GetStandbyOwnersInStore");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);

            if (!_VisitorManagement.CheckAccess(Username, null, Operation.PERMENENT_CLOSE_STORE))
            {
                string  errorMessage = $"Visitor is not an admin.";
                LogErrorMessage("GetDailyPopulationStatistics", errorMessage);
                throw new Exception(errorMessage);
            }
           
            return _dalTRranslator.PopulationStatisticsListDalToDomain(_dalController.GetDailyPopulationStatistics(dateTime), dateTime);
        }
    }
}

