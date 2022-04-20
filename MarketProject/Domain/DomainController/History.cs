using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class History
    {
        private IDictionary<String, List<Tuple<DateTime, ShoppingBasket>>> _storePurchaseHistory; //storeName:String
        private IDictionary<String, List<Tuple<DateTime, ShoppingCart>>> _registeredPurchaseHistory; //username:String

        public History()
        {
            _storePurchaseHistory = new Dictionary<String, List<Tuple<DateTime, ShoppingBasket>>>();
            _registeredPurchaseHistory = new Dictionary<String, List<Tuple<DateTime, ShoppingCart>>>();
        }

        public bool CheckIfUserPurchasedInStore(String username, String storeName)
        {
            if (!_registeredPurchaseHistory.ContainsKey(username))
                return false;

            List<Tuple<DateTime, ShoppingCart>> purchases = _registeredPurchaseHistory[username];
            foreach (Tuple<DateTime, ShoppingCart> purchase in purchases)
            {
                ShoppingBasket basket = purchase.Item2.GetShoppingBasket(storeName);
                if(basket != null)
                {
                    return true;
                }
            }
            return false;
        }


        public bool CheckIfUserPurchasedItemInStore(String username, String storeName, Item item)
        {
            if (!_registeredPurchaseHistory.ContainsKey(username))
                return false;

            List<Tuple<DateTime, ShoppingCart>> purchases = _registeredPurchaseHistory[username];
            foreach (Tuple<DateTime, ShoppingCart> purchase in purchases)
            {
                ShoppingBasket basket = purchase.Item2.GetShoppingBasket(storeName);
                if (basket != null)
                {
                    if (basket.isItemInBasket(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String storeName)
        {
            if (!_storePurchaseHistory.ContainsKey(storeName))
                throw new Exception($"There is purchase history for {storeName} yet.");
            return _storePurchaseHistory[storeName];
        }
    }
}
