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
      
        public Market()
        {
            _storeManagement = new StoreManagement();
            _userManagement = new UserManagement();
            _history = new History();
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
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
            _userManagement.AddItemToUserCart(userToken, _storeManagement.GetStore(storeName), item, amount);
        }

        public Item RemoveItemFromCart(String userToken, int itemID, String storeName)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_removed= _userManagement.RemoveItemFromCart(userToken, item, _storeManagement.GetStore(storeName));
            // now update store stock
            _storeManagement.UnreserveItemInStore(storeName, item, amount_removed);
            return item;
        }

        public void UpdateQuantityOfItemInCart(String userToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_differnce = _userManagement.GetUpdatingQuanitityDiffrence(userToken, item, _storeManagement.GetStore(storeName), newQuantity);
            if (amount_differnce > 0)// add item to cart and remove it from store stock
                _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
            else//remove item from cart and add to store stock
                _storeManagement.UnreserveItemInStore(storeName, item, amount_differnce);
            _userManagement.UpdateItemInUserCart(userToken, _storeManagement.GetStore(storeName), item, newQuantity);
        }

        public void OpenNewStore(String username, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (_userManagement.IsUserAVisitor(username))
                throw new Exception($"Only registered users are allowed to rate stores.");
            StoreFounder founder = null; // GET A FOUNDER SOMEHOW
            // Check if he is null or what...
            _storeManagement.OpenNewStore(founder, storeName, purchasePolicy, discountPolicy);
        }

        public String GetStoreInformation(String username, String storeName)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            if (!_storeManagement.isStoreActive(storeName)) //&& !_userManagement.CheckUserPermission(username, SYSTEM_ADMIN || STORE_OWNER))
                throw new Exception($"Store {storeName} is currently inactive.");
            return _storeManagement.GetStoreInformation(storeName);
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

        public ICollection<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String username, String storeName)
        {
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
                throw new Exception("Invalif Input: Quantity has to be at least 0.");
            _storeManagement.UpdateStockQuantityOfItem(storeName, itemID, newQuantity);
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
        public void PurchaseMyCart(String userToken, String adrerss)
        {//II.2.5
            if(!_userManagement.IsUserAVisitor(userToken))
                throw new Exception("the given user is no longer a visitor in system");
            ShoppingCart shoppingCartToDocument = _userManagement.PurchaceMyCart(userToken, adrerss);
            //send to history
            _history.AddStoresPurchases(shoppingCartToDocument);
            if (_userManagement.IsUserLoggedin(userToken))
                _history.AddRegisterPurchases(shoppingCartToDocument, _userManagement.GetRegisteredUsernameByToken(userToken)); 
        }
    }
}
