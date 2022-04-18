using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class History
    {
        private IDictionary<String, ICollection<(DateTime, ShoppingBasket)>> _storePurchaseHistory; //storeName:String
        private IDictionary<String, ICollection<(DateTime, ShoppingCart)>> _registeredPurchaseHistory; //username:String

        // Harel: TODO: FINISH THIS FUNCTION.
        public bool CheckIfUserPurchasedInStore(String username, String storeName)
        {
            if (!_registeredPurchaseHistory.ContainsKey(username))
                return false;
            
            ICollection<(DateTime, ShoppingCart)> purchases = _registeredPurchaseHistory[username];

            return false;
        }

        public ICollection<(DateTime, ShoppingBasket)> GetStorePurchaseHistory(String storeName)
        {
            if (!_storePurchaseHistory.ContainsKey(storeName))
                throw new Exception($"There is purchase history for {storeName} yet.");
            return _storePurchaseHistory[storeName];
        }
    }
}
