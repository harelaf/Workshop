using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Market
    {
        private StoreManagement _storeManagement;
        private VisitorManagement _VisitorManagement;
        private History _history;
        private Object _stockLock = new Object();
        private Object _storeLock = new Object();

        public Market()
        {
            _storeManagement = new StoreManagement();
            _VisitorManagement = new VisitorManagement();
            _history = new History();
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
            if (!_VisitorManagement.IsVisitorAVisitor(VisitorToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception("there is no store in system with the given storeid");
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    throw new Exception($"Store {storeName} is currently inactive.");
                lock (store.Stock)
                {
                    Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
                    _VisitorManagement.AddItemToVisitorCart(VisitorToken, store, item, amount);
                }
            }

        }

        public Item RemoveItemFromCart(String VisitorToken, int itemID, String storeName)
        {//II.2.4
            if (!_VisitorManagement.IsVisitorAVisitor(VisitorToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception("there is no store in system with the givn storeid");
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

        public void UpdateQuantityOfItemInCart(String VisitorToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            if (!_VisitorManagement.IsVisitorAVisitor(VisitorToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_differnce = _VisitorManagement.GetUpdatingQuantityDifference(VisitorToken, item, _storeManagement.GetStore(storeName), newQuantity);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    throw new Exception($"Store {storeName} is currently inactive.");
                lock (store.Stock)
                {
                    if (amount_differnce == 0)
                        throw new Exception("Update Quantity Of Item In Cart faild: current quantity and new quantity are the same!");
                    if (amount_differnce > 0)// add item to cart and remove it from store stock
                        _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
                    else//remove item from cart and add to store stock
                        _storeManagement.UnreserveItemInStore(storeName, item, -1 * amount_differnce);
                    _VisitorManagement.UpdateItemInVisitorCart(VisitorToken, store, item, newQuantity);
                }
            }
        }


        public void OpenNewStore(String token, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_VisitorManagement.IsVisitorLoggedin(token))
                throw new Exception($"Only registered Visitors are allowed to rate stores.");
            if (_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception($"A store with the name {storeName} already exists in the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(token);
            StoreFounder founder = new StoreFounder(Username, storeName);
            _VisitorManagement.AddRole(Username, founder);
            _storeManagement.OpenNewStore(founder, storeName, purchasePolicy, discountPolicy);
        }

        public Store GetStoreInformation(String authToken, String storeName)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("Only registered and logged in Visitors are allowed to perform this operation!");
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (_storeManagement.isStoreActive(storeName) || _VisitorManagement.CheckAccess(Username, storeName, Operation.STORE_INFORMATION))
                    return _storeManagement.GetStoreInformation(storeName);
                throw new Exception($"Store {storeName} is currently inactive and Visitor is not the owner.");
            }
        }

        public void RateStore(String authToken, String storeName, int rating, String review)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_history.CheckIfVisitorPurchasedInStore(Username, storeName))
                throw new Exception($"Visitor {Username} has never purchased in {storeName}.");
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Store name is blank.");
            if (rating < 0 || rating > 10)
                throw new Exception("Invalid Input: rating should be in the range [0, 10].");
            _storeManagement.RateStore(Username, storeName, rating, review);
        }

        public void AddItemToStoreStock(String authToken, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    throw new Exception($"Store {storeName} is currently inactive and Visitor is not the owner.");
                if (storeName.Equals(""))
                    throw new Exception("Invalid Input: Blank store name.");
                if (price < 0)
                    throw new Exception("Invalid Input: Price has to be at least 0.");
                if (name.Equals(""))
                    throw new Exception("Invalid Input: Blank item name.");
                if (quantity < 0)
                    throw new Exception("Invalid Input: Quantity has to be at least 0.");
                lock (store.Stock)
                {
                    _storeManagement.AddItemToStoreStock(storeName, itemID, name, price, description, category, quantity);
                }
            }
        }


        public void RemoveItemFromStore(String authToken, String storeName, int itemID)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    throw new Exception($"Store {storeName} is currently inactive and Visitor is not the owner.");
                if (storeName.Equals(""))
                    throw new Exception("Invalid Input: Blank store name.");
                lock (store.Stock)
                {
                    _storeManagement.RemoveItemFromStore(storeName, itemID);
                }
            }
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchasesHistory(String authToken, String storeName)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.STORE_HISTORY_INFO))
                throw new Exception($"This Visitor is not an admin or owner in {storeName}.");
            return _history.GetStorePurchaseHistory(storeName);
        }

        public void UpdateStockQuantityOfItem(String authToken, String storeName, int itemID, int newQuantity)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.MANAGE_INVENTORY))
                    throw new Exception($"Store {storeName} is currently inactive and Visitor is not the owner.");
                if (storeName.Equals(""))
                    throw new Exception("Invalid Input: Blank store name.");
                if (newQuantity < 0)
                    throw new Exception("Invalid Input: Quantity has to be at least 0.");
                lock (store.Stock)
                {
                    _storeManagement.UpdateStockQuantityOfItem(storeName, itemID, newQuantity);
                }
            }
        }

        public void CloseStore(String authToken, String storeName)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.CLOSE_STORE))
                    throw new Exception($"Store {storeName} is currently inactive and Visitor is not the owner.");
                if (storeName.Equals(""))
                    throw new Exception("Invalid Input: Blank store name.");
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
                SendMessageToRegisterd(storeName, name, title, message);
            }
            /*
             * if (!_VisitorManagement.CheckVisitorPermission(Username, STORE_FOUNDER))
             *     throw new Exception($"This Visitor is not the founder of {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            _storeManagement.CloseStore(storeName);
            // Send Alerts to all roles of [storeName]
        }
        public void CloseStorePermanently(String authToken, String storeName)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_VisitorManagement.CheckAccess(Username, storeName, Operation.PERMENENT_CLOSE_STORE))
                    throw new Exception($"Visitor is not an admin.");

                if (storeName.Equals(""))
                    throw new Exception("Invalid Input: Blank store name.");
                List<String> names = _storeManagement.GetStoreRolesByName(storeName);
                String title = $"Store: {storeName} is permanently closing down: [{DateTime.Now.ToString()}].";
                String message = $"I am sad to inform you that {storeName} is closing down. " +
                    $"All of your roles have been revoked." +
                    $"Yours Truly," +
                    $"{Username}.";
                foreach (String name in names)
                {
                    _VisitorManagement.RemoveRole(name, storeName);
                    SendMessageToRegisterd(storeName, name, title, message);
                }
                _storeManagement.CloseStorePermanently(storeName);
            }
        }

        public void ReopenStore(String authToken, String storeName)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("The given Visitor is no longer logged in to the system.");
            String Username = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            Store store = _storeManagement.GetStore(storeName);
            lock (store)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_VisitorManagement.CheckAccess(Username, storeName, Operation.REOPEN_STORE))
                    throw new Exception($"Store {storeName} is currently inactive and Visitor is not the owner.");

                if (storeName.Equals(""))
                    throw new Exception("Invalid Input: Blank store name.");
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
                SendMessageToRegisterd(storeName, name, title, message);
            }
        }

        public void EditItemPrice(String authToken, String storeName, int itemID, double newPrice)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken)) {
                throw new Exception("the given Visitor is not a visitor in the system");
            }
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
            {
                throw new Exception("the given Visitor is not a stock owner of the given store");
            }
            _storeManagement.EditItemPrice(storeName, itemID, newPrice);
        }
        public void EditItemName(String authToken, String storeName, int itemID, String newName)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
            {
                throw new Exception("the given Visitor is not a visitor in the system");
            }
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
            {
                throw new Exception("the given Visitor is not a stock owner of the given store");
            }
            _storeManagement.EditItemName(storeName, itemID, newName);
        }
        public void EditItemDescription(String authToken, String storeName, int itemID, String newDescription)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
            {
                throw new Exception("the given Visitor is not a visitor in the system");
            }
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
            {
                throw new Exception("the given Visitor is not a stock owner of the given store");
            }
            _storeManagement.EditItemDescription(storeName, itemID, newDescription);
        }

        public void RateItem(String authToken, int itemID, String storeName, int rating, String review)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            //should check that this Visitor bought this item by his purches History
            /*if(rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException("Rate should be beteen 1 to 5");
            }*/
            Item item = _storeManagement.GetItem(storeName, itemID);
            if (!_history.CheckIfVisitorPurchasedItemInStore(appointerUsername, storeName, item))
            {
                throw new Exception("This Visitor has never bought item with id: " + itemID + " at " + storeName);
            }
            _storeManagement.RateItem(appointerUsername, item, rating, review);
        }

        public List<Item> GetItemInformation(String authToken, String itemName, String itemCategory, String keyWord)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.IsRegistered(appointerUsername))
            {
                throw new Exception("Visitor " + appointerUsername + " not found in system");
            }
            return _storeManagement.GetItemInformation(itemName, itemCategory, keyWord);
        }

        public void SendMessageToStore(String authToken, String storeName, String title, String message)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (!_VisitorManagement.IsRegistered(appointerUsername))
            {
                throw new Exception("Visitor " + appointerUsername + " not found in system");
            }
            _storeManagement.SendMessageToStore(appointerUsername, storeName, title, message);
        }

        public void SendMessageToRegisterd(String storeName, String UsernameReciever, String title, String message)
        {
            if (!_storeManagement.CheckStoreNameExists(storeName))
            {
                throw new Exception("Store " + storeName + " not found in system");
            }
            if (!_VisitorManagement.IsRegistered(UsernameReciever))
            {
                throw new Exception("Visitor " + UsernameReciever + " not found in system");
            }
            _VisitorManagement.SendMessageToRegistered(storeName, UsernameReciever, title, message);
        }

        public void AnswerStoreMesseage(String authToken, String storeName, String UsernameReciever, String title, String reply)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);

            if (!_VisitorManagement.IsRegistered(UsernameReciever))
            {
                throw new Exception("Visitor " + UsernameReciever + " not found in system");
            }
            _VisitorManagement.SendMessageToRegistered(storeName, UsernameReciever, title, reply);
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
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_MANAGER))
            {
                StoreManager newManager = new StoreManager(managerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreManager(newManager, storeName))
                    _VisitorManagement.AddRole(managerUsername, newManager); }
        }

        internal ICollection<MessageToRegistered> GetRegisteredMessages(string authToken)
        {
            return _VisitorManagement.getRegisteredMessages(authToken);
        }

        public void AddStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.4
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_OWNER))
            {
                StoreOwner newOwner = new StoreOwner(ownerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreOwner(newOwner, storeName))
                    _VisitorManagement.AddRole(ownerUsername, newOwner);
            }
        }

        public void PurchaseMyCart(String VisitorToken, String address, String city, String country, String zip, String purchaserName)
        {//II.2.5
            if (!_VisitorManagement.IsVisitorAVisitor(VisitorToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
            ShoppingCart shoppingCartToDocument = _VisitorManagement.PurchaseMyCart(VisitorToken, address, city, country, zip, purchaserName);
            //send to history
            _history.AddStoresPurchases(shoppingCartToDocument);
            if (_VisitorManagement.IsVisitorLoggedin(VisitorToken))
                _history.AddRegisterPurchases(shoppingCartToDocument, _VisitorManagement.GetRegisteredUsernameByToken(VisitorToken));
        }

        public ShoppingCart ViewMyCart(String authToken)
        {//II.2.4
            if (!_VisitorManagement.IsVisitorAVisitor(authToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
            return _VisitorManagement.GetVisitorShoppingCart(authToken);
        }

        public void RemoveStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.5
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.REMOVE_OWNER))
            {
                if (_storeManagement.RemoveStoreOwner(ownerUsername, storeName))
                    _VisitorManagement.RemoveRole(ownerUsername, storeName);
            }
        }
        public void RemoveStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.8
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            if (_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.REMOVE_MANAGER))
            {
                if (_storeManagement.RemoveStoreManager(managerUsername, storeName))
                    _VisitorManagement.RemoveRole(managerUsername, storeName);
            }
        }
        public ICollection<Tuple<DateTime, ShoppingCart>> GetMyPurchases(String authToken)
        {//II.3.7
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
            return _history.GetRegistreredPurchaseHistory(_VisitorManagement.GetRegisteredUsernameByToken(authToken));
        }
        public Registered GetVisitorInformation(String authToken)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is no longer a visitor in system");
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
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                throw new Exception($"this Visitor does not have permission to permorm this operation");
            return _storeManagement.getStoreManagers(storeName);
        }

        public List<StoreOwner> getStoreOwners(String storeName, String authToken)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                throw new Exception($"this Visitor does not have permission to permorm this operation");
            return _storeManagement.getStoreOwners(storeName);
        }

        public StoreFounder getStoreFounder(String storeName, String authToken)
        {
            if (!_VisitorManagement.IsVisitorLoggedin(authToken))
                throw new Exception("the given Visitor is not a visitor in the system");
            if (!_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                throw new Exception("this Visitor does not have permission to permorm this operation");
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
            if (_VisitorManagement.CheckAccess(_VisitorManagement.GetRegisteredUsernameByToken(authToken), null, Operation.CANCEL_SUBSCRIPTION)) // TODO: fix when checkAccess properly implemented
            {
                Registered registeredToRemove = _VisitorManagement.GetRegisteredVisitor(usr_toremove);
                _VisitorManagement.RemoveRegisteredVisitor(usr_toremove);
                _storeManagement.RemoveAllRoles(registeredToRemove);
            }
            else
            {
                throw new Exception("Visitor tried to perform an unautherised operation - Cancel Subscription.");
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

        public void AddManagerPermission(String authToken, String managerUsername, String storeName, Operation op)
        {
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
            if (_VisitorManagement.CheckAccess(appointerUsername, storeName, Operation.CHANGE_MANAGER_PREMISSIONS))
                _VisitorManagement.AddManagerPermission(appointerUsername, managerUsername, storeName, op);

        }

        public void RemoveManagerPermission(String authToken, String managerUsername, String storeName, Operation op)
        {
            String appointerUsername = _VisitorManagement.GetRegisteredUsernameByToken(authToken);
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
        public void Register(String authToken, String Username, String password)
        {//II.1.3
         // TODO: Transfer cart? (Same dillema as login)
            _VisitorManagement.Register(Username, password);
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
    }
}

