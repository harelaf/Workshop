using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Market
    {
        private StoreManagement _storeManagement;
        private UserManagement _userManagement;
        private History _history;
        private Object _stockLock = new Object();
        private Object _storeLock = new Object();

        public Market()
        {
            _storeManagement = new StoreManagement();
            _userManagement = new UserManagement();
            _history = new History();
        }

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the user as the current admin.</para>
        /// </summary>
        public void RestartSystem(String adminUsername, String adminPassword, String ipShippingService, String ipPaymentService)
        {//I.1
            _userManagement.AdminStart(adminUsername, adminPassword);

            // Do starting system stuff with IPs

        }

        /// add\update basket eof store with item and amount.
        /// update store stock: itemAmount- amount

        //--userToken should be a visitor in system.
        //--item itemID should be an item of storeName
        //--storeName should be a store in system
        //--storeName should have at least amount of itemID
        public void AddItemToCart(String userToken, int itemID, String storeName, int amount)
        {//II.2.3
            if (!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    throw new Exception($"Store {storeName} is currently inactive.");
                lock (_stockLock)
                {
                    Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
                    _userManagement.AddItemToUserCart(userToken, _storeManagement.GetStore(storeName), item, amount);
                }
            }
            
        }

        public Item RemoveItemFromCart(String userToken, int itemID, String storeName)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_removed = _userManagement.RemoveItemFromCart(userToken, item, _storeManagement.GetStore(storeName));
            // now update store stock
            lock (_stockLock)
            {
                _storeManagement.UnreserveItemInStore(storeName, item, amount_removed);
            }
            return item;
        }

        public void UpdateQuantityOfItemInCart(String userToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_differnce = _userManagement.GetUpdatingQuanitityDiffrence(userToken, item, _storeManagement.GetStore(storeName), newQuantity);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    throw new Exception($"Store {storeName} is currently inactive.");
                lock (_stockLock)
                {
                    if (amount_differnce > 0)// add item to cart and remove it from store stock
                        _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
                    else//remove item from cart and add to store stock
                        _storeManagement.UnreserveItemInStore(storeName, item, amount_differnce);
                    _userManagement.UpdateItemInUserCart(userToken, _storeManagement.GetStore(storeName), item, newQuantity);
                }
            }
        }


        public void OpenNewStore(String token, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_userManagement.IsUserLoggedin(token))
                throw new Exception($"Only registered users are allowed to rate stores.");
            if (_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception($"A store with the name {storeName} already exists in the system.");
            StoreFounder founder = null; // GET A FOUNDER SOMEHOW
            // Check if he is null or what...
            _storeManagement.OpenNewStore(founder, storeName, purchasePolicy, discountPolicy);
        }

        public String GetStoreInformation(String userToken, String storeName)
        {
            if (!_userManagement.IsUserLoggedin(userToken))
                throw new Exception("Only registered and logged in users are allowed to perform this operation!");
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            string userName = _userManagement.GetRegisteredUsernameByToken(userToken);
            lock (_storeLock)
            {
                if (_storeManagement.isStoreActive(storeName) || _userManagement.checkAccess(userName, storeName, Operation.STORE_INFORMATION))
                    return _storeManagement.GetStoreInformation(storeName);
                throw new Exception($"Store {storeName} is currently inactive.");
            }
        }

        public void RateStore(String username, String storeName, int rating, String review)
        {
            if (_userManagement.IsUserAVisitor(username))
                throw new Exception($"Only registered users are allowed to rate stores.");
            if (!_history.CheckIfUserPurchasedInStore(username, storeName))
                throw new Exception($"User {username} has never purchased in {storeName}.");
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Store name is blank.");
            if (rating < 0 || rating > 10)
                throw new Exception("Invalid Input: rating should be in the range [0, 10].");
            _storeManagement.RateStore(username, storeName, rating, review);
        }

        public void AddItemToStoreStock(String username, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, STORE_FOUNDER || STORE_OWNER))
             *     throw new Exception($"This user is not an owner in {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (price < 0)
                throw new Exception("Invalid Input: Price has to be at least 0.");
            if (name.Equals(""))
                throw new Exception("Invalid Input: Blank item nam.");
            if (quantity < 0)
                throw new Exception("Invalid Input: Quantity has to be at least 0.");
            lock (_stockLock)
            {
                _storeManagement.AddItemToStoreStock(storeName, itemID, name, price, description, category, quantity);
            }
        }


        public void RemoveItemFromStore(String username, String storeName, int itemID)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, STORE_FOUNDER || STORE_OWNER))
             *     throw new Exception($"This user is not an owner in {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            lock (_stockLock)
            {
                _storeManagement.RemoveItemFromStore(storeName, itemID);
            }
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchasesHistory(String username, String storeName)
        {
            //check user loggin!!!!!!!!!!!
            /*
             * if (!_userManagement.CheckUserPermission(username, STORE_FOUNDER || STORE_OWNER))
             *     throw new Exception($"This user is not an owner in {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            return _history.GetStorePurchaseHistory(storeName);
        }

        public void UpdateStockQuantityOfItem(String username, String storeName, int itemID, int newQuantity)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, STORE_FOUNDER || STORE_OWNER))
             *     throw new Exception($"This user is not an owner in {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (newQuantity < 0)
                throw new Exception("Invalid Input: Quantity has to be at least 0.");
            lock (_stockLock)
            {
                _storeManagement.UpdateStockQuantityOfItem(storeName, itemID, newQuantity);
            }
        }

        public void CloseStore(string username, String storeName)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, STORE_FOUNDER))
             *     throw new Exception($"This user is not the founder of {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            _storeManagement.CloseStore(storeName);
            // Send Alerts to all roles of [storeName]
        }

        public void ReopenStore(string username, String storeName)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, STORE_FOUNDER))
             *     throw new Exception($"This user is not the founder of {storeName}.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            _storeManagement.ReopenStore(storeName);
            // Send Alerts to all roles of [storeName]
        }

        public void CloseStorePermanently(String username, String storeName)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, SYSTEM_ADMIN))
             *     throw new Exception($"This user is not a system admin.");
             */
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            _storeManagement.CloseStorePermanently(storeName);
            // Remove all owners/managers...
            // Send alerts to all roles of [storeName]
        }


        public void EditItemPrice(String username, String storeName, int itemID, double newPrice)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, ???))
             *     throw new Exception($"This user is not the founder of {storeName}.");
             */
            _storeManagement.EditItemPrice(storeName, itemID, newPrice);
        }
        public void EditItemName(String username, String storeName, int itemID, int new_price, String newName)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, ???))
             *     throw new Exception($"This user is not the founder of {storeName}.");
             */
            _storeManagement.EditItemName(storeName, itemID, new_price, newName);
        }
        public void EditItemDescription(String username, String storeName, int itemID, String newDescription)
        {
            /*
             * if (!_userManagement.CheckUserPermission(username, ???))
             *     throw new Exception($"This user is not the founder of {storeName}.");
             */
            _storeManagement.EditItemDescription(storeName, itemID, newDescription);
        }

        public void RateItem(String username, int itemID, String storeName, int rating, String review)

        {
            //should check that this user bought this item by his purches History
            /*if(rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException("Rate should be beteen 1 to 5");
            }*/
            Item item = _storeManagement.GetItem(storeName, itemID);
            if (!_history.CheckIfUserPurchasedItemInStore(username, storeName, item))
            {
                throw new Exception("This user has never bought item with id: " + itemID + " at " + storeName);
            }
            _storeManagement.RateItem(username, item, rating, review);
        }

        public void GetItemInformation(String username, String itemName, String itemCategory, String keyWord)
        {
            if (!_userManagement.IsRegistered(username))
            {
                throw new Exception("User " + username + " not found in system");
            }

            //TODO ron -> complete
        }

        public void SendMessageToStore(String username, String storeName, String title, String message)
        {
            if (!_userManagement.IsRegistered(username))
            {
                throw new Exception("User " + username + " not found in system");
            }
            _storeManagement.SendMessageToStore(username, storeName, title, message);
        }

        public void SendMessageToRegisterd(String storeName, String usernameReciever, String title, String message)
        {
            if (!_storeManagement.CheckStoreNameExists(storeName))
            {
                throw new Exception("Store " + storeName + " not found in system");
            }
            if (!_userManagement.IsRegistered(usernameReciever))
            {
                throw new Exception("User " + usernameReciever + " not found in system");
            }
            _userManagement.SendMessageToRegisterd(storeName, usernameReciever, title, message);
        }

        public bool AddStoreManager(string appointerUsername, string managerUsername, string storeName)
        {//II.4.6
            if (_userManagement.checkAccess(appointerUsername, storeName, Operation.APPOINT_MANAGER))
            {
                StoreManager newManager = new StoreManager(managerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreManager(newManager, storeName))
                {
                    _userManagement.AddRole(managerUsername, newManager);
                    return true;
                }
            }
            return false;
        }

        public bool AddStoreOwner(string appointerUsername, string ownerUsername, string storeName)
        {//II.4.4
            if (_userManagement.checkAccess(appointerUsername, storeName, Operation.APPOINT_OWNER))
            {
                StoreOwner newOwner = new StoreOwner(ownerUsername, storeName, appointerUsername);
                if (_storeManagement.AddStoreOwner(newOwner, storeName))
                {
                    _userManagement.AddRole(ownerUsername, newOwner);
                    return true;
                }
            }
            return false;
        }

        public void PurchaseMyCart(String userToken, String address, String city, String country, String zip, String purchaserName)
        {//II.2.5
            if (!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            ShoppingCart shoppingCartToDocument = _userManagement.PurchaceMyCart(userToken, address, city, country, zip, purchaserName);
            //send to history
            _history.AddStoresPurchases(shoppingCartToDocument);
            if (_userManagement.IsUserLoggedin(userToken))
                _history.AddRegisterPurchases(shoppingCartToDocument, _userManagement.GetRegisteredUsernameByToken(userToken));
        }

        public ShoppingCart ViewMyCart(String authToken)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(authToken))
                throw new Exception("the given user is no longer a visitor in system");
            return _userManagement.GetUserShoppingCart(authToken);
        }

        public Boolean RemoveStoreOwner(String appointerUsername, String ownerUsername, String storeName)
        {//II.4.5
            if (_userManagement.checkAccess(appointerUsername, storeName, Operation.REMOVE_OWNER))
            {
                if (_storeManagement.RemoveStoreOwner(ownerUsername, storeName))
                {
                    _userManagement.RemoveRole(ownerUsername, storeName);
                    return true;
                }
            }
            return false;
        }
        public Boolean RemoveStoreManager(String appointerUsername, String managerUsername, String storeName)
        {//II.4.8
            if (_userManagement.checkAccess(appointerUsername, storeName, Operation.REMOVE_MANAGER))
            {
                if (_storeManagement.RemoveStoreManager(managerUsername, storeName))
                {
                    _userManagement.RemoveRole(managerUsername, storeName);
                    return true;
                }
            }
            return false;
        }
        public ICollection<Tuple<DateTime, ShoppingCart>> GetMyPurchases(String authToken)
        {//II.3.7
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is no longer a visitor in system");
            return _history.GetRegistreredPurchaseHistory(_userManagement.GetRegisteredUsernameByToken(authToken));
        }
        public Registered GetUserInformation(String authToken)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is no longer a visitor in system");
            return _userManagement.GetRegisteredUser(_userManagement.GetRegisteredUsernameByToken(authToken));
        }

        public void ExitSystem()
        {

        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in user.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the user should use with the system.</returns>
        public String Login(String authToken, String username, String password)
        {
            // TODO: Transfer cart?
            return _userManagement.Login(authToken, username, password);
        }

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out user identified by authToken.</para>
        /// </summary>
        /// <param name="authToken"> The token of the user to log out.</param>
        public String Logout(String authToken)
        {
            return _userManagement.Logout(authToken);
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered user from our system and remove their roles from all relevant stores.</para>
        /// </summary>
        /// <param name="authToken"> The token authenticating the user making the request.</param>
        /// <param name="usr_toremove"> The user to remove and revoke the roles of.</param>
        public void RemoveRegisteredUser(String authToken, String usr_toremove)
        {
            if (_userManagement.checkAccess(authToken, "CHANGE_ME", Operation.CANCEL_SUBSCRIPTION)) // TODO: fix when checkAccess properly implemented
            {
                Registered registeredToRemove = _userManagement.GetRegisteredUser(usr_toremove);
                _userManagement.RemoveRegisteredUser(usr_toremove);
                _storeManagement.RemoveAllRoles(registeredToRemove);
            }
        }

        public String EnterSystem() // Generating token and returning it
        { //II.1.1
            return _userManagement.enter();
        }

        public void ExitSystem(String authToken) // Removing cart and token assigned to guest
        { //II.1.2
            _userManagement.ExitSystem(authToken);
        }

        public void Register(String authToken, String username, String password)
        {//II.1.3
            _userManagement.Register(username, password);
        }
    }
}
