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
            if (!_userManagement.isUserAVisitor(username))
                throw new Exception("the given user is no longer a visitor in system");
            if (!_storeManagement.IsStoreExist(storeName))
                throw new Exception("there is no store in system with the givn storeid");
            Item item = _storeManagement.ReserveItemFromStore(storeName, itemID, amount);
            _userManagement.addItemToUserCart(username, _storeManagement.GetStore(storeName), item, amount);
        }
    }
}
