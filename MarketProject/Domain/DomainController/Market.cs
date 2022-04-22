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
            int amount_differnce = _userManagement.GetUpdatingQuantityDifference(userToken, item, _storeManagement.GetStore(storeName), newQuantity);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName))
                    throw new Exception($"Store {storeName} is currently inactive.");
                lock (_stockLock)
                {
                    if (amount_differnce == 0)
                        throw new Exception("Update Quantity Of Item In Cart faild: current quantity and new quantity are the same!");
                    if (amount_differnce > 0)// add item to cart and remove it from store stock
                        _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
                    else//remove item from cart and add to store stock
                        _storeManagement.UnreserveItemInStore(storeName, item, -1* amount_differnce);
                    _userManagement.UpdateItemInUserCart(userToken, _storeManagement.GetStore(storeName), item, newQuantity);
                }
            }
        }


        public void OpenNewStore(String authToken, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception($"Only registered users are allowed to rate stores.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception($"A store with the name {storeName} already exists in the system.");
            StoreFounder founder = new StoreFounder(username, storeName);
            _userManagement.AddRole(username, founder);
            _storeManagement.OpenNewStore(founder, storeName, purchasePolicy, discountPolicy);
        }

        public Store GetStoreInformation(String authToken, String storeName)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("Only registered and logged in users are allowed to perform this operation!");
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            string userName = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (_storeManagement.isStoreActive(storeName) || _userManagement.CheckAccess(userName, storeName, Operation.STORE_INFORMATION))
                    return _storeManagement.GetStoreInformation(storeName);
                throw new Exception($"Store {storeName} is currently inactive and user is not the owner.");
            }
        }

        public void RateStore(String authToken, String storeName, int rating, String review)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (!_history.CheckIfUserPurchasedInStore(username, storeName))
                throw new Exception($"User {username} has never purchased in {storeName}.");
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Store name is blank.");
            if (rating < 0 || rating > 10)
                throw new Exception("Invalid Input: rating should be in the range [0, 10].");
            _storeManagement.RateStore(username, storeName, rating, review);
        }

        public void AddItemToStoreStock(String authToken, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_userManagement.CheckAccess(username, storeName, Operation.MANAGE_INVENTORY))
                    throw new Exception($"Store {storeName} is currently inactive and user is not the owner.");
            }
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (price < 0)
                throw new Exception("Invalid Input: Price has to be at least 0.");
            if (name.Equals(""))
                throw new Exception("Invalid Input: Blank item name.");
            if (quantity < 0)
                throw new Exception("Invalid Input: Quantity has to be at least 0.");
            lock (_stockLock)
            {
                _storeManagement.AddItemToStoreStock(storeName, itemID, name, price, description, category, quantity);
            }
        }


        public void RemoveItemFromStore(String authToken, String storeName, int itemID)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_userManagement.CheckAccess(username, storeName, Operation.MANAGE_INVENTORY))
                    throw new Exception($"Store {storeName} is currently inactive and user is not the owner.");
            }
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            lock (_stockLock)
            {
                _storeManagement.RemoveItemFromStore(storeName, itemID);
            }
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchasesHistory(String authToken, String storeName)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_storeManagement.CheckStoreNameExists(storeName))
                throw new Exception($"Store {storeName} does not exist.");
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (_userManagement.CheckAccess(username, storeName, Operation.STORE_HISTORY_INFO))
                throw new Exception($"This user is not an admin or owner in {storeName}.");
            return _history.GetStorePurchaseHistory(storeName);
        }

        public void UpdateStockQuantityOfItem(String authToken, String storeName, int itemID, int newQuantity)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_userManagement.CheckAccess(username, storeName, Operation.MANAGE_INVENTORY))
                    throw new Exception($"Store {storeName} is currently inactive and user is not the owner.");
            }
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (newQuantity < 0)
                throw new Exception("Invalid Input: Quantity has to be at least 0.");
            lock (_stockLock)
            {
                _storeManagement.UpdateStockQuantityOfItem(storeName, itemID, newQuantity);
            }
        }

        public void CloseStore(string authToken, String storeName)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_userManagement.CheckAccess(username, storeName, Operation.CLOSE_STORE))
                    throw new Exception($"Store {storeName} is currently inactive and user is not the owner.");
            }
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            _storeManagement.CloseStore(storeName);
            List<String> names = _storeManagement.GetStoreRolesByName(storeName);
            String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am sad to inform you that {storeName} is temporarily closing down. " +
                $"Your roles in the store will remain until we decide permanently close down." +
                $"Yours Truly," +
                $"{username}.";
            foreach (String name in names)
            {
                SendMessageToRegisterd(storeName, name, title, message);
            }
        }

        public void ReopenStore(string authToken, String storeName)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (!_storeManagement.isStoreActive(storeName) && !_userManagement.CheckAccess(username, storeName, Operation.REOPEN_STORE))
                    throw new Exception($"Store {storeName} is currently inactive and user is not the owner.");
            }
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            _storeManagement.ReopenStore(storeName);
            List<String> names = _storeManagement.GetStoreRolesByName(storeName);
            String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am happy to inform you that {storeName} is reopening. " +
                $"Your roles in the store stayed the same." +
                $"Yours Truly," +
                $"{username}.";
            foreach (String name in names)
            {
                SendMessageToRegisterd(storeName, name, title, message);
            }
        }

        public void CloseStorePermanently(String authToken, String storeName)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("The given user is no longer logged in to the system.");
            String username = _userManagement.GetRegisteredUsernameByToken(authToken);
            lock (_storeLock)
            {
                if (!_userManagement.CheckAccess(username, storeName, Operation.PERMENENT_CLOSE_STORE))
                    throw new Exception($"User is not an admin.");
            }
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            List<String> names = _storeManagement.GetStoreRolesByName(storeName);
            String title = $"Store: {storeName} is permanently closing down: [{DateTime.Now.ToString()}].";
            String message = $"I am sad to inform you that {storeName} is closing down. " +
                $"All of your roles have been revoked." +
                $"Yours Truly," +
                $"{username}.";
            foreach (String name in names)
            {
                _userManagement.RemoveRole(name, storeName);
                SendMessageToRegisterd(storeName, name, title, message);
            }
            _storeManagement.CloseStorePermanently(storeName);
        }


        public void EditItemPrice(String authToken, String storeName, int itemID, double newPrice)
        {
            if (!_userManagement.IsUserLoggedin(authToken)) {
                throw new Exception("the given user is not a visitor in the system");
            }
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (!_userManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
            {
                throw new Exception("the given user is not a stock owner of the given store");
            }
            _storeManagement.EditItemPrice(storeName, itemID, newPrice);
        }
        public void EditItemName(String authToken, String storeName, int itemID, String newName)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
            {
                throw new Exception("the given user is not a visitor in the system");
            }
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (!_userManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
            {
                throw new Exception("the given user is not a stock owner of the given store");
            }
            _storeManagement.EditItemName(storeName, itemID, newName);
        }
        public void EditItemDescription(String authToken, String storeName, int itemID, String newDescription)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
            {
                throw new Exception("the given user is not a visitor in the system");
            }
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (!_userManagement.CheckAccess(appointerUsername, storeName, Operation.STOCK_EDITOR))
            {
                throw new Exception("the given user is not a stock owner of the given store");
            }
            _storeManagement.EditItemDescription(storeName, itemID, newDescription);
        }

        public void RateItem(String authToken, int itemID, String storeName, int rating, String review)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            //should check that this user bought this item by his purches History
            /*if(rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException("Rate should be beteen 1 to 5");
            }*/
            Item item = _storeManagement.GetItem(storeName, itemID);
            if (!_history.CheckIfUserPurchasedItemInStore(appointerUsername, storeName, item))
            {
                throw new Exception("This user has never bought item with id: " + itemID + " at " + storeName);
            }
            _storeManagement.RateItem(appointerUsername, item, rating, review);
        }

        public List<Item> GetItemInformation(String authToken, String itemName, String itemCategory, String keyWord)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (!_userManagement.IsRegistered(appointerUsername))
            {
                throw new Exception("User " + appointerUsername + " not found in system");
            }
            return _storeManagement.GetItemInformation(itemName, itemCategory, keyWord);
        }

        public void SendMessageToStore(String authToken, String storeName, String title, String message)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (!_userManagement.IsRegistered(appointerUsername))
            {
                throw new Exception("User " + appointerUsername + " not found in system");
            }
            _storeManagement.SendMessageToStore(appointerUsername, storeName, title, message);
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
            _userManagement.SendMessageToRegistered(storeName, usernameReciever, title, message);
        }

        public void AnswerStoreMesseage(String authToken, String storeName, String usernameReciever, String title, String reply)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            
            if (!_userManagement.IsRegistered(usernameReciever))
            {
                throw new Exception("User " + usernameReciever + " not found in system");
            }
            _userManagement.SendMessageToRegistered(storeName, usernameReciever, title, reply);
        }

        public bool AddStoreManager(string authToken, string managerUsername, string storeName)
        {//II.4.6
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (_userManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_MANAGER))
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

        public bool AddStoreOwner(string authToken, string ownerUsername, string storeName)
        {//II.4.4
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (_userManagement.CheckAccess(appointerUsername, storeName, Operation.APPOINT_OWNER))
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
            ShoppingCart shoppingCartToDocument = _userManagement.PurchaseMyCart(userToken, address, city, country, zip, purchaserName);
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

        public Boolean RemoveStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.5
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            string appointerUsername = _userManagement.GetRegisteredUsernameByToken(authToken);
            if (_userManagement.CheckAccess(appointerUsername, storeName, Operation.REMOVE_OWNER))
            {
                if (_storeManagement.RemoveStoreOwner(ownerUsername, storeName))
                {
                    _userManagement.RemoveRole(ownerUsername, storeName);
                    return true;
                }
            }
            return false;
        }

        public Boolean RemoveStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.8
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            if (_userManagement.CheckAccess(_userManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.REMOVE_MANAGER))
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

        /// <summary>
        /// <para> For Req II.3.8. </para>
        /// <para> Updates a user's password if given the correct previous password.</para>
        /// </summary>
        /// <param name="authToken"> The authenticating token of the user changing the password.</param>
        /// <param name="oldPassword"> The user's current password. </param>
        /// <param name="newPassword"> The new updated password. </param>
        public void EditUserPassword(String authToken, String oldPassword, String newPassword)
        {
            _userManagement.EditUserPassword(authToken, oldPassword, newPassword);
        }

        public List<StoreManager> getStoreManagers(string storeName, String authToken)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            if (!_userManagement.CheckAccess(_userManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                throw new Exception($"this user does not have permission to permorm this operation");
            return _storeManagement.getStoreManagers(storeName);
        }

        public List<StoreOwner> getStoreOwners(string storeName, String authToken)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            if (!_userManagement.CheckAccess(_userManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                throw new Exception($"this user does not have permission to permorm this operation");
            return _storeManagement.getStoreOwners(storeName);
        }

        public StoreFounder getStoreFounder(string storeName, String authToken)
        {
            if (!_userManagement.IsUserLoggedin(authToken))
                throw new Exception("the given user is not a visitor in the system");
            if (!_userManagement.CheckAccess(_userManagement.GetRegisteredUsernameByToken(authToken), storeName, Operation.STORE_WORKERS_INFO))
                throw new Exception("this user does not have permission to permorm this operation");
            return _storeManagement.getStoreFounder(storeName);
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
            if (_userManagement.CheckAccess(authToken, "CHANGE_ME", Operation.CANCEL_SUBSCRIPTION)) // TODO: fix when checkAccess properly implemented
            {
                Registered registeredToRemove = _userManagement.GetRegisteredUser(usr_toremove);
                _userManagement.RemoveRegisteredUser(usr_toremove);
                _storeManagement.RemoveAllRoles(registeredToRemove);
            }
        }

        public String EnterSystem() // Generating token and returning it
        { //II.1.1
            return _userManagement.EnterSystem();
        }

        public void ExitSystem(String authToken) // Removing cart and token assigned to guest
        { //II.1.2
            _userManagement.ExitSystem(authToken);
        }

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> If credentials are valid, register new user.</para>
        /// </summary>
        /// <param name="authToken"> The token of the guest currently registering.</param>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        public void Register(String authToken, String username, String password)
        {//II.1.3
            // TODO: Transfer cart? (Same dillema as login)
            _userManagement.Register(username, password);
        }

        /// <summary>
        /// <para> For Req II.3.6. </para>
        /// <para> Files a complaint to the current system admin.</para>
        /// </summary>
        /// <param name="authToken"> The token of the user filing the complaint. </param>
        /// <param name="cartID"> The cart ID relevant to the complaint. </param>
        /// <param name="message"> The message detailing the complaint. </param>
        public void FileComplaint(String authToken, int cartID, String message)
        {
            _userManagement.FileComplaint(authToken, cartID, message);  
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
            _userManagement.ReplyToComplaint(authToken, complaintID, reply);
        }
    }
}
