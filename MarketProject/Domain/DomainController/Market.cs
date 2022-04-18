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

        /// <summary> 
        /// add\update basket eof store with item and amount.
        /// update store stock: itemAmount- amount
        /// </summary>
        /// <param name="username"></param>     --username should be a visitor in system.
        /// <param name="itemID"></param>       --item itemID should be an item of storeName
        /// <param name="storeName"></param>    --storeName should be a store in system
        /// <param name="amount"></param>       --storeName should have at least amount of itemID
        /// <exception cref="Exception"></exception>
        public void AddItemToCart(String username, int itemID, String storeName, int amount)
        {//II.2.3
            if (!_userManagement.IsUserAVisitor(username))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
            _userManagement.AddItemToUserCart(username, _storeManagement.GetStore(storeName), item, amount);
        }

        public Item RemoveItemFromCart(String username, int itemID, String storeName)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(username))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_removed= _userManagement.RemoveItemFromCart(username, item, _storeManagement.GetStore(storeName));
            // now update store stock
            _storeManagement.UnreserveItemInStore(storeName, item, amount_removed);
            return item;
        }

        public void UpdateQuantityOfItemInCart(String username, int itemID, String storeName, int newQuantity)
        {//II.2.4
            if (!_userManagement.IsUserAVisitor(username))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.GetItem(storeName, itemID);
            int amount_differnce = _userManagement.GetUpdatingQuanitityDiffrence(username, item, _storeManagement.GetStore(storeName), newQuantity);
            if (amount_differnce > 0)// add item to cart and remove it from store stock
                _storeManagement.ReserveItemFromStore(storeName, itemID, amount_differnce);
            else//remove item from cart and add to store stock
                _storeManagement.UnreserveItemInStore(storeName, item, amount_differnce);
            _userManagement.UpdateItemInUserCart(username, _storeManagement.GetStore(storeName), item, newQuantity);
        }

        public bool OpenNewStore(StoreFounder founder, String storeName, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            return _storeManagement.OpenNewStore(founder, storeName, purchasePolicy, discountPolicy);
        }

        public String GetStoreInformation(String storeName)
        {
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Blank store name.");
            return _storeManagement.GetStoreInformation(storeName);
        }

        public void RateStore(String username, String storeName, int rating, String review)
        {
            //TODO: add a function in History to check if [username] bought in [storeName].
            /*if (!_history.UserPurchasedInStore(String username, String storeName))
                return false;*/
            if (storeName.Equals(""))
                throw new Exception("Invalid Input: Store name is blank.");
            if (rating < 0 || rating > 10)
                throw new Exception("Invalid Input: rating should be in the range [0, 10].");
            _storeManagement.RateStore(username, storeName, rating, review);
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String username, String storeName)
        {
            if (!_storeManagement.CheckStoreNameExists(storeName))
                return null;
            return _history.GetStorePurchaseHistory(username, storeName);
        }
    }
}
