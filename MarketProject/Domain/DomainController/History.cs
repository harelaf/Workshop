using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class History
    {
        private IDictionary<String, ICollection<Tuple<DateTime, ShoppingBasket>>> _storePurchaseHistory; //storeName:String
        private IDictionary<String, ICollection<Tuple<DateTime, ShoppingCart>>> _registeredPurchaseHistory; //username:String

        // Harel: TODO: FINISH THIS FUNCTION.
        public bool CheckIfUserPurchasedInStore(String username, String storeName)
        {
            if (!_registeredPurchaseHistory.ContainsKey(username))
                return false;

            ICollection<Tuple<DateTime, ShoppingCart>> purchases = _registeredPurchaseHistory[username];

            return false;
        }
        public ICollection<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String storeName)
        {
            if (!_storePurchaseHistory.ContainsKey(storeName))
                throw new Exception($"There is purchase history for {storeName} yet.");
            return _storePurchaseHistory[storeName];
        }

        public void AddStoresPurchases(ShoppingCart shoppingCart)
        {
            foreach (ShoppingBasket shoppingBasket in shoppingCart._shoppingBaskets)
            {
                String storeName = shoppingBasket.Store.GetName();
                if (!_storePurchaseHistory.ContainsKey(storeName))
                    _storePurchaseHistory.Add(storeName, new List<Tuple<DateTime, ShoppingBasket>>());
                _storePurchaseHistory[storeName].Add(new Tuple<DateTime, ShoppingBasket>(DateTime.Now, shoppingBasket));              
            }
        }
        public void AddRegisterPurchases(ShoppingCart shoppingCart, String username)
        {
            if (!_registeredPurchaseHistory.ContainsKey(username))
                _registeredPurchaseHistory.Add(username, new List<Tuple<DateTime, ShoppingCart>>());
            _registeredPurchaseHistory[username].Add(new Tuple<DateTime, ShoppingCart>(DateTime.Now, shoppingCart));

        }
    }
}
