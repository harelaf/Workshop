﻿using MarketProject.Domain.PurchasePackage.DiscountPackage;
using MarketProject.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Shared;

namespace MarketProject.Domain
{
    public class Market
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private StoreManagement _storeManagement;
        private VisitorManagement _VisitorManagement;
        private History _history;
        private IDictionary<string, Operation> _opNameToOp;

        public Market()
        {
            _storeManagement = new StoreManagement();
            _VisitorManagement = new VisitorManagement();
            _history = new History();
            _opNameToOp = new Dictionary<string, Operation>();
            setOPerationDictionary();
        }
        
       
        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the Visitor as the current admin.</para>
        /// </summary>
        public void RestartSystem(String adminUsername, String adminPassword, String ipShippingService, String ipPaymentService)
        {//I.1
            _VisitorManagement.AdminStart(adminUsername, adminPassword);

            // Do starting system stuff with IPs

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
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    errorMessage = $"Store {storeName} is currently inactive.";
                if (errorMessage != null)
                {
                    LogErrorMessage("AddItemToCart", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
                    _VisitorManagement.AddItemToVisitorCart(VisitorToken, store, item, amount);
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
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_removed = _VisitorManagement.RemoveItemFromCart(VisitorToken, item, _storeManagement.GetStore(storeName));
            // now update store stock
            Store store = _storeManagement.GetStore(storeName);
            lock (store.Stock)
            {
                _storeManagement.UnreserveItemInStore(storeName, item, amount_removed);
            }
            return item;
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
            int amount_differnce = _VisitorManagement.GetUpdatingQuantityDifference(VisitorToken, item, _storeManagement.GetStore(storeName), newQuantity);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName))
                {
                    errorMessage = $"Store {storeName} is currently inactive.";
                    LogErrorMessage("UpdateQuantityOfItemInCart", errorMessage);
                    throw new Exception(errorMessage);
                }
                lock (store.Stock)
                {
                    if (amount_differnce == 0)
                        errorMessage = "Update Quantity Of Item In Cart faild: current quantity and new quantity are the same!";
                    if (errorMessage != null)
                    {
                        LogErrorMessage("UpdateQuantityOfItemInCart", errorMessage);
                        throw new Exception(errorMessage);
                    }
                    if (amount_differnce > 0)// add item to cart and remove it from store stock
                        _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
                    else//remove item from cart and add to store stock
                        _storeManagement.UnreserveItemInStore(storeName, item, -1 * amount_differnce);
                    _VisitorManagement.UpdateItemInVisitorCart(VisitorToken, store, item, newQuantity);
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
                    return _storeManagement.GetStoreInformation(storeName);
                
                else
                {
                    String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
                    if ( _VisitorManagement.CheckAccess(Username, storeName, Operation.STORE_INFORMATION))
                        return _storeManagement.GetStoreInformation(storeName);
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

        public void AddItemToStoreStock(String authToken, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "AddItemToStoreStock");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
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
                    _storeManagement.AddItemToStoreStock(storeName, itemID, name, price, description, category, quantity);
                }
            }
        }


