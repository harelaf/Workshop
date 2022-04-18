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

        public Boolean PurchaseMyCart(String username)
        {//II.2.5
            if (!_userManagement.IsUserAVisitor(username))
                throw new Exception("the given user is no longer a visitor in system");

            _userManagement.
            return false;
        }
    }
}