        public void RemoveItemFromStore(String authToken, String storeName, int itemID)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "RemoveItemFromStore");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
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
            Store store = _storeManagement.GetStore(storeName);
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
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.CLOSE_STORE))
                    errorMessage = $"Store {storeName} is currently inactive and Visitor is not the owner.";
                else if (storeName.Equals(""))
                    errorMessage = "Invalid Input: Blank store name.";
                if (errorMessage != null)
                {
                    LogErrorMessage("CloseStore", errorMessage);
                    throw new Exception(errorMessage);
                }
                _storeManagement.CloseStore(storeName);
            }
            List<String> names = _storeManagement.GetStoreRolesByName(storeName);
            String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am sad to inform you that {storeName} is temporarily closing down. " +
                $"Your roles in the store will remain until we decide permanently close down." +
                $"Yours Truly," +
                $"{Username}.";
            foreach (String name in names)
            {
                SendNotification(storeName, name, title, message);
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
                if (errorMessage != null)
                {
                    LogErrorMessage("CloseStorePermanently", errorMessage);
                    throw new Exception(errorMessage);
                }
                List<String> names = _storeManagement.GetStoreRolesByName(storeName);
                String title = $"Store: {storeName} is permanently closing down: [{DateTime.Now.ToString()}].";
                String message = $"I am sad to inform you that {storeName} is closing down. " +
                    $"All of your roles have been revoked." +
                    $"Yours Truly," +
                    $"{Username}.";
                foreach (String name in names)
                {
                    _VisitorManagement.RemoveRole(name, storeName);
                    SendNotification(storeName, name, title, message);
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
            List<String> names = _storeManagement.GetStoreRolesByName(storeName);
            String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am happy to inform you that {storeName} is reopening. " +
                $"Your roles in the store stayed the same." +
                $"Yours Truly," +
                $"{Username}.";
            foreach (String name in names)
            {
                SendNotification(storeName, name, title, message);
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
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Item item = _storeManagement.GetItem(storeName, itemID);
            if (rating < 0 || rating > 10)
                errorMessage = "Rate should be beteen 0 to 10";
            else if (!_history.CheckIfVisitorPurchasedItemInStore(appointerUsername, storeName, item))
                errorMessage = "This Visitor has never bought item with id: " + itemID + " at " + storeName;
            if (errorMessage != null)
            {
                LogErrorMessage("RateItem", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.RateItem(appointerUsername, item, rating, review);
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

        public void SendMessageToStore(String authToken, String storeName, String title, String message, int id)
        {
            String errorMessage = null;
            CheckIsVisitorLoggedIn(authToken, "SendMessageToStore");
            string senderUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (errorMessage != null)
            {
                LogErrorMessage("SendMessageToStore", errorMessage);
                throw new Exception(errorMessage);
            }
            _storeManagement.SendMessageToStore(senderUsername, storeName, title, message, id);
        }

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
                senderUsername= _VisitorManagement.GetRegisteredUsernameByToken(userToken);
                if (_VisitorManagement.CheckAccess(senderUsername, null, Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE))
                {
                    errorMessage = "User " + senderUsername + " don't have permission to preform this operation.";
                }
            }
            
            if (errorMessage != null)
            {
                LogErrorMessage("SendMessageToRegisterd", errorMessage);
                throw new Exception(errorMessage);
            }
            _VisitorManagement.SendAdminMessageToRegistered(UsernameReciever,senderUsername, title, message);
        }
        private void SendNotification(string storeName, string usernameReciever, String title, String message)
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
            _VisitorManagement.SendNotificationMessageToRegistered(usernameReciever, storeName ,title, message);
        }
        //1. admin: sender, reciver, msg, title
        //2. notify:reciver, msg, title, store,
        //4. complaint: send recive
        public void AnswerStoreMesseage(string authToken, string receiverUsername, int msgID, string storeName,  string reply)
        {
            String errorMessage = null;
            MessageToStore msg = null;
            CheckIsVisitorLoggedIn(authToken, "AnswerStoreMesseage");
            string appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
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
            _VisitorManagement.SendStoreMessageReplyment(msg, appointerUsername,receiverUsername ,reply);
        }

        public Queue<MessageToStore> GetStoreMessages(String authToken, String storeName)
        {
            String username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if(_VisitorManagement.CheckAccess(username, storeName, Operation.RECEIVE_AND_REPLY_STORE_MESSAGE))
                return _storeManagement.GetStoreMessages(storeName);
            return null;
        }

        public void AddStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.6
            CheckIsVisitorLoggedIn(authToken, "AddStoreManager");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_MANAGER))
            {
                StoreManager newManager = new StoreManager(managerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreManager(newManager, storeName))
                    _VisitorManagement.AddRole(managerUsername, newManager); }
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

        public void AddStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.4
            CheckIsVisitorLoggedIn(authToken, "AddStoreOwner");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_OWNER))
            {
                StoreOwner newOwner = new StoreOwner(ownerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreOwner(newOwner, storeName))
                    _VisitorManagement.AddRole(ownerUsername, newOwner);
            }
        }

        public void PurchaseMyCart(String VisitorToken, String address, String city, String country, String zip, String purchaserName, String paymentMethode, String shipmentMethode)
        {//II.2.5
            CheckIsVisitorAVisitor(VisitorToken, "PurchaseMyCart");
            ShoppingCart shoppingCartToDocument = _VisitorManagement.PurchaseMyCart(VisitorToken, address, city, country, zip, purchaserName, paymentMethode, shipmentMethode);
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
                if (_storeManagement.RemoveStoreOwner(ownerUsername, storeName, appointerUsername))
                    _VisitorManagement.RemoveRole(ownerUsername, storeName);
            }
        }
        public void RemoveStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.8
            CheckIsVisitorLoggedIn(authToken, "RemoveStoreManager");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.REMOVE_MANAGER))
            {
                if (_storeManagement.RemoveStoreManager(managerUsername, storeName, appointerUsername))
                    _VisitorManagement.RemoveRole(managerUsername, storeName);
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
            return _VisitorManagement.GetRegisteredVisitor(_VisitorManagement.GetRegisteredUsernameByToken(authToken));
        }

        internal void AppointSystemAdmin(String authToken, String adminUsername)
        {
            String registered = _VisitorManagement.GetRegisteredUsernameByToken(_VisitorManagement.GetRegisteredUsernameByToken(authToken));
            if (_VisitorManagement.CheckAccess(registered, null, Operation.APPOINT_SYSTEM_ADMIN))
                _VisitorManagement.AppointSystemAdmin(adminUsername);
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
            return _VisitorManagement.Login(authToken, Username, password);
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
                _storeManagement.RemoveAllRoles(registeredToRemove);
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
            return _VisitorManagement.EnterSystem();
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
                throw new Exception("Manager can only have permiisions: 4.12: RECEIVE_AND_REPLY_STORE_MESSAGE or 4.13:STORE_HISTORY_INFO");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.CHANGE_MANAGER_PREMISSIONS))
                _VisitorManagement.AddManagerPermission(appointerUsername, managerUsername, storeName, op);

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
                HasPermission(authToken, "", "PERMENENT_CLOSE_STORE");
            }
            catch (Exception e)
            {
                isAdmin = false;
            }
            string username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            List<string> stores =_VisitorManagement.GetStoresOfUser(username);
            return _storeManagement.GetStoresByName(stores);
        }

        public List<Store> GetAllActiveStores(String authToken)
        {
            String errorMessage = null;
            CheckIsVisitorAVisitor(authToken, "GetAllActiveStores");
            bool isAdmin = true;
            try
            {
                HasPermission(authToken, "", "PERMENENT_CLOSE_STORE");
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
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.CHANGE_MANAGER_PREMISSIONS))
                _VisitorManagement.RemoveManagerPermission(appointerUsername, managerUsername, storeName, op);
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
        }

        public Item GetItem(string token,string storeName, int itemId)
        {
            string errorMessage=null;
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

        public void AddStoreDiscount(String authToken, String storeName, Discount discount)
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
            _storeManagement.AddStoreDiscount(storeName, discount);
        }
    }
}
